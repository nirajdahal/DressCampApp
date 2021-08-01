using AutoMapper;
using Core.Dtos.Products;
using Core.Dtos.User;
using Core.Entities.Identity;
using Core.Entities.Product;


namespace API.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserForRegistrationDto, User>();
            CreateMap<ProductBrandDto, ProductBrand>();
            CreateMap<ProductTypeDto, ProductType>();
           
            CreateMap<ProductCreationDto, Product>();

            CreateMap<Picture, PictureDto>();
            CreateMap<PictureDto, Picture>();
            CreateMap<Product, ProductDto>()
                .ForMember(x => x.ProductBrand, o => o.MapFrom(p => p.ProductBrand.Name))
                .ForMember(x => x.ProductType, o => o.MapFrom(p => p.ProductType.Name));
        }
    }
}
