using gradeManagerServerAPi.Models.paiement;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace gradeManagerServerAPi.Models.StudentM
{
    public class Etudiant
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le matricule est obligatoire.")]
        public string Matricule { get; set; }

        
        [StringLength(50, ErrorMessage = "Le nom ne doit pas dépasser 50 caractères.")]
        public string Nom { get; set; }

       
        [StringLength(50, ErrorMessage = "Le prénom ne doit pas dépasser 50 caractères.")]
        public string Prenom { get; set; }

       
        public DateTime DateNaissance { get; set; }

        
        [EmailAddress(ErrorMessage = "L'email n'est pas valide.")]
        public string Email { get; set; }


        [Range(100000000, 999999999, ErrorMessage = "Le numéro doit être valide.")]
        public int Numero { get; set; }

        [RegularExpression("^[MF]$", ErrorMessage = "Le sexe doit être 'M' ou 'F'.")]
        public char Sexe { get; set; }
        public int ClasseId { get; set; }
        public Classe Classe { get; set; }
        [JsonIgnore]
        public virtual ICollection<Paiement> Paiements { get; set; } = new List<Paiement>();

        [JsonIgnore]
        public virtual ICollection<Inscription> Inscriptions { get; set; } = new List<Inscription>();
    }
}
