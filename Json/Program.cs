using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using System.IO;

namespace JSON
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
    class Program
    {
        static string LoadSecutiryFrom(int start)
        {
            var url = string.Format("iss/securities.json?start={0}", start);
            return MoexDownloader.Load(url);
        }

        static Security[] LoadSecuritiesFrom(int start)
        {
            var result = LoadSecutiryFrom(start);
            var root = JsonConvert.DeserializeObject<Security>(result);
            var securities = root.securities.data.Select(
                d => new Security
                {
                    id = Convert.ToInt32(d[0]),
                    secid = d[1] as string,
                    shortname = d[2] as string,
                    regnumber = d[3] as string,
                    name = d[4] as string,
                    isin = d[5] as string,
                    is_traded = Convert.ToInt32(d[6]),
                    emitent_id = Convert.ToInt32(d[7]),
                    emitent_title = d[8] as string,
                    emitent_inn = d[9] as string,
                    emitent_okpo = d[10] as string,
                    gosreg = d[11] as string,
                    type = d[12] as string,
                    group = d[13] as string,
                    primary_boardid = d[14] as string,
                    marketprice_boardid = d[15] as string,
                }
                ).ToArray();
            return securities;
        }
        static HttpDownloader MoexDownloader = new HttpDownloader("https://iss.moex.com/");
        static void Main(string[] args)
        {
            string sec = Environment.CurrentDirectory + @"\sec.txt";
            List<Security> securities = new List<Security>();
            if (File.Exists(sec))
            {
                var path = JsonConvert.DeserializeObject<Security[]>(File.ReadAllText(sec));
                securities.AddRange(path);
                Console.WriteLine(securities.Count);
            }
            else
            {
                securities = LoadSecuritiesFrom(0).ToList();
                string file = JsonConvert.SerializeObject(securities);
                File.WriteAllText("sec.txt", file);
            }
        }

    }
}
