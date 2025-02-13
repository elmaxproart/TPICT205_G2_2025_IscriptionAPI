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
        /// Enregistrement d'un utilisateur avec le rôle "User"
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
           
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                NickName = model.FullName,
                FirstName = model.FirstName,  
                LastName = model.LastName,    
                Title = model.Title,          
                Enable = true,                
                createAt = DateTime.Now,      
                UpdateAt = DateTime.Now       
            };

            
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
                // Si 2FA est activé
                if (await _userManager.GetTwoFactorEnabledAsync(user))
                {
                   
                    // Générer et envoyer le code 2FA
                    var code = await _validationCodeService.GenerateAndSaveCodeAsync(user);
                    var message = $"Votre code 2FA est : {code}. Il expirera dans 10 minutes.";

                    var sendEmailResult = await _emailSender.SendEmailAsync(user.Email, "Code de vérification 2FA", message);

                    if (!sendEmailResult)
                    {
                        return StatusCode(500, "Erreur lors de l'envoi du code de vérification.\nVotre connexion peut être instable.");
                    }

                    return Ok(new { message = "Veuillez vérifier votre e-mail pour le code 2FA." });
                }

                // Si 2FA n'est pas activé, générer un nouveau token 
            
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
                return Ok(new { user.Email, user.NickName, Roles = await _userManager.GetRolesAsync(user) });
            }
        }

       
}

