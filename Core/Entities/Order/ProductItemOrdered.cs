using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Order
{

    [Owned]

    public class ProductItemOrdered
    {


        public ProductItemOrdered()
        {
        }

        public ProductItemOrdered(string productName, int productId, string pictureUrl)
        {
            ProductName = productName;
            ProductItemId = productId;
            PictureUrl = pictureUrl;
        }

        public int ProductItemId { get; set; }
        public string ProductName { get; set; }

        public string PictureUrl { get; set; }
    }
}
