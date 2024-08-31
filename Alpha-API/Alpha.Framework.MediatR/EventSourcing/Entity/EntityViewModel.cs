using System;

namespace Alpha.Framework.MediatR.EventSourcing.Entity
{
    public class EntityViewModel
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
