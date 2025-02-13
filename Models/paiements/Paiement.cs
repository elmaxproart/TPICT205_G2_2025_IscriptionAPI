namespace gradeManagerServerAPi.Models.paiement
{
    using gradeManagerServerAPi.Models.StudentM;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    public class Paiement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Matricule { get; set; } 

        [Required]
        public decimal Montant { get; set; }

        [Required]
        public bool Valide { get; set; } 

        [Required]
        public DateTime DatePaiement { get; set; } = DateTime.Now;


        [JsonIgnore]
        [ForeignKey("Etudiant")]
        public int EtudiantId { get; set; }

        [JsonIgnore]
        public virtual Etudiant Etudiant { get; set; }
    }

}
