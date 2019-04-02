using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_StatisticsChannelUser
    {
        public System.DateTime Date { get; set; }
        public int Channel { get; set; }
        public int RegistCount { get; set; }
        public int ChargeMoney { get; set; }
    }
}
