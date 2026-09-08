namespace OnlineLearning.Core.Common
{
    public class DbResult
    {
        public int Result { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class DbResult<T> : DbResult
    {
        public T? Data { get; set; }
    }
}
