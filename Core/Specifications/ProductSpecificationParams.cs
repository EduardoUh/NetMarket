﻿namespace Core.Specifications
{
    public class ProductSpecificationParams
    {
        public int? Brand { get; set; }
        public int? Category { get; set; }
        public string Sort { get; set; } = string.Empty;
        public int PageIndex { get; set; } = 1;

        private const int MaxPageSize = 50;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string Search { get; set; } = string.Empty;
    }
}
