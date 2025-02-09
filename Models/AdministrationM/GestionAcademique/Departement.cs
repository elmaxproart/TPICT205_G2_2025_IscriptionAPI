using System.Text.Json.Serialization;

namespace gradeManagerServerAPi.Models.AdministrationM.GestionAcademique
{
    public class Departement
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        // Clé étrangère vers la Faculté
        public int FaculteId { get; set; }
        public Faculte Faculte { get; set; }

        // Relation avec les Filières
        [JsonIgnore]
        public ICollection<Filiere> Filieres { get; set; }=new List<Filiere>();
    }

}