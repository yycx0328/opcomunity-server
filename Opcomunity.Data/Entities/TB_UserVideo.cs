using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_UserVideo
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Extention { get; set; }
        public string ImgPath { get; set; }
        public int Views { get; set; }
        public int Praises { get; set; }
        public bool IsFree { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
