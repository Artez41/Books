using Books.Application.Services;
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
    }
}
