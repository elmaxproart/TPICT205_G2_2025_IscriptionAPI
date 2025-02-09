using AutoMapper;
using gradeManagerServerAPi.Models.UserConnexion;
using gradeManagerServerAPi.Services.UserService.Notification;
using gradeManagerServerAPi.Services.UserService.StandardConnexion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace gradeManagerServerAPi.Controllers.UserC
{

    [Route("api/auth")]
        [ApiController]
        public class AuthController : ControllerBase
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly IConfiguration _configuration;
            private readonly IUserService _userService;
            private readonly IMapper _mapper;
            private readonly IEmailSender _emailSender;
            private readonly ValidationCodeService _validationCodeService;

        public AuthController( ValidationCodeService validationCodeService,IEmailSender emailSender, IMapper mapper,  IUserService userService,   UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
                _userManager = userManager;
                _signInManager = signInManager;
                _configuration = configuration;
                _userService = userService;
                _mapper = mapper;
                _emailSender = emailSender;
                _validationCodeService = validationCodeService;


        }
            /// <summary>
            /// enregistrement d'un utilisateur avec le role user
            /// </summary>
            /// <param name="model"></param>
            /// <returns></returns>
            [HttpPost("register")]
            public async Task<IActionResult> Register([FromBody] RegisterModel model)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FullName = model.FullName };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    return Ok(new { message = "Utilisateur créé avec succès." });
                }
                return BadRequest(result.Errors);
            }
        /// <summary>
        /// connexion de l'utilisateur + generation de JWT
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                // Vérification 2FA : Si l'utilisateur a 2FA activé, générer et enregistrer le code
                if (await _userManager.GetTwoFactorEnabledAsync(user))
                {
                    // Générer et enregistrer le code 2FA dans la BD via ValidationCodeService
                    var code = await _validationCodeService.GenerateAndSaveCodeAsync(user); // Appelle la méthode pour générer et sauvegarder le code 2FA

                    var message = $"Votre code 2FA est : {code}. Il expirera dans 10 minutes.";

                    // Envoyer le code par e-mail
                    var sendEmailResult = await _emailSender.SendEmailAsync(user.Email, "Code de vérification 2FA", message);

                    if (!sendEmailResult)
                    {
                        return StatusCode(500, "Erreur lors de l'envoi du code de vérification.\n Votre connexion peut être instable.");
                    }

                    // Demander à l'utilisateur de saisir le code 2FA
                    return Ok(new { message = "Veuillez vérifier votre e-mail pour le code 2FA." });
                }

                // Générer un JWT si la vérification 2FA n'est pas activée
                var userResult = _mapper.Map<ApplicationUser, object>(user);
                var token = _userService.GenerateJwtToken(user);
                return Ok(new { token });
            }

            return Unauthorized();
        }


        /// <summary>
        /// recuperer les userConnecter
        /// </summary>
        /// <returns></returns>
        [Authorize]
            [HttpGet("me")]
            public async Task<IActionResult> GetCurrentUser()
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return NotFound();
                return Ok(new { user.Email, user.FullName, Roles = await _userManager.GetRolesAsync(user) });
            }
        }

       
}

