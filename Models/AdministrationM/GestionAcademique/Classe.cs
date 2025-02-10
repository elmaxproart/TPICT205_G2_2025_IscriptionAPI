using gradeManagerServerAPi.Models.AdministrationM.GestionAcademique;
using gradeManagerServerAPi.Models.StudentM;
using System.Text.Json.Serialization;

public class Classe
{
    public int Id { get; set; }
    public int FiliereId { get; set; }
    public int SpecialiteId { get; set; }
    public int NiveauId { get; set; }
    public int GradeId { get; set; }


    // Propriétés de navigation
    public virtual Filiere Filiere { get; set; }
    public virtual Specialite Specialite { get; set; }
    public virtual Niveau Niveau { get; set; }
    public virtual Grade Grade { get; set; }
    [JsonIgnore]
    public virtual ICollection<Etudiant> Etudiants { get; set; }=new HashSet<Etudiant>();
    [JsonIgnore]
    public virtual ICollection<UE> UEs { get; set; } = [];
    [JsonIgnore]
    public ICollection<Inscription> Inscriptions { get; set; } = new List<Inscription>();

}
