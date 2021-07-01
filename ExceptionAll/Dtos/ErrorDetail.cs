namespace ExceptionAll.Dtos
{
    public class ErrorDetail
    {
        public string Title { get; }
        public string Message { get; }

        public ErrorDetail(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}