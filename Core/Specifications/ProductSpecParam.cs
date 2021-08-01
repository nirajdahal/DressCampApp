using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
    public class ProductSpecParam
    {
        private const int MaxPageSize = 5;

        public int PageIndex { get; set; } = 1;

        private int _pageSize;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public int? TypeId { get; set; }

        public int? BrandId { get; set; }
        public string Sort { get; set; }
        public string Search { get; set; }
    }
}
