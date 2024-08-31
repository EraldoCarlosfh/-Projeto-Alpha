using Alpha.Domain.Entities;
using Alpha.Framework.MediatR.Notifications;
using MediatR;

namespace Alpha.Domain.Commands.Products
{
    public class ProductCommand : OctaNotifiable, IRequest<ICommandResult<Product>>
    {
        public string Name { get; set; }
        public string BarCode { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
    }
}
