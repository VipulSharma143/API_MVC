using API.Models;
using API.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IUserRepo _userRepo;
        public AccountController(IUserRepo userRepo)
        {
            _userRepo = userRepo;   
        }
        [HttpPost("register")]
       public IActionResult Register([FromBody]User user)
        {
            if(ModelState.IsValid)
            {
                var IsUnique = _userRepo.IsUniqueUser(user.Username);
                if (!IsUnique) return BadRequest("User In Use!!");
                var UserInfo=_userRepo.Register(user.Username,user.Password);
                user = UserInfo;
                if (UserInfo == null) return BadRequest();
            }
            return Ok(user);
        }
        [HttpPost("Authenticate")]
        public IActionResult Authenticate(UserVM userVM) 
        {
          var user=_userRepo.Authenticate(userVM.Username,userVM.Password);
            if(user==null) return BadRequest("Wrong Username/Password");
            return Ok(user);
        }
    }
}
