using Exchange.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Services
{
    class MarketLoader
    {
        public string LoadMarketFrom(string secId)
        {
            var url = string.Format("iss/engines/stock/markets/shares/boards/TQBR/securities/{0}.json", secId);
            return MoexDownloader.Load(url);
        }

        public Marketdata[] LoadMarket(string secId)
        {
            var result = LoadMarketFrom(secId);
            var root = JsonConvert.DeserializeAnonymousType(result, new { Marketdata = new RootObject() });
            var market = root.Marketdata.data.Select(
                d => new Marketdata
                {
                    secId = d[0] as string,
                    time = d[41] as string,
                    last = Convert.ToDouble(d[12]),
                    lastChange = Convert.ToDouble(d[25]),
                    valToday = Convert.ToDouble(d[29]),
                    volToday = Convert.ToDouble(d[28]),
                    value = Convert.ToDouble(d[13]),
                }
                ).ToArray();
            return market;
        }
        static HttpDownloader MoexDownloader = new HttpDownloader("https://iss.moex.com/");
    }
}
