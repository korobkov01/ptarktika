using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Model
{
    class CurrentPrice
    {
            public DateTime tradeDate { get; set; }
            public string boardId { get; set; }
            public string secId { get; set; }
            public DateTime tradeTime { get; set; }
            public double curPrice { get; set; }
            public double lastPrice { get; set; }
            public double legalClose { get; set; }
    }
}
