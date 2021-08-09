using API.Helpers;
using AutoMapper;
using Core.Dtos.Basket;
using Core.Dtos.Orders;
using Core.Dtos.Products;
using Core.Dtos.User;
using Core.Entities.Basket;
using Core.Entities.Identity;
using Core.Entities.Order;
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

            //order dto
            CreateMap<AddressDto, Address>();
            CreateMap<Address, AddressDto>();
            CreateMap<Order, OrderToReturnDto>()
               .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
               .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());
        }
    }
}
