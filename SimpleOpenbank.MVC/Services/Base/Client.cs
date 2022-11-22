using System.Net.Http;

namespace SimpleOpenbank.MVC.Services.Base
{
    public partial class Client : IClient
    {


        public HttpClient HttpClient
        {
            get
            {
                return HttpClient;
            }
        }
    }
}
