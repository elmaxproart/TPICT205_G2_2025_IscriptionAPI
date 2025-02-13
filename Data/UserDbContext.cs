using gradeManagerServerAPi.Models.UserConnexion;
using gradeManagerServerAPi.Services.UserService._2FA;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace gradeManagerServerAPi.Data
{
    public class UserDbContext : IdentityDbContext<ApplicationUser>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<ValidationCode2FA> ValidationCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserInfo>()
                .HasOne(ui => ui.User)
                .WithOne()
                .HasForeignKey<UserInfo>(ui => ui.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
