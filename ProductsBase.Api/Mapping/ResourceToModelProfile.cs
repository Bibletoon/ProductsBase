using AutoMapper;
using ProductsBase.Api.Resources;
using ProductsBase.Domain.Models;

namespace ProductsBase.Api.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<UserResource, User>();
            CreateMap<UserCredentialResource, User>();
            
            CreateMap<SaveCategoryResource, Category>();

            CreateMap<SaveProductResource, Product>()
                .ForMember(src => src.UnitOfMeasurement,
                           opt =>
                               opt.MapFrom(src => (UnitOfMeasurement)src.UnitOfMeasurement));
        }
    }
}