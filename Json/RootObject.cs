using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSON
{
    public class RootObject
    {
        public Securities securities { get; set; }
    }
    public class Metadata
    {
        public Id id { get; set; }
        public ColumnMetadata secid { get; set; }
        public ColumnMetadata shortname { get; set; }
        public ColumnMetadata regnumber { get; set; }
        public ColumnMetadata name { get; set; }
        public ColumnMetadata isin { get; set; }
        public Is_Traded is_traded { get; set; }
        public Emitent_Id emitent_id { get; set; }
        public ColumnMetadata emitent_title { get; set; }
        public ColumnMetadata emitent_inn { get; set; }
        public ColumnMetadata emitent_okpo { get; set; }
        public ColumnMetadata gosreg { get; set; }
        public Type type { get; set; }
        public ColumnMetadata group { get; set; }
        public ColumnMetadata primary_boardid { get; set; }
        public ColumnMetadata marketprice_boardid { get; set; }
    }
    public class Security
    {
        public Securities securities { get; set; }
        public int id { get; set; }
        public string secid { get; set; }
        public string shortname { get; set; }
        public string regnumber { get; set; }
        public string name { get; set; }
        public string isin { get; set; }
        public int is_traded { get; set; }
        public int emitent_id { get; set; }
        public string emitent_title { get; set; }
        public string emitent_inn { get; set; }
        public string emitent_okpo { get; set; }
        public string gosreg { get; set; }
        public string type { get; set; }
        public string group { get; set; }
        public string primary_boardid { get; set; }
        public string marketprice_boardid { get; set; }
    }

    public class Securities
    {
        public string[] columns { get; set; }
        public object[][] data { get; set; }
    }

    public class Id
    {
        public string type { get; set; }
    }

    public class ColumnMetadata
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }
    public class Is_Traded
    {
        public string type { get; set; }
    }

    public class Emitent_Id
    {
        public string type { get; set; }
    }
}
