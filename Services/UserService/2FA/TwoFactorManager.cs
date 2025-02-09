using AutoMapper;
using gradeManagerServerAPi.Data.UserManagementAPI.Data;
using gradeManagerServerAPi.Models.UserConnexion;
using gradeManagerServerAPi.Services.UserService.Notification;
using gradeManagerServerAPi.Services.UserService.StandardConnexion;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gradeManagerServerAPi.Services.UserService
{
    [ApiController]
    [Route("api/[controller]")]
    public class TwoFactorManager : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly ValidationCodeService _validationCodeService;
        private readonly ApplicationDbContext _context;

        public TwoFactorManager( ApplicationDbContext context ,IEmailSender emailSender, IMapper mapper, IUserService userService,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ValidationCodeService validationCodeService)
        {
            _userManager = userManager;
            _userService = userService;
            _mapper = mapper;
            _emailSender = emailSender;
            _validationCodeService = validationCodeService;
            _context = context;
        }

        public class Verify2FAModel
        {
            public string Email { get; set; }
            public string Code { get; set; }
        }

        public class Enable2FAModel
        {
            public string Email { get; set; }
        }

        public class Disable2FAModel
        {
            public string Email { get; set; }
        }

        [HttpPost("enable-2fa")]
        public async Task<IActionResult> Enable2FA([FromBody] Enable2FAModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound("Utilisateur introuvable.");
            }

            // Générer et stocker le code 2FA en BD
            var code = await _validationCodeService.GenerateAndSaveCodeAsync(user);

            var message = $"Votre code de vérification 2FA est : {code}"+"   il expirera dans 10min";

            // Envoyer le code par email
            var sendEmailResult = await _emailSender.SendEmailAsync(user.Email, "Code de vérification 2FA", message);

            if (!sendEmailResult)
            {
                return StatusCode(500, "Erreur lors de l'envoi du code de vérification.");
            }

            // Activer 2FA pour l'utilisateur
            await _userManager.SetTwoFactorEnabledAsync(user, true);

            return Ok(new { Message = "2FA activé et code envoyé par email." });
        }

        [HttpPost("verify-2fa")]
        public async Task<IActionResult> Verify2FA([FromBody] Verify2FAModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized("Utilisateur introuvable.");
            }

            // Vérifier si le code est valide en base de données
            var valid = await _validationCodeService.ValidateCodeAsync(user, model.Code);
            if (!valid)
            {
                return Unauthorized("Code 2FA invalide ou expiré.");
            }

            // Générer un token JWT
            var token = _userService.GenerateJwtToken(user);
            return Ok(new { token });
        }


        [HttpPost("disable-2fa")]
        public async Task<IActionResult> Disable2FA([FromBody] Disable2FAModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound("Utilisateur introuvable.");
            }

            // Désactiver le 2FA
            await _userManager.SetTwoFactorEnabledAsync(user, false);

            // Supprimer les codes 2FA associés à cet utilisateur
            var userCodes = _context.ValidationCodes.Where(v => v.UserId == user.Id);
            _context.ValidationCodes.RemoveRange(userCodes);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "2FA a été désactivé." });
        }

    }
}
