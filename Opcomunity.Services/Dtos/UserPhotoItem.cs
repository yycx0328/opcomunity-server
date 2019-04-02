using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Dtos
{
    public class UserPhotoItem
    {
        public long Id { get; set; }
        public int SortId { get; set; }
        public string ImageWebPath { get; set; }
        public string ThumbnailPath { get; set; }
    }
}
