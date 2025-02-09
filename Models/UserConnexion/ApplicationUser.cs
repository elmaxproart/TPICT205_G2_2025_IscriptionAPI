using Microsoft.AspNetCore.Identity;

namespace gradeManagerServerAPi.Models.UserConnexion
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        
    }
}
