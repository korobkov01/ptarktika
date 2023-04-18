using Exchange.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exchange.Services
{
    class PriceLoader
    {
        public string LoadPriceFrom(int start)
        {
            var url = string.Format("iss/statistics/engines/stock/currentprices.json?start={0}", start);
            return MoexDownloader.Load(url);
        }

        public CurrentPrice[] LoadPrices(int start)//
        {
            var result = LoadPriceFrom(start);
            var root = JsonConvert.DeserializeAnonymousType(result, new { CurrentPrices = new RootObject() });
            var prices = root.CurrentPrices.data.Select(
                d => new CurrentPrice
                {
                    tradeDate = Convert.ToDateTime(d[0]),
                    secId = d[1] as string,
                    boardId = d[2] as string,
                    tradeTime = Convert.ToDateTime(d[3]),
                    curPrice = Convert.ToDouble(d[4]),
                    lastPrice = Convert.ToDouble(d[5]),
                    legalClose = Convert.ToDouble(d[6]),
                }
                ).ToArray();
            return prices;
        }
        static HttpDownloader MoexDownloader = new HttpDownloader("https://iss.moex.com/");


        public List<CurrentPrice> LoadPrice()
        {
            string priceFile = Environment.CurrentDirectory + @"\Price.txt";
            List<CurrentPrice> securities = new List<CurrentPrice>();
            if (File.Exists(priceFile))
            {
                var path = JsonConvert.DeserializeObject<CurrentPrice[]>(File.ReadAllText(priceFile));
                securities.AddRange(path);
            }
            else
            {
                int threadCount = Environment.ProcessorCount * 10;
                ThreadPool.SetMinThreads(threadCount, threadCount);
                DateTime dt = DateTime.Now;
                for (int i = 0; ; i += 100 * threadCount)
                {
                    bool hasEmpty = false;
                    var allTasks = new Task<CurrentPrice[]>[threadCount];
                    Parallel.For(0, threadCount, (j) =>
                    {
                        allTasks[j] = Task.Run<CurrentPrice[]>(() => LoadPrices(i + j * 100));
                    });
                    foreach (var item in allTasks)
                    {
                        if (item.Result.Length == 0)
                        {
                            hasEmpty = true;
                            break;
                        }
                        securities.AddRange(item.Result);
                    }
                    if (hasEmpty) { break; }
                }
                string file = JsonConvert.SerializeObject(securities);
                File.WriteAllText("Price.txt", file);
            }
            return securities;
        }
    }
}
