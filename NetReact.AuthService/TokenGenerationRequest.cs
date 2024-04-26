namespace NetReact.AuthService;

public record TokenGenerationRequest(Guid UserId, string Email, string Role);