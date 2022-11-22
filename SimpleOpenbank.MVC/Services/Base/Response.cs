namespace SimpleOpenbank.MVC.Services.Base
{
    public class Response<T>
    {
        public string Message { get; set; }
        public string ValidationErros { get; set; }
        public bool Success { get; set; }
        public T Data { get; set; }
    }
}
