using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gradeManagerServerAPi.Models.UserConnexion
{
    public class UserInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
    }

    public class RegisterModel
    {
        public string FullName { get; set; }  // Nom complet
        public string FirstName { get; set; } // Prénom
        public string LastName { get; set; }  // Nom de famille
        public string Email { get; set; }     // Email
        public string Password { get; set; }  // Mot de passe
        public string Title { get; set; }     // Titre (ex: Mr, Mme)
    }


    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
