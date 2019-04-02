using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Dtos
{
    public class BannerItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string Parameters { get; set; }
        public int SortId { get; set; }

    }
}
