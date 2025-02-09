using gradeManagerServerAPi.Models.AdministrationM.GestionAcademique;

namespace gradeManagerServerAPi.Models.StudentM
{
    public class Inscription
    {
        public int Id { get; set; }
        public int EtudiantId { get; set; }
        public Etudiant Etudiant { get; set; }

        public int ClasseId { get; set; }
        public Classe Classe { get; set; }

        public DateTime DateInscription { get; set; }
        public bool EstValide { get; set; } 
    }

}
