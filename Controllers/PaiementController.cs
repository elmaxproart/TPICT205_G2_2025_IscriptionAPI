using gradeManagerServerAPi.Data.UserManagementAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace gradeManagerServerAPi.Controllers
{
    public class PaiementController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaiementController(ApplicationDbContext context)
        {
            _context = context;
        }

    }
}
