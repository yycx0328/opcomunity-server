using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Dtos
{
    public class GiftItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Conver { get; set; }
        public int OriginalPrice { get; set; }
        public int DiscountPrice { get; set; }
    }
}
