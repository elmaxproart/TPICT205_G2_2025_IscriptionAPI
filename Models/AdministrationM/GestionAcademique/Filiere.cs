using System.Diagnostics;
using System.Text.Json.Serialization;

namespace gradeManagerServerAPi.Models.AdministrationM.GestionAcademique
{
    public class Filiere
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        // Clé étrangère vers le Département
        public int DepartementId { get; set; }
        public Departement Departement { get; set; }

        // Relation avec Spécialités, Niveaux et Grades
        [JsonIgnore]
        public ICollection<Specialite> Specialites { get; set; }=new List<Specialite>();
        [JsonIgnore]
        public ICollection<Niveau> Niveaux { get; set; }=new HashSet<Niveau>();
        [JsonIgnore]
        public ICollection<Grade> Grades { get; set; } =new HashSet<Grade>();
        [JsonIgnore]
        public ICollection<Classe> Classes { get; set; } =new List<Classe>();
    }

}