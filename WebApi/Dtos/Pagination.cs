﻿namespace WebApi.Dtos
{
    public class Pagination<T> where T : class
    {
        public int Count { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public IReadOnlyList<T> Data { get; set; } = new List<T>();
        public int PageCount { get; set; }
    }
}
