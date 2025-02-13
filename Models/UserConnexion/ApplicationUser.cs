using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace gradeManagerServerAPi.Models.UserConnexion
{
    public class ApplicationUser : IdentityUser
    {
        public string NickName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [JsonIgnore]
        public string Title { get; set; }
        [JsonIgnore]
        public DateTime createAt { get; set; } = DateTime.Now;
        [JsonIgnore]
        public DateTime UpdateAt { get; set; }
        public bool Enable { get; set; }
    }
}
