using gradeManagerServerAPi.Models.AdministrationM.GestionAcademique;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Faculte
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Le champ Nom est obligatoire.")]
    public string Nom { get; set; }

    [JsonIgnore] 
    public ICollection<Departement>? Departements { get; set; } = new List<Departement>();
}

