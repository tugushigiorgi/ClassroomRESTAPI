using GoogleClassroom.Data_Transfer_Objects;
using GoogleClassroom.Database;
using GoogleClassroom.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoogleClassroom.Controllers
{

    [Route("/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserRepository repo;
        private ClassroomDbContext dbContext;


        public UserController(UserRepository repository, ClassroomDbContext db) {

            repo = repository;
            dbContext = db;



        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto dto) {
            
            var result=await repo.CreateAccount(dto);
            if(result.code==0) { return BadRequest(result.message); }

            return Ok(result.message);
 
        }





        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto) { 
        
        var rst=await repo.Login(dto);
    
        if(rst==null) { return BadRequest("error"); }

        return Ok(rst);
        
        
        
        }



    }
}
