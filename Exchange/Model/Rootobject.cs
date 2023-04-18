using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Model
{
    class RootObject
    {
        public string[] columns { get; set; }
        public object[][] data { get; set; }
    }
}
