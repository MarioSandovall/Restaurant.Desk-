using Model.Interfaces;
using Model.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions
{
    public static class HttpHelper
    {
        public static async Task<IValueResponse<T>> ReadHttpContentAsync<T>(this HttpResponseMessage response)
        {
            var result = new ValueResponse<T> { IsSuccess = response.IsSuccessStatusCode };
            if (result.IsSuccess)
            {
                result.Value = await response.Content.ReadAsAsync<T>();
            }
            else
            {
                var tuple = response.ConvertResponseMessage();
                result.Title = tuple.Item1;
                result.Message = tuple.Item2;
            }
            return result;
        }

        public static IResponse ReadHttpContentAsync(this HttpResponseMessage response)
        {
            var result = new Response { IsSuccess = response.IsSuccessStatusCode };
            if (!result.IsSuccess)
            {
                var tuple = response.ConvertResponseMessage();
                result.Title = tuple.Item1;
                result.Message = tuple.Item2;
            }
            return result;
        }

        public static Tuple<string, string> ConvertResponseMessage(this HttpResponseMessage response)
        {
            string message;
            string title;

            var httpErrorObject = response.Content.ReadAsStringAsync().Result;
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    title = "Restaurant System";
                    message = httpErrorObject.Trim('"', '/');
                    break;
                case HttpStatusCode.NotFound:
                    title = response.StatusCode.ToString();
                    message = $"{response.ReasonPhrase} : {response.RequestMessage} ";
                    break;
                case HttpStatusCode.InternalServerError:
                    title = response.StatusCode.ToString();
                    message = httpErrorObject;
                    break;
                default:
                    title = response.StatusCode.ToString();
                    message = "Error, Please Contact Your System Administrator";
                    break;
            }
            return Tuple.Create(title, message);
        }

        public static string ToQueryString<T>(this string url, T myObject)
        {
            if (myObject == null) return url;

            var properties = myObject.GetType().GetProperties();
            if (!properties.Any()) return url;

            var urlBuilder = new StringBuilder(url);
            urlBuilder.Append("?");
            var last = properties.Last();

            foreach (var property in myObject.GetType().GetProperties())
            {

                urlBuilder.Append($"{property.Name}={ property.GetValue(myObject)}");
                if (property.Name != last.Name) urlBuilder.Append("&");
            }

            return urlBuilder.ToString();
        }

    }
}
