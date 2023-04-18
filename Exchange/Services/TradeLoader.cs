using Exchange.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Services
{
    class TradeLoader
    {
        public string LoadTradeFrom(string AFLT)
        {
            var url = string.Format("iss/engines/stock/markets/shares/securities/{0}/trades.json", AFLT);
            return MoexDownloader.Load(url);
        }

        public Trades[] LoadTradesFrom(string AFLT)
        {
            var result = LoadTradeFrom(AFLT);
            var root = JsonConvert.DeserializeAnonymousType(result, new { Trades = new RootObject() });
            var trades = root.Trades.data.Select(
                d => new Trades
                {
                    secid = d[1] as string,
                    boardid = d[2] as string,
                    tradeTime = Convert.ToDateTime(d[3]),
                    price = Convert.ToDouble(d[4]),
                    quanitity = Convert.ToInt32(d[5]),
                    tradeName = d[6] as string,
                }
                ).ToArray();
            return trades;
        }
        static HttpDownloader MoexDownloader = new HttpDownloader("https://iss.moex.com/");
    }
}
