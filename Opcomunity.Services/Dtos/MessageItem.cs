using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Dtos
{
    public class MessageItem
    { 
        public long Id { get; set; } 
        public int Category { get; set; }
        public string CategoryDescription { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Parameters { get; set; }
        public long CrreateTime { get; set; }
    }
}
