using Alpha.Framework.MediatR.Auditorship.Domain.Enums;
using Alpha.Framework.MediatR.EventSourcing.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.Framework.MediatR.Auditorship.Domain.Entities
{
    public class Audit : Entity<Audit>
    {
        public OperationType OperationType { get; set; }
        public string RequestClassIdentification { get; set; }
        public string RequestPayload { get; set; }
        public string EntityClassIdentification { get; set; }
        public EntityId EntityId { get; set; }
        public string UserIdentification { get; set; }
    }
}
