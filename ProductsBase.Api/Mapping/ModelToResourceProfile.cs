using System.Linq;
using AutoMapper;
using ProductsBase.Api.Resources;
using ProductsBase.Api.Utility.Extensions;
using ProductsBase.Domain.Models;
using ProductsBase.Domain.Security.Tokens;

namespace ProductsBase.Api.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<User, UserResource>().ForMember(u => u.Roles, opt => opt.MapFrom(u => u.Roles.Select(ur => ur.Name)));

            CreateMap<AccessToken, AccessTokenResource>()
                .ForMember(x => x.AccessToken, opt => opt.MapFrom(src => src.Token))
                .ForMember(src => src.RefreshToken,
                           opt => opt.MapFrom(src => src.RefreshToken.Token));

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