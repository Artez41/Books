using Books.Api.Mapping;
using Books.Application.Services;
using Books.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Controllers
{
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet(ApiEndpoints.Books.GetAll)]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var books = await _bookService.GetAllAsync(token);
            var booksResponse = books.MapToResponse();

            return Ok(booksResponse);
        }

        [HttpGet(ApiEndpoints.Books.Get)]
        public async Task<IActionResult> Get([FromRoute] string idOrSlug, CancellationToken token)
        {
            var book = Guid.TryParse(idOrSlug, out var id)
                ? await _bookService.GetByIdAsync(id, token)
                : await _bookService.GetBySlugAsync(idOrSlug, token);

            if (book is null)
                return NotFound();

            return Ok(book.MapToResponse());
        }

        [HttpPost(ApiEndpoints.Books.Create)]
        public async Task<IActionResult> Create([FromBody] CreateBookRequest request, CancellationToken token)
        {
            var book = request.MapToBook();
            await _bookService.CreateAsync(book, token);

            return CreatedAtAction(nameof(Get), new { idOrSlug =  book.Id }, book);
        }

        [HttpPut(ApiEndpoints.Books.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateBookRequest request, CancellationToken token)
        {
            var book = request.MapToBook(id);
            var updatedBook = await _bookService.UpdateAsync(book, token);

            if (updatedBook is null)
                return NotFound();

            return Ok(book.MapToResponse());
        }
    }
}