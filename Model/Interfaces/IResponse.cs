namespace Model.Interfaces
{
    public interface IResponse
    {
        bool IsSuccess { get; set; }
        string Message { get; set; }
        string Title { get; set; }
    }
}
