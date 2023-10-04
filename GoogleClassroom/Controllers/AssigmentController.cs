using GoogleClassroom.Data_Transfer_Objects;
using GoogleClassroom.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GoogleClassroom.Controllers
{
    [ApiController]
    [Route("assigment")]
    public class AssigmentController:ControllerBase
    {
        private AssigmentRepository assigmentRepository;
      public AssigmentController(AssigmentRepository AssigmentRepository) {
            assigmentRepository = AssigmentRepository;
        
        }



 
        [HttpPost("grade")]
        public async Task<IActionResult> SetGradeToAssigment([FromBody] AssigmentGradeDto dto) {

            if(!ModelState.IsValid) { return BadRequest("Check request Parameters"); }


            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return BadRequest();

            if (Guid.TryParse(userIdClaim.Value, out Guid userId))

            {



                ControllerResponse result = await assigmentRepository.SetGradeToAssigment(dto, userId);

                if (result.code == 0) { return BadRequest(result.message); }

                return Ok(result.message);


            }
            return BadRequest( );
        }

 
       
        [HttpPost]
        public async Task<IActionResult> AddAssigment([FromBody] AddAssigmentDto addAssigmentDto) {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return BadRequest();

            if (Guid.TryParse(userIdClaim.Value, out Guid userId))

            {



                if (ModelState.IsValid)
                {



                    ControllerResponse result = await assigmentRepository.AddAssigment(addAssigmentDto, userId);

                    if (result.code == 0) return BadRequest(result.message);

                    return Ok(result.message);
                }
                return BadRequest("Check Request Parameters");
            }
            return BadRequest();


        }
 
    

        [HttpDelete("{assigmentid}")]

        public async Task<IActionResult> DeleteAssigment(int assigmentid) {
 

            if (assigmentid == 0) { return BadRequest("Check request Parameters"); }
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return BadRequest();

            if (Guid.TryParse(userIdClaim.Value, out Guid userId))

            {
                ControllerResponse result = await assigmentRepository.DeleteAssigmentById(assigmentid, userId);

                if (result.code == 0) return BadRequest(result.message);
                return Ok(result.message);
            }


            return BadRequest();
        }




        [HttpPut]
        public async Task<ActionResult> EditAssigment([FromBody] EditAssigmentDto dto)
        {

            if (!ModelState.IsValid) { return BadRequest(); }
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return BadRequest();

            if (Guid.TryParse(userIdClaim.Value, out Guid userId))
            { 

                var result = await assigmentRepository.EditAssigment(dto, userId);

            if (result.code == 0) return BadRequest(result.message);

            return Ok(result.message);

        }

            return BadRequest();





        }




        

        [HttpPost("comment")]
        public async Task<ActionResult> AddCommentToAssigment([FromBody] AddCommentDto commentdto)
        {
             
            if(!ModelState.IsValid) { return BadRequest("Check Request Parameters"); }


            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return BadRequest();

            if (Guid.TryParse(userIdClaim.Value, out Guid userId))
            {

                ControllerResponse result = await assigmentRepository.AddCommentToAssigment(commentdto, userId);

                if (result.code == 0) return BadRequest(result.message);
                return Ok(result.message);
            }


            return BadRequest();
        }


       
        [HttpDelete("comment/{id}")]
        public async Task<IActionResult> DeleteCommentFromAssigment(int id) {


            if(id==0) return BadRequest("Check Request Parameters");


            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return BadRequest();

            if (Guid.TryParse(userIdClaim.Value, out Guid userId))
            {

 




                ControllerResponse result = await assigmentRepository.DeleteCommentFromAssigment(id, userId);

                if (result.code == 0) { return BadRequest(result.message); }
                return Ok(result.message);
            }

            return BadRequest();



        }




       
        [HttpGet("detailed/{id}")]
        public IActionResult  DetailedAssigment(int id) {


            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return BadRequest();

            if (Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                var result = assigmentRepository.CurrentUSerDetailedAssigment(id, userId);

                if (result  == null) return NotFound("user does not have rooms");

                return Ok(result);
            }
            return BadRequest();
        
        
        }





        


        //gets data for user  to see assigment, see comment, see   file , and see own file
        [HttpGet("{id}")]
        public List<AssigmentDto> GetAssgimentById(int id ) {


            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return null;

            if (Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return assigmentRepository.GetAssigmentById(id, userId);
            }
            return null;



        }
        

  


    }
}
