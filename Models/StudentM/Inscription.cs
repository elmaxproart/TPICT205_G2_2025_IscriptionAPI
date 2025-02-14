using gradeManagerServerAPi.Models.AdministrationM.GestionAcademique;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace gradeManagerServerAPi.Models.StudentM
{
    public class Inscription
    {
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

        [JsonIgnore]
        public int EtudiantId { get; set; }

        [JsonIgnore]
        public Etudiant Etudiant { get; set; }

        // Liste des UEs (au lieu d'un seul)
        public List<int> UeIds { get; set; } = new List<int>();

        [JsonIgnore]
        public List<UE> Ues { get; set; } = new List<UE>();
        [JsonIgnore]
        public DateTime DateInscription { get; set; }

        [JsonIgnore]
        public bool EstValide { get; set; }
        [JsonIgnore]
        public int semestre { get; set; }

   

    }
}
