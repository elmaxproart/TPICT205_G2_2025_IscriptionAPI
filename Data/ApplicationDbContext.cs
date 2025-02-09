
namespace gradeManagerServerAPi.Data
{
    using gradeManagerServerAPi.Models.UserConnexion;
    using gradeManagerServerAPi.Services.UserService._2FA;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;


    namespace UserManagementAPI.Data
    {
        public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

            public DbSet<UserInfo> UserInfos { get; set; }
            public DbSet<ValidationCode2FA> ValidationCodes { get; set; }
            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);

                // Définir la relation entre User et UserInfo
                builder.Entity<UserInfo>()
                    .HasOne(ui => ui.User)
                    .WithOne()
                    .HasForeignKey<UserInfo>(ui => ui.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            }
        }
    }

}

