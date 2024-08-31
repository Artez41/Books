﻿namespace Books.Api
{
    public static class ApiEndpoints
    {
        private const string ApiBase = "api";

        public static class Books
        {
            private const string Base = $"{ApiBase}/books";

            public const string Get = $"{Base}/{{id}}";
        }
    }
}
