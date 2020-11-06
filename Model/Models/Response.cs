using Model.Interfaces;

namespace Model.Models
{
    public class Response : IResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
    }
}
