using Books.Application.Models;
using Books.Application.Repositories;
using FluentValidation;

namespace Books.Application.Validators
{
    public class BookValidator : AbstractValidator<Book>
    {
        private readonly IBookRepository _bookRepository;

        public BookValidator(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;

            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Title)
                .NotEmpty();

            RuleFor(x => x.Author)
                .NotEmpty();

            RuleFor(x => x.Genres)
                .NotEmpty();

            RuleFor(x => x.YearOfRelease)
                .LessThanOrEqualTo(DateTime.UtcNow.Year);

            RuleFor(x => x.NumberOfPages)
                .GreaterThan(0);

            RuleFor(x => x.Slug)
                .MustAsync(ValidateSlug)
                .WithMessage("This book already exists in the system");
        }

        private async Task<bool> ValidateSlug(Book book, string slug, CancellationToken token = default)
        {
            var existingBook = await _bookRepository.GetBySlugAsync(slug);

            if (existingBook is not null)
                return existingBook.Id == book.Id;

            return existingBook is null;
        }
    }
}
