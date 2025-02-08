using Microsoft.AspNetCore.Mvc;

namespace gradeManagerServerAPi.Controllers.StudentC
{
    [ApiController]
    [Route("[controller]")]
    public class EtudiantController : ControllerBase
    {

        private readonly ILogger<EtudiantController> _logger;

        public EtudiantController(ILogger<EtudiantController> logger)
        {
            _logger = logger;
        }


    }
}
