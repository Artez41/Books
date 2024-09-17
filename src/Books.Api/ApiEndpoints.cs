﻿namespace Books.Api
{
    public static class ApiEndpoints
    {
        private const string ApiBase = "api";

        public static class Books
        {
            private const string Base = $"{ApiBase}/books";

            public const string Create = Base;
            public const string GetAll = Base;
            public const string Get = $"{Base}/{{idOrSlug}}";
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";

            public const string Rate = $"{Base}/{{id:guid}}/ratings";
            public const string DeleteRating = $"{Base}/{{id:guid}}/ratings";
        }

        public static class Auth
        {
            public const string Token = $"{ApiBase}/token";
        }

        public static class Ratings
        {
            private const string Base = $"{ApiBase}/ratings";

            public const string GetUserRatings = $"{Base}/me";
        }
    }
}
