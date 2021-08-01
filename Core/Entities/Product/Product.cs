using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities.Product
{
    public class Product : BaseEntity
    {
        [Required]
        [MaxLength(150, ErrorMessage = "Maximum length for the Title is 200 characters.")]
        public string Name { get; set; }

        [Required]

        [MaxLength(500, ErrorMessage = "Maximum length for the Title is 200 characters.")]
        public string Description { get; set; }

        [ForeignKey(nameof(Picture))]
        public List<Picture> Picture { get; set; }
        
        public double Price { get; set; }

        [ForeignKey(nameof(ProductType))]
        public int ProducTypeId { get; set; }
        public ProductType ProductType { get; set; }


        [ForeignKey(nameof(ProductBrand))]
        public int ProductBrandId { get; set; }
        public ProductBrand ProductBrand { get; set; }


    }
}
