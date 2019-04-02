using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_AppVersion
    {
        public int Id { get; set; }
        public int Channel { get; set; }
        public string Version { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string MinVersion { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime UpdateTime { get; set; }
    }
}
