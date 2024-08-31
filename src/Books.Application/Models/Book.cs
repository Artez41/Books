using System.Text.RegularExpressions;

namespace Books.Application.Models
{
    public partial class Book
    {
        public required Guid Id { get; init; }
        public required string Title { get; init; }
        public string Slug => GenerateSlug();
        public required string Author { get; init; }
        public required string Description { get; init; }
        public required int YearOfRelease { get; init; }
        public required int NumberOfPages { get; init; }
        public required List<string> Genres { get; init; } = new();

        private string GenerateSlug()
        {
            var sluggedTitle = SlugRegex().Replace(Title, string.Empty)
                .ToLower().Replace(" ", "-");
            return $"{sluggedTitle}-{YearOfRelease}";
        }

        [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 5)]
        private static partial Regex SlugRegex();
    }
}
