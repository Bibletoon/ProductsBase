using AutoMapper;
using ProductsBase.Api.Resources;
using ProductsBase.Data.Models;
using ProductsBase.Data.Utility.Extensions;

namespace ProductsBase.Api.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Category, CategoryResource>();

            CreateMap<Product, ProductResource>()
                .ForMember(src => src.UnitOfMeasurement,
                           opt =>
                               opt.MapFrom(src => src.UnitOfMeasurement.ToDescriptionString()));

            CreateMap<Page<Product>, PageResource<ProductResource>>();
            CreateMap<Page<Category>, PageResource<CategoryResource>>();
        }
    }
}