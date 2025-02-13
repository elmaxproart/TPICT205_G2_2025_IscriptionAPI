
namespace gradeManagerServerAPi.Data
{
    using gradeManagerServerAPi.Models.AdministrationM.GestionAcademique;
    using gradeManagerServerAPi.Models.paiement;
    using gradeManagerServerAPi.Models.StudentM;
    using gradeManagerServerAPi.Models.UserConnexion;
    using gradeManagerServerAPi.Services.UserService._2FA;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System.Reflection.Emit;


    namespace UserManagementAPI.Data
    {
        public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
            //utilisateurs
            public DbSet<UserInfo> UserInfos { get; set; }
            public DbSet<ValidationCode2FA> ValidationCodes { get; set; }

            // Tables Académiques
            public DbSet<Classe> Classes { get; set; }
            public DbSet<Departement> Departements { get; set; }
            public DbSet<Faculte> Facultes { get; set; }
            public DbSet<Filiere> Filieres { get; set; }
            public DbSet<Grade> Grades { get; set; }
            public DbSet<Niveau> Niveaux { get; set; }
            public DbSet<Specialite> Specialites { get; set; }
            public DbSet<UE> UEs { get; set; }
            // Tables Étudiant & Inscription
            public DbSet<Etudiant> Etudiants { get; set; }
            public DbSet<Inscription> Inscriptions { get; set; }
            //paiement
            public DbSet<Paiement> Paiements { get; set; }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);

                // Définir la relation entre User et UserInfo
                builder.Entity<UserInfo>()
                    .HasOne(ui => ui.User)
                    .WithOne()
                    .HasForeignKey<UserInfo>(ui => ui.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Configuration de la relation entre Classe et Filiere
                builder.Entity<Classe>()
                    .HasOne(c => c.Filiere)
                    .WithMany(f => f.Classes)
                    .HasForeignKey(c => c.FiliereId);

                // Configuration de la relation entre Classe et Specialite
                builder.Entity<Specialite>()
                    .HasOne(c => c.Filiere)
                    .WithMany(s => s.Specialites)
                    .HasForeignKey(c => c.FiliereId);

                // Configuration de la relation entre Classe et Niveau
                builder.Entity<Niveau>()
                    .HasOne(c => c.Filiere)
                    .WithMany(n => n.Niveaux)
                    .HasForeignKey(c => c.FiliereId);

                // Configuration de la relation entre Classe et Grade
                builder.Entity<Grade>()
                    .HasOne(c => c.Filiere)
                    .WithMany(g => g.Grades)
                    .HasForeignKey(c => c.FiliereId);

                // Configuration de la relation entre Departement et Faculte
                builder.Entity<Departement>()
                    .HasOne(d => d.Faculte)
                    .WithMany(f => f.Departements)
                    .HasForeignKey(d => d.FaculteId);

                // Configuration de la relation entre Filiere et Departement
                builder.Entity<Filiere>()
                    .HasOne(f => f.Departement)
                    .WithMany(d => d.Filieres)
                    .HasForeignKey(f => f.DepartementId);

                // Configuration de la relation entre UE et Classe
                builder.Entity<UE>()
                    .HasOne(u => u.Classe)
                    .WithMany(c => c.UEs)
                    .HasForeignKey(u => u.ClasseId);

                // Configuration de la relation entre Filiere et Grade
                builder.Entity<Filiere>()
                    .HasMany(f => f.Grades)
                    .WithOne(g => g.Filiere)
                    .HasForeignKey(g => g.FiliereId);

                // Configuration de la relation entre Filiere et Niveau
                builder.Entity<Filiere>()
                    .HasMany(f => f.Niveaux)
                    .WithOne(n => n.Filiere)
                    .HasForeignKey(n => n.FiliereId);

                // Configuration de la relation entre Filiere et Specialite
                builder.Entity<Filiere>()
                    .HasMany(f => f.Specialites)
                    .WithOne(s => s.Filiere)
                    .HasForeignKey(s => s.FiliereId);

                builder.Entity<Inscription>()
                           .HasOne(i => i.Etudiant) // Une inscription a un étudiant  
                           .WithMany(e => e.Inscriptions) // Un étudiant a plusieurs inscriptions  
                           .HasForeignKey(i => i.EtudiantId); // La clé étrangère
                                                              //inscription et ue.
          
                //class et etudiant
                builder.Entity<Etudiant>()
                    .HasOne(c=>c.Classe)
                    .WithMany(e =>e.Etudiants)
                    .HasForeignKey (c => c.ClasseId);


                builder.Entity<Paiement>()
               .Property(p => p.Montant)
               .HasColumnType("decimal(18,2)");

                builder.Entity<Paiement>()
               .HasKey(p => p.Id); // Définit Id comme clé primaire  

                // Configuration de la relation un-à-plusieurs entre Etudiant et Paiement  
                builder.Entity<Etudiant>()
                    .HasMany(e => e.Paiements)          // Un étudiant a plusieurs paiements  
                    .WithOne(p => p.Etudiant)            // Chaque paiement est pour un étudiant  
                    .HasForeignKey(p => p.EtudiantId)    // La clé étrangère dans Paiement  
                    .OnDelete(DeleteBehavior.Cascade);

            }

        }
    }

}

