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
    class SecutirysLoad
    {
        public string LoadSecutiryFrom(string start)
        {
            var url = string.Format("iss/engines/stock/markets/shares/boards/TQBR/securities/{0}.json", start);
            return MoexDownloader.Load(url);
        }

        public Security[] LoadSecuritiesFrom(string start)
        {
            var result = LoadSecutiryFrom(start);
            var root = JsonConvert.DeserializeAnonymousType(result, new { Securities = new RootObject() });
            var securities = root.Securities.data.Select(
                d => new Security
                {
                    secId = d[0] as string,
                    boardId = d[1] as string,
                    shortName = d[2] as string,
                    prevPrice = Convert.ToDouble(d[3], CultureInfo.InvariantCulture),
                    lotSize = Convert.ToDouble(d[4], CultureInfo.InvariantCulture),
                    faceValue = Convert.ToDouble(d[5], CultureInfo.InvariantCulture),
                    status = d[6] as string,
                    boardName = d[7] as string,
                    drcimals = Convert.ToDouble(d[8], CultureInfo.InvariantCulture),
                    secName = d[9] as string,
                    remarks = d[10] as string,
                    marketCod = d[11] as string,
                    instrId = d[12] as string,
                    sectorId = d[13] as string,
                    minStep = Convert.ToDouble(d[14], CultureInfo.InvariantCulture),
                    prevWarprice = Convert.ToDouble(d[15], CultureInfo.InvariantCulture),
                    faceUnit = d[16] as string,
                    prevDate = d[17] as string,
                    issueSize = Convert.ToDouble(d[18], CultureInfo.InvariantCulture),
                    isin = d[19] as string,
                    latName = d[20] as string,
                    regNumber = d[21] as string,
                    prevLegalClosePrice = Convert.ToDouble(d[22], CultureInfo.InvariantCulture),
                    prevadmittedquote = Convert.ToDouble(d[23], CultureInfo.InvariantCulture),
                    cerrencyId = d[24] as string,
                    secType = d[25] as string,
                    listLevel = Convert.ToDouble(d[26], CultureInfo.InvariantCulture),
                    settleDate = d[27] as string,
                }
                ).ToArray();
            return securities;
        }
        static HttpDownloader MoexDownloader = new HttpDownloader("https://iss.moex.com/");
    }
}
