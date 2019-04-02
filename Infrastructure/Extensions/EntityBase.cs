using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime? LastUpdatedTime { get; set; }

        public bool IsNew => this.Id == Guid.Empty;

        public void NewId()
        {
            this.Id = Guid.NewGuid();
        }

        protected EntityBase()
        {
            CreatedTime = DateTime.Now;
        }
    }
}
