namespace Model.Interfaces
{
    public interface IValueResponse<T> : IResponse
    {
        T Value { get; set; }
    }
}
