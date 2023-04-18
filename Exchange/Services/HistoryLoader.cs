using Exchange.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Services
{
    class HistoryLoader 
    {
        public string LoadHistoriesFrom(string secId, string from, string till)
        {
            var url = string.Format("iss/history/engines/stock/markets/shares/boards/TQBR/securities/{0}.json?from={1}&till={2}", secId, from, till );
            return MoexDownloader.Load(url);
        }
        public History[] LoadHistory(string secId, string from, string till)
        {
            var result = LoadHistoriesFrom(secId, from, till);
            var root = JsonConvert.DeserializeAnonymousType(result, new { History = new RootObject() });
            var history = root.History.data.Select(     
                d => new History
                {
                    secId = d[3] as string,
                    close = Convert.ToDouble(d[14], CultureInfo.InvariantCulture),
                    tradeDate = d[1] as string,
                }
                ).ToArray();
            return history;
        }
        static HttpDownloader MoexDownloader = new HttpDownloader("https://iss.moex.com/");
    }
}
