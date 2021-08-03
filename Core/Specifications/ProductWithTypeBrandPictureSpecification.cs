using Core.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Specifications
{
    public class ProductWithTypeBrandPictureSpecification : BaseSpecification<Product>
    {
        public ProductWithTypeBrandPictureSpecification(ProductSpecParam productParam) : base(
           x =>
           (string.IsNullOrEmpty(productParam.Search) || x.Name.Contains(productParam.Search.ToLower())) &&
           (!productParam.BrandId.HasValue || x.ProductBrandId == productParam.BrandId) &&
           (!productParam.TypeId.HasValue || x.ProducTypeId == productParam.TypeId)

           )
        {
            AddInclude(x => x.ProductBrand);
            AddInclude(x => x.ProductType);
            AddInclude(x => x.Picture);

            if (!string.IsNullOrEmpty(productParam.Sort))
            {
                switch (productParam.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(x => x.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(x => x.Price);
                        break;
                    default:
                        AddOrderBy(x => x.Name);
                        break;
                }
            }

            ApplyPaging(productParam.PageSize * (productParam.PageIndex - 1), productParam.PageSize);
        }
    
        public ProductWithTypeBrandPictureSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductBrand);
            AddInclude(x => x.ProductType);
            AddInclude(x => x.Picture);
        }


    }
}
