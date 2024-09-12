﻿using Books.Api.Mapping;
using Books.Application.Services;
using Books.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetAll([FromQuery] GetAllBooksRequest request, CancellationToken token)
        {
            var options = request.MapToOptions();

            var books = await _bookService.GetAllAsync(options, token);
            var booksResponse = books.MapToResponse(options.Page, options.PageSize, 15);

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

        [Authorize]
        [HttpPost(ApiEndpoints.Books.Create)]
        public async Task<IActionResult> Create([FromBody] CreateBookRequest request, CancellationToken token)
        {
            var book = request.MapToBook();
            await _bookService.CreateAsync(book, token);

            return CreatedAtAction(nameof(Get), new { idOrSlug =  book.Id }, book);
        }

        [Authorize]
        [HttpPut(ApiEndpoints.Books.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateBookRequest request, CancellationToken token)
        {
            var book = request.MapToBook(id);
            var updatedBook = await _bookService.UpdateAsync(book, token);

            if (updatedBook is null)
                return NotFound();

            return Ok(book.MapToResponse());
        }

        [Authorize]
        [HttpDelete(ApiEndpoints.Books.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            var deleted = await _bookService.DeleteByIdAsync(id, token);

            if (!deleted)
                return NotFound();

            return Ok();
        }
    }
}