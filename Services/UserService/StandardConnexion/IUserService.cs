using gradeManagerServerAPi.Models.UserConnexion;
using Microsoft.IdentityModel.Tokens;

namespace gradeManagerServerAPi.Services.UserService.StandardConnexion
{
    public interface IUserService
    {
        string GenerateJwtToken(ApplicationUser user);


    }
}
