using Core.Entities.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Dtos.Products
{
    public class ProductCreationDto
    {
        [Required]
        [MaxLength(150, ErrorMessage = "Maximum length for the Title is 200 characters.")]
        public string Name { get; set; }

        [Required]

        [MaxLength(500, ErrorMessage = "Maximum length for the Title is 500 characters.")]
        public string Description { get; set; }

       
        public List<PictureDto> Picture { get; set; }

        public double Price { get; set; }

        public ProductTypeDto ProductType { get; set; }
        public ProductBrandDto ProductBrand { get; set; }
    }
}
