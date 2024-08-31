using Alpha.Framework.MediatR.EventSourcing.Entity;

namespace Alpha.Api.ViewModels.Products
{
    public class ProductViewModel : EntityViewModel, IViewModel
    {
        public string Name { get; set; }
        public string BarCode { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
    }
}