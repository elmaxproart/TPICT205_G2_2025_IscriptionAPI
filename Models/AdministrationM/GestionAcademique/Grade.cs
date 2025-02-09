namespace gradeManagerServerAPi.Models.AdministrationM.GestionAcademique
{
    public class Grade
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        // Clé étrangère vers Filière
        public int FiliereId { get; set; }
        public Filiere Filiere { get; set; }
    }

}