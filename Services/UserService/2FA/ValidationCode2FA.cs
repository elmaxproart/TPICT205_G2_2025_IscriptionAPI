namespace gradeManagerServerAPi.Services.UserService._2FA
{
    using gradeManagerServerAPi.Data.UserManagementAPI.Data;
    using gradeManagerServerAPi.Models.UserConnexion;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    public class ValidationCode2FA
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // Lien vers l'utilisateur

        [Required]
        public string HashedCode { get; set; } // Code haché

        [Required]
        public DateTime CreatedAt { get; set; } // Date de création

        [Required]
        public DateTime Expiration { get; set; } // Expire après 10 minutes

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } // Relation avec l'utilisateur
    }

}
