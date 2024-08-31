using Alpha.Framework.MediatR.EventSourcing.Entity;

namespace Alpha.Domain.Entities
{
    public partial class Product : Entity<Product>
    {
        public string Name { get; private set; }
        public string BarCode { get; private set; }
        public string Image { get; private set; }
        public decimal? Price { get; private set; }
    }
}
