using AutoMapper;
using Core.Dtos.Products;
using Core.Dtos.User;
using Core.Entities.Identity;
using Core.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserForRegistrationDto, User>();
            CreateMap<ProductBrandDto, ProductBrand>();
            CreateMap<ProductTypeDto, ProductType>();
            CreateMap<PictureDto, Picture>();
            CreateMap<ProductCreationDto, Product>();
        }
    }
}
