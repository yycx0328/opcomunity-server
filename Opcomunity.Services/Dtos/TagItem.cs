using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Dtos
{ 
    public sealed partial class TagItem
    {
        public long TagId { get; set; }
        public string TagName { get; set; }
        public int SortId { get; set; }
    }
}
