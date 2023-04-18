using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Model
{
    class Trades
    {
        public string secid { get; set; }
        public string tradeName { get; set; }
        public string boardid { get; set; }
        public double price { get; set; }
        public int quanitity { get; set; }
        public DateTime tradeTime { get; set; }
    }
}
