using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_CityConfig
    {
        public int Id { get; set; }
        public string CityCode { get; set; }
        public string CityShortName { get; set; }
        public string CityFullName { get; set; }
        public string CityLocation { get; set; }
    }
}
