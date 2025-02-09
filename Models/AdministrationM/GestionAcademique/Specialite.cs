namespace gradeManagerServerAPi.Models.AdministrationM.GestionAcademique
{
    public class Specialite
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        // Clé étrangère vers Filière
        public int FiliereId { get; set; }
        public Filiere Filiere { get; set; }

    }

}