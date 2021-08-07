using API.Helpers;
using AutoMapper;
using Core.Dtos.Basket;
using Core.Dtos.Products;
using Core.Dtos.User;
using Core.Entities.Basket;
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

            CreateMap<Picture, PictureDto>()
                .ForMember(x => x.PictureUrl, p => p.MapFrom<PictureURLResorver>());
            CreateMap<PictureDto, Picture>();
            CreateMap<Product, ProductDto>()
                .ForMember(x => x.ProductBrand, o => o.MapFrom(p => p.ProductBrand.Name))
                .ForMember(x => x.ProductType, o => o.MapFrom(p => p.ProductType.Name));
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
        }
    }
}
