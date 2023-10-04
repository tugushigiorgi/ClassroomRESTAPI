using GoogleClassroom.Data_Transfer_Objects;
using GoogleClassroom.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GoogleClassroom.Controllers
{
    [ApiController]
    [Route("/studentassigment")]
    public class StudentAssigmentController :ControllerBase
    {

        StudentAssigmentRepository studentAssigmentRepository;

         public StudentAssigmentController(StudentAssigmentRepository repo) {
        
            studentAssigmentRepository = repo;
        
        }

        [HttpPost]
        public async Task<IActionResult> MakeAssigment([FromBody] AssignTaskDto dto) {


            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return BadRequest();

            if (Guid.TryParse(userIdClaim.Value, out Guid userId))
            {



                ControllerResponse result = await studentAssigmentRepository.AssignTask(dto, userId);
                if (result.code == 0) { return BadRequest(result.message); }

                return Ok(result.message);

            }
            return BadRequest();



        }

        

    }
}
