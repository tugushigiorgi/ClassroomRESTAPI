using GoogleClassroom.Data_Transfer_Objects;
using GoogleClassroom.Database;
using GoogleClassroom.Database.Models;
using GoogleClassroom.NewFolder;
using GoogleClassroom.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Collections.Generic;
 
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace GoogleClassroom.Controllers
{

    [ApiController]

    [Route("/room")]
    public class RoomController : ControllerBase
    {
        private ClassroomDbContext classroomdb;
        private RoomRepositoy roomrepository;

        public RoomController(ClassroomDbContext dbcontext, RoomRepositoy Roomrepository) {
            classroomdb = dbcontext;
            roomrepository = Roomrepository;


        }

       



        [HttpGet("userrooms")]
        public IEnumerable<Room> UserCreatedRoomsList()
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return null;

            if (Guid.TryParse(userIdClaim.Value, out Guid userId))

            {
                return roomrepository.UserCreatedRoomsList(userId);

            }

            return null;
        }






        [HttpGet("userjoinedrooms/{id}")]
        public List<List<Room>> userjoinedrooms(Guid id)
        {
            return roomrepository.GetUserJoinedRooms(Guid.Empty);
        }



        [HttpPost("createroom")]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDto RoomDto) {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return null;

            if (Guid.TryParse(userIdClaim.Value, out Guid userId))

            {
                if (ModelState.IsValid)
                {

                    ControllerResponse result = await roomrepository.CreateRoom(RoomDto, userId);
                    if (result.code == 0) return BadRequest(result.message);

                    return Ok(result.message);



                }
            }

            return BadRequest("check request parameters");
        }



       
        [HttpPost("joinclass/{classCode}")]
        public async Task<IActionResult> JoinUserToRoom(string classCode) {

            if (classCode != null)
            {

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null) return null;

                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    ControllerResponse result = await roomrepository.JoinUserToRoom(userId, classCode);


                    if (result.code == 0)
                    {
                        return BadRequest(result.message);

                    }

                    return Ok(result.message);
                }
            }

            return BadRequest("Check request parameters");
        }





        [HttpGet("roombyid/{id}")]
        public List<Roomdto> getRoomByID(int id)
        {

            if (id != 0) {
                return roomrepository.GetRoomById(id);
            }
            return null;





        }







        [HttpDelete("user")]
        public async Task<IActionResult> RemoveUserFromRoom([FromBody] UsrIdRoomIdDto dto)
        {

            if (ModelState.IsValid)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null) return null;

                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    ControllerResponse result = await roomrepository.RemoveUserFromRoom(dto.roomid,userId);

                    if (result.code == 0)
                    {

                        return BadRequest(result.message);
                    }
                    return Ok(result.message);
                }
                 


            } 

            return BadRequest("Check request Parameters");

        }

        

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserOwnRoomById(int id ) {




            if (id != 0)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null) return null;

                if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    ControllerResponse result = await roomrepository.DeleteUserOwnRoomById(userId, id);
                    if (result.code == 0)
                    {

                        return BadRequest(result.message);
                    }



                    return Ok(result.message);

                }
            }
            return BadRequest("Check request parameters");




        }





        

        [HttpPut]
        public async Task<ActionResult> EditRoom([FromBody] EditRoomDto dto) {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return null;

            if (Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                ControllerResponse result = await roomrepository.EditRoom(dto, userId);
                if (result.code == 0) { return BadRequest(result.message); }
                return Ok(result.message);

            }


            return BadRequest("error");





        }




    }
}
