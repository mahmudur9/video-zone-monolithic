namespace API.Domain.Interfaces
{
    public interface IAuthService
    {
        string GenerateToken(Guid userId, string email, string role);
    }
}
