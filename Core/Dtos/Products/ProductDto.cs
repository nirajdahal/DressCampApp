using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Dtos.Products
{
    public class ProductDto
    {

        public string Name { get; set; }

 
        public string Description { get; set; }

         public List<PictureDto> Picture { get; set; }
        public double Price { get; set; }


        public string ProductType { get; set; }


        public string ProductBrand { get; set; }
    }
}
