using Microsoft.AspNetCore.Identity;

namespace BlogSite.API.Repositories.Interfaces
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user , List<string> roles);
    }
}
