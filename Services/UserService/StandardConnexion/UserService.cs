using gradeManagerServerAPi.Models.UserConnexion;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace gradeManagerServerAPi.Services.UserService.StandardConnexion
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
        }

        public string GenerateJwtToken(ApplicationUser user)
        {
            // Récupérer les rôles de l'utilisateur
            var userRoles = _userManager.GetRolesAsync(user).Result;

            // Créer les informations de réclamation (claims) de l'utilisateur
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email)
    };

            // Ajouter les rôles de l'utilisateur
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Générer une clé aléatoire sécurisée (256 bits ou 512 bits)
            var key = GenerateSecureKey(256); // Choisir la taille de la clé : 256 bits ou 512 bits

            // Créer les credentials de signature pour le JWT
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Créer le token JWT
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],  // Définir l'émetteur
                _configuration["Jwt:Audience"],  // Définir l'audience
                claims,
                expires: DateTime.UtcNow.AddHours(2),  // Définir l'expiration du token
                signingCredentials: creds
            );

            // Retourner le token sous forme de chaîne
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Génère une clé secrète aléatoire sécurisée.
        /// </summary>
        /// <param name="keySizeInBits">La taille de la clé en bits (256 ou 512 bits)</param>
        /// <returns>SymmetricSecurityKey générée aléatoirement</returns>
        private SymmetricSecurityKey GenerateSecureKey(int keySizeInBits)
        {
            // Taille de la clé en octets
            int keySizeInBytes = keySizeInBits / 8;

            // Utilisation de RNGCryptoServiceProvider pour générer une clé sécurisée
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] keyBytes = new byte[keySizeInBytes];
                rng.GetBytes(keyBytes); // Remplir keyBytes avec des valeurs aléatoires

                return new SymmetricSecurityKey(keyBytes);
            }
        }
    }
}
