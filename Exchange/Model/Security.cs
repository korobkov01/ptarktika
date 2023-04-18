using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Model
{
    public class Security
    {
        public string secId { get; set; }
        public string boardId { get; set; }
        public string shortName { get; set; }
        public double prevPrice { get; set; }
        public double lotSize { get; set; }
        public double faceValue { get; set; }
        public string status { get; set; }
        public string boardName { get; set; }
        public double drcimals { get; set; }
        public string secName { get; set; }
        public string remarks { get; set; }
        public string marketCod { get; set; }
        public string instrId { get; set; }
        public string sectorId { get; set; }
        public double minStep { get; set; }
        public double prevWarprice { get; set; }
        public string faceUnit { get; set; }
        public string prevDate { get; set; }
        public double issueSize { get; set; }
        public string isin { get; set; }
        public string latName { get; set; }
        public string regNumber { get; set; }
        public double prevLegalClosePrice { get; set; }
        public double prevadmittedquote { get; set; }
        public string cerrencyId { get; set; }
        public string secType { get; set; }
        public double listLevel { get; set; }
        public string settleDate { get; set; }
    }
}
