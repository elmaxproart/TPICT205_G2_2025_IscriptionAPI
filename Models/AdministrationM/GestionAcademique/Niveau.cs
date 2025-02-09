namespace gradeManagerServerAPi.Models.AdministrationM.GestionAcademique
{
    public class Niveau
    {
        public int Id { get; set; }
        public string Libelle { get; set; }

        // Clé étrangère vers Filière
        public int FiliereId { get; set; }
        public Filiere Filiere { get; set; }
    }

}