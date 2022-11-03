using Microsoft.AspNetCore.Mvc;
using SharedListApi.Services;

namespace SharedListApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterUserController : Controller
    {
        IUserRegistrationService _userRegistrationService;

        public RegisterUserController(IUserRegistrationService userRegistrationService)
        {
            _userRegistrationService = userRegistrationService;
        }

        public IActionResult Index()
        {
            var userCredentials = _userRegistrationService.RegisterUser();
            return Ok(userCredentials);
        }
    }
}
