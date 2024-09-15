using Books.Api.Auth;
using Books.Application.Services;
using Books.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Controllers
{
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingsController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [Authorize]
        [HttpPost(ApiEndpoints.Books.Rate)]
        public async Task<IActionResult> RateBook([FromRoute] Guid id,
            [FromBody] RateBookRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var result = await _ratingService.RateBookAsync(id, userId!.Value, request.Rating, token);
            return result ? Ok() : NotFound();
        }
    }
}
