namespace gradeManagerServerAPi.Controllers.UserC
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using global::gradeManagerServerAPi.Data.UserManagementAPI.Data;
    using global::gradeManagerServerAPi.Models.UserConnexion;

    namespace gradeManagerServerAPi.Controllers
    {
        [Authorize]
        [ApiController]
        [Route("api/userinfo")]
        public class UserInfoController : ControllerBase
        {
            private readonly ApplicationDbContext _context;
            private readonly UserManager<ApplicationUser> _userManager;

            public UserInfoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
            {
                _context = context;
                _userManager = userManager;
            }

            [HttpPost("update")]
            public async Task<IActionResult> UpdateUserInfo([FromBody] UserInfo model)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Unauthorized("Utilisateur non trouvé");

                var userInfo = _context.UserInfos.FirstOrDefault(ui => ui.UserId == user.Id);
                if (userInfo == null)
                {
                    userInfo = new UserInfo
                    {
                        UserId = user.Id,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                        DateOfBirth = model.DateOfBirth
                    };
                    _context.UserInfos.Add(userInfo);
                }
                else
                {
                    userInfo.Address = model.Address;
                    userInfo.PhoneNumber = model.PhoneNumber;
                    userInfo.DateOfBirth = model.DateOfBirth;
                    _context.UserInfos.Update(userInfo);
                }

                await _context.SaveChangesAsync();
                return Ok("Informations utilisateur mises à jour avec succès");
            }

            [HttpGet("me")]
            public async Task<IActionResult> GetUserInfo()
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Unauthorized("Utilisateur non trouvé");

                var userInfo = _context.UserInfos.FirstOrDefault(ui => ui.UserId == user.Id);
                return Ok(userInfo ?? new UserInfo());
            }



          
    }
    }

}
