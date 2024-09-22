using Asp.Versioning;
using Books.Api.Auth;
using Books.Api.Mapping;
using Books.Application.Services;
using Books.Contracts.Requests;
using Books.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Books.Api.Controllers
{
    [ApiController]
    [ApiVersion(1.0)]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IOutputCacheStore _outputCacheStore;

        public BooksController(IBookService bookService, IOutputCacheStore outputCacheStore)
        {
            _bookService = bookService;
            _outputCacheStore = outputCacheStore;
        }

        [HttpGet(ApiEndpoints.Books.GetAll)]
        [OutputCache(PolicyName = "BookCache")]
        [ProducesResponseType(typeof(BooksResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllBooksRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var options = request.MapToOptions().WithUserId(userId);

            var books = await _bookService.GetAllAsync(options, token);
            var booksCount = await _bookService.GetCountAsync(options, token);

            var booksResponse = books.MapToResponse(options.Page, options.PageSize, booksCount);

            return Ok(booksResponse);
        }

        [HttpGet(ApiEndpoints.Books.Get)]
        [OutputCache(PolicyName = "BookCache")]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] string idOrSlug, CancellationToken token)
        {
            var book = Guid.TryParse(idOrSlug, out var id)
                ? await _bookService.GetByIdAsync(id, token)
                : await _bookService.GetBySlugAsync(idOrSlug, token);

            if (book is null)
                return NotFound();

            return Ok(book.MapToResponse());
        }

        [Authorize(AuthConstants.LibrarianPolicyName)]
        [HttpPost(ApiEndpoints.Books.Create)]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateBookRequest request, CancellationToken token)
        {
            var book = request.MapToBook();
            await _bookService.CreateAsync(book, token);

            await _outputCacheStore.EvictByTagAsync("books", token);

            return CreatedAtAction(nameof(Get), new { idOrSlug =  book.Id }, book);
        }

        [Authorize(AuthConstants.LibrarianPolicyName)]
        [HttpPut(ApiEndpoints.Books.Update)]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateBookRequest request, CancellationToken token)
        {
            var book = request.MapToBook(id);
            var userId = HttpContext.GetUserId();
            var updatedBook = await _bookService.UpdateAsync(book, userId, token);

            if (updatedBook is null)
                return NotFound();

            await _outputCacheStore.EvictByTagAsync("books", token);

            return Ok(book.MapToResponse());
        }

        [Authorize(AuthConstants.AdminUserPolicyName)]
        [HttpDelete(ApiEndpoints.Books.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            var deleted = await _bookService.DeleteByIdAsync(id, token);

            if (!deleted)
                return NotFound();

            await _outputCacheStore.EvictByTagAsync("books", token);

            return Ok();
        }
    }
}