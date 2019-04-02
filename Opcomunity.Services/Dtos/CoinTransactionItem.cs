using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Dtos
{
    public class CoinJournalItem
    {
        public long Id { get; set; }
        public long CoinCount { get; set; }
        public long CurrentCount { get; set; }
        public string IOStatus { get; set; }
        public int JournalType { get; set; }
        public string JournalDesc { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
