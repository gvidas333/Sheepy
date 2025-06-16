using Server.Models;

namespace Server.Services.Interfaces;

public interface ITokenService
{
    string CreateToken(ApplicationUser user);
}