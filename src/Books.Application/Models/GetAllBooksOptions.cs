﻿namespace Books.Application.Models
{
    public class GetAllBooksOptions
    {
        public Guid? UserId { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? SortField { get; set; }
        public SortOrder? SortOrder { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public enum SortOrder
    {
        Unsorted,
        Ascending,
        Descending
    }
}
