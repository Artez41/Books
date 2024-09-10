using Books.Application.Models;
using FluentValidation;

namespace Books.Application.Validators
{
    public class GetAllBooksOptionsValidator : AbstractValidator<GetAllBooksOptions>
    {
        public static readonly string[] AcceptableSortFields =
        {
            "yearofrelease"
        };

        public GetAllBooksOptionsValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 25)
                .WithMessage("You can get between 1 and 25 books per page");
        }
    }
}
