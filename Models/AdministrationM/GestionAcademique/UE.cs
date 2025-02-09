namespace gradeManagerServerAPi.Models.AdministrationM.GestionAcademique
{
    public class UE
    {
        public int Id { get; set; }
        public string Libelle { get; set; } // Nom en PascalCase
        public int Coef { get; set; }
        public int Semestre { get; set; }

        // Relation avec Classe
        public int ClasseId { get; set; }
        public Classe Classe { get; set; } // Navigation Property
    }
}
