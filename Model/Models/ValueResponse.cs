using Model.Interfaces;

namespace Model.Models
{
    public class ValueResponse<T> : Response, IValueResponse<T>
    {
        public T Value { get; set; }
    }
}
