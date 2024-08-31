using Books.Application.Services;
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

        [HttpGet(ApiEndpoints.Books.Get)]
        public async Task<IActionResult> Get([FromRoute] string idOrSlug, CancellationToken token)
        {
            var book = Guid.TryParse(idOrSlug, out var id)
                ? await _bookService.GetByIdAsync(id, token)
                : await _bookService.GetBySlugAsync(idOrSlug, token);

            if (book is null)
                return NotFound();

            return Ok(book);
        }
    }
}