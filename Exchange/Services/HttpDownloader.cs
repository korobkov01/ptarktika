using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Exchange.Services
{
    class HttpDownloader
    {
        private readonly string address;
        HttpClient client = new HttpClient();
        public HttpDownloader(string address)
        {
            this.address = address;
            client.BaseAddress = new Uri(address);
        }

        public string Load(string method)
        {
            var task = client.GetStringAsync(method);
            return task.Result;
        }
    }
}
