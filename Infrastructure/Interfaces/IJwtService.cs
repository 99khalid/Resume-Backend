using Domain.Models;

namespace Infrastructure.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}