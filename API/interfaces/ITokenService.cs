using API.Entities;

namespace API.interfaces
{
    public interface ITokenService
    {
        string createToken(AppUser user);
    }
}