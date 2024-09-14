namespace Books.Contracts.Requests
{
    public class TokenGenerationRequest
    {
        public required Guid UserId { get; init; }
        public required string Email { get; init; }
        public required Dictionary<string, object> CustomClaims { get; init; } = new();
    }
}
