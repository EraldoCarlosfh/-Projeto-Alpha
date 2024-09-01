using Alpha.Api.ViewModels.Products;
using Alpha.Domain.Entities;
using AutoMapper;

namespace Alpha.Api.Mappings.Products
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();
        }
    }
}
