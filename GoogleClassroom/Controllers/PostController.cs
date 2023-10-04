using GoogleClassroom.Data_Transfer_Objects;
using GoogleClassroom.Database;
using GoogleClassroom.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GoogleClassroom.Controllers
{

    [ApiController]
    [Route("/post")]
    public class PostController :ControllerBase{


        ClassroomDbContext classroomcontext;
        PostRepositoy postrepositoy;
        public PostController(ClassroomDbContext db, PostRepositoy Postrepositoy) {
            
            classroomcontext = db;
            postrepositoy = Postrepositoy;
        }

      
        [HttpPost]
        public async Task<IActionResult>  AddPost([FromBody] AddPostDto addPostDto) {



            if (ModelState.IsValid)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null) return BadRequest();

                if (Guid.TryParse(userIdClaim.Value, out Guid userId))

                {
                    ControllerResponse result = await postrepositoy.Addpost(addPostDto, userId);

                    if (result.code == 0)
                    {
                        return BadRequest(result.message);
                    }

                    return Ok(result.message);
                }

                return BadRequest("Check Request Parameters");

            }

            return BadRequest();

        }


     
        [HttpPut]
        public async Task<IActionResult> EditPost([FromBody] EditPostDto dto ) { 
        
            if(!ModelState.IsValid) { return BadRequest("Check request parameters"); }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return BadRequest();

            if (Guid.TryParse(userIdClaim.Value, out Guid userId))
            { 


                ControllerResponse result =await postrepositoy.EditPost(dto, userId);

            if(result.code==0) { return BadRequest(result.message); }
            return Ok(result.message);
}

            return BadRequest();

        }




     
       
        [HttpDelete("{postid}")]
        public async Task<IActionResult> DeletePost(int postid) {


           

            if(postid==0) { return BadRequest("Check Request Parameters"); }


            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return BadRequest();

            if (Guid.TryParse(userIdClaim.Value, out Guid userId))
            {



                ControllerResponse result = await postrepositoy.DeletePost(postid, userId);

                if (result.code == 0)
                {
                    return BadRequest(result.message);
                }
                return Ok(result.message);
            }
            return BadRequest();



        }


        //auth
        [HttpGet("{id}")]
        
        public IActionResult GetPostByID(int id) {

             
            if (id == 0) return BadRequest("Check Request parameters");


            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return BadRequest();

            if (Guid.TryParse(userIdClaim.Value, out Guid userId))
            {


                var result = postrepositoy.GetPostById(id, userId);

                if (result == null) return NotFound();
                return Ok(result);
            }


            return BadRequest();
        }


      
        //X auth
        [HttpPost("addcomment")]
        public async Task<ActionResult> AddComment(PostCommentDto dto) {

         

            if(ModelState.IsValid) {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null) return BadRequest();

                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {

 
                 
                ControllerResponse result = await  postrepositoy.AddComment(dto, userId);
            if(result.code ==0) return BadRequest(result.message);

            return Ok(result.message);
            }
            return BadRequest("Check Request Parameters");
            }

            return BadRequest();

        }

        
       

        [HttpDelete("comment/{commentid}")]
        public async Task<IActionResult> DeleteComment(int commentid) {

            if (commentid==0) return BadRequest("Check request parameters");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return BadRequest();

            if (Guid.TryParse(userIdClaim.Value, out Guid userId))
            {


                ControllerResponse result = await postrepositoy.DeleteComment(commentid, userId);
                if (result.code == 0) return BadRequest(result.message);

                return Ok(result.message);
            }

            return BadRequest();
        }
       
        

    }
}
