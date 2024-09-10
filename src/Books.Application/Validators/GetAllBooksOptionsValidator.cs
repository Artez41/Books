using Books.Application.Models;
using FluentValidation;

namespace Books.Application.Validators
{
    public class GetAllBooksOptionsValidator : AbstractValidator<GetAllBooksOptions>
    {
        public static readonly string[] AcceptableSortFields =
        {
            "yearofrelease", "title", "author", "numberofpages"
        };

        public GetAllBooksOptionsValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.SortField)
                .Must(x => x is null || AcceptableSortFields.Contains(x, StringComparer.OrdinalIgnoreCase))
                .WithMessage("You can only sort by 'yearofrelease', 'title', 'author' or 'numberofpages'");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 25)
                .WithMessage("You can get between 1 and 25 books per page");
        }
    }
}
