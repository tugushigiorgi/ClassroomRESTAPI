using GoogleClassroom.Data_Transfer_Objects;
using GoogleClassroom.Database;
using GoogleClassroom.Database.Models;
using GoogleClassroom.NewFolder;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace GoogleClassroom.Repositories
{
    public class RoomRepositoy
    {

        ClassroomDbContext classroomdb;

        public RoomRepositoy(ClassroomDbContext db)
        {
            classroomdb = db;

        }


        public IEnumerable<Room> UserCreatedRoomsList(Guid id)
        {


            var user = classroomdb.user.Include(c => c.CreatedRooms).SingleOrDefault(u => u.Id == id);

            if (user != null)
            {
                return user.CreatedRooms;
            }

            return null;




        }


        public List<List<Room>> GetUserJoinedRooms(Guid id)
        {


            return classroomdb
                .user
                .Where(z => z.Id==id)
                .Select(c => c.JoinedRooms)
                .ToList();




        }

        public List<Roomdto> GetRoomById(int roomid)
        {


            var roomdto = classroomdb.room.Where(r => r.Id == roomid).Select(rm => new Roomdto
            {
                classCode = rm.ClassCode,
                className = rm.ClassName,

                subject = rm.Subject,
                backgroundpicture = rm.BackgroundPicture,
                roomowner = rm.RoomOwner,
                posts = rm.Posts,
                assigments = rm.Assigments,
                JoinedUsers = rm.JoinedUsers


            }).ToList();


            if (roomdto != null) return roomdto;

            return null;









        }


        
        public async Task<ControllerResponse> RemoveUserFromRoom(int roomid, Guid userid)
        {

            var room = classroomdb.room
                .Include(r => r.JoinedUsers)
                .SingleOrDefault(r => r.Id == roomid);

            if (room == null) return new ControllerResponse
            {
                code = 0,
                message = $"Room with id {roomid} Not Found "
            };
            var userToRemove = room.JoinedUsers.SingleOrDefault(u => u.Id==(userid));

            if (userToRemove == null) return new ControllerResponse
            {
                code = 0,
                message = $"User  with id {userid} Not Found "
            };

            if (userToRemove.Id!=( room.RoomOwnerID))
            {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"User  with id :{userid} is Owner of Group and Can not be Removed  "
                };



            }

            room.JoinedUsers.Remove(userToRemove);


            try
            {
                room.JoinedUsers.Remove(userToRemove);
                await classroomdb.SaveChangesAsync();
                return new ControllerResponse
                {
                    code = 1,
                    message = $"User  with id :{userid}  Succesfully Removed from Room With  id : {roomid} "
                };

            }
            catch (DbUpdateException ex)
            {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"Error While Saving Changes: {ex} "
                };


            }


















        }


        
        public async Task<ControllerResponse> JoinUserToRoom(Guid userid, string classCode)
        {

            var room = classroomdb.room.Include(p => p.JoinedUsers).SingleOrDefault(r => r.ClassCode == classCode);


            if (room == null)
            {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"Room  Not Found  with code :  {classCode}"
                };
            }

            user usr = classroomdb.user.SingleOrDefault(u => u.Id ==userid  );

            if (usr == null) 
            {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"User with id {userid} Not Found "
                };
            }


            var checkifjoined = room.JoinedUsers.SingleOrDefault(u => u.Id== ( userid));
            if (checkifjoined != null)
            {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"User with id {userid} Already Joined in Room With id :{room.Id} "
                };
            }





            try
            {
                room.JoinedUsers.Add(usr);
                await classroomdb.SaveChangesAsync();

                return new ControllerResponse
                {
                    code = 1,
                    message = $"User  with id :{userid}  Succesfully Joined in Room With code  : {classCode} "
                };

            }
            catch (DbUpdateException ex)
            {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"Error While Saving Changes: {ex} "
                };


            }








        }

        
        public async Task<ControllerResponse> CreateRoom(CreateRoomDto RoomDto, Guid currentuserid)
        {

            var currentuser = classroomdb.user.SingleOrDefault(x => x.Id==currentuserid);
            if (currentuser == null)
            {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"User With id {currentuserid} not found"


                };
            }


            Room room = new Room(classroomdb.room.ToList())
            {
                ClassName = RoomDto.ClassName,

                Subject = RoomDto.Subject,
                RoomOwner = currentuser,
                BackgroundPicture = RoomDto.BackgroundPicture,
                JoinedUsers = new List<user> { currentuser }
            };

            




            try
            {
                classroomdb.room.Add(room);

                await classroomdb.SaveChangesAsync();
                return new ControllerResponse
                {
                    code = 1,
                    message = $"Room Created Succesfuly "
                };

            }
            catch (DbUpdateException ex)
            {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"Error While Saving Changes: {ex} "
                };


            }




        }



         
        public async Task<ControllerResponse> DeleteUserOwnRoomById(Guid userid, int roomid)
        {


            var currentuser = classroomdb.user.SingleOrDefault(r => r.Id ==userid);

            if (currentuser == null) {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"User not found with id {userid}"


                };
 
            }
             
            var room = classroomdb.room.SingleOrDefault(r => r.Id == roomid);

            if (room == null)
            {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"Room  not found with id {roomid}"


                };

            }
             
                if (room.RoomOwnerID!= currentuser.Id)
                {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"User with id :{userid} is not Owner of the group with id {roomid}  and Dont have Permission "


                };

            }



             



            try
            {
                classroomdb.room.Remove(room);
                await classroomdb.SaveChangesAsync();

                return new ControllerResponse
                {
                    code = 1,
                    message = $"Room Deleted Succesfully id:{roomid}"
                };

            }
            catch (DbUpdateException ex)
            {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"Error While Saving Changes: {ex} "
                };


            }





             
        }


                
        public async Task<ControllerResponse> EditRoom(EditRoomDto dto, Guid currentuserid)
        {

        var Currentuser=classroomdb.user.SingleOrDefault(u=>u.Id==( currentuserid));
        if(Currentuser == null)
            {

                return new ControllerResponse
                {
                    code = 0,
                    message = $"User  Not Found  with ID :  {currentuserid}"
                };

            }

        var CurrentRoom=classroomdb.room.Include(c=>c.JoinedUsers).SingleOrDefault(r=>r.Id == dto.RoomId);
            if (CurrentRoom == null) {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"Room  Not Found  with ID :  {dto.RoomId}"
                };


            }


            var check = CurrentRoom.JoinedUsers.SingleOrDefault(i=>i.Id==(currentuserid));
            
            if(check == null)
            {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"User Does not Have Permisson to Edit Room "
                };


            }
            if (dto.subject != null && CurrentRoom.Subject != dto.subject) {

                CurrentRoom.Subject = dto.subject;


            }
            if(dto.className!=null && CurrentRoom.ClassName != dto.className)
            {
                CurrentRoom.ClassName = dto.className;


            }
            if (dto.backgroundpicture != null && CurrentRoom.BackgroundPicture != dto.backgroundpicture) {

                CurrentRoom.BackgroundPicture = dto.backgroundpicture;



            }




          


            try
            {
                await classroomdb.SaveChangesAsync();

                return new ControllerResponse
                {
                    code = 1,
                    message = $"Room Updated Succesfully "
                };

            }
            catch (DbUpdateException ex)
            {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"Error While Saving Changes: {ex} "
                };


            }
        }

        

        

    }
}
