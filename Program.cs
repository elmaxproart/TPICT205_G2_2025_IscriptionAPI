using gradeManagerServerAPi.Data;
using gradeManagerServerAPi.Data.UserManagementAPI.Data;
using gradeManagerServerAPi.Models.UserConnexion;
using gradeManagerServerAPi.Services.UserService.Notification;
using gradeManagerServerAPi.Services.UserService.StandardConnexion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace gradeManagerServerAPi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuration de la BD avec MySQL
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

            // Configurer Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configuration de JWT
            var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            // Ajouter la politique CORS ici
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                {
                    policy.WithOrigins("https://localhost:5000")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials()   // Permet les cookies ou les informations d'identification
                          .SetPreflightMaxAge(TimeSpan.FromMinutes(10));  // Durée de validité du cache des requêtes pré-vol (preflight)
                });
            });

            // Ajouter les services nécessaires au conteneur
            builder.Services.AddAuthorization();
            builder.Services.AddControllers();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<UserManager<ApplicationUser>>();
            builder.Services.AddScoped<SignInManager<ApplicationUser>>();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddSingleton<IEmailSender, EmailSender>();
            builder.Services.AddScoped<ValidationCodeService>();

            // Configuration Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Inscription Module API",
                    Version = "v1",
                    Description = "API pour gérer l'inscription des utilisateurs et leurs informations supplémentaires.",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Support Inscription Module",
                        Email = "maxymtene40@gmail.com"
                    }
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                // Inclure les commentaires XML dans Swagger
                var xmlFile = "documentation.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            var app = builder.Build();

            // Créer les rôles et assigner les utilisateurs
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                string[] roles = { "Admin", "User" };

                // Vérifier et créer les rôles
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Assigner un utilisateur spécifique au rôle Admin si nécessaire
                var adminUser = await userManager.FindByEmailAsync("admin@example.com");
                if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }

                // Assigner un utilisateur spécifique au rôle User si nécessaire
                var user = await userManager.FindByEmailAsync("user@example.com");
                if (user != null && !await userManager.IsInRoleAsync(user, "User"))
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
            }

            // Configuration du pipeline de requêtes HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(); // Génère Swagger JSON
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inscription API V1");
                    c.RoutePrefix = string.Empty; // Affiche Swagger à la racine
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication(); // Utilisation de l'authentification JWT
            app.UseAuthorization();  // Utilisation de l'autorisation

            // Utiliser la politique CORS dans l'application
            app.UseCors("AllowSpecificOrigin");

            app.MapControllers(); // Mapping des contrôleurs

            app.Run(); // Lancer l'application
        }
    }
}
