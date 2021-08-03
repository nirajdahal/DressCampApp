using AutoMapper;
using Core.Dtos.Products;
using Core.Entities.Product;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    public class PictureURLResorver : IValueResolver<Picture, PictureDto, string>
    {
        private readonly IConfiguration _config;

        public PictureURLResorver(IConfiguration config)
        {
            _config = config;
        }
        public string Resolve(Picture source, PictureDto destination, string destMember, ResolutionContext context)
        {
            return _config["AppURL"] + source.PictureUrl;
        }
    }
}
