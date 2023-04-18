using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Model
{
    public class History
    {
        public string secId { get; set; }
        public double close { get; set; }
        public float floatClose { get { return (float)close; } }
        public string tradeDate { get; set; }
    }
}
