using GoogleClassroom.Data_Transfer_Objects;
using GoogleClassroom.Database;
using GoogleClassroom.Database.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace GoogleClassroom.Repositories
{
    public class StudentAssigmentRepository
    {

        private ClassroomDbContext classroomdb;

            public StudentAssigmentRepository(ClassroomDbContext db) {

            classroomdb=db;


        }


        public async Task<ControllerResponse> AssignTask(AssignTaskDto dto, Guid currentuserId) {
            
            

            var Currentuser=classroomdb.user.SingleOrDefault(x => x.Id==currentuserId);
        
        
            if(Currentuser == null)
            {

                return new ControllerResponse
                {
                    code = 0,
                    message = $"User not found with id :{currentuserId}"


                };

            }

           


        
            var  assigment= classroomdb.assigment.Include(c=>c.StudentAssigments). SingleOrDefault(i=>i.Id == dto.assignmentId);
            
            if(assigment == null)
            {
                return new ControllerResponse
                {
                    code = 0,
                    message = $" Assigment  not found with id :{dto.assignmentId}"


                };



            }

            var ifassignedAlready = assigment.StudentAssigments.SingleOrDefault(c => c.UserId == currentuserId);

            if (ifassignedAlready != null) {

                return new ControllerResponse
                {
                    code = 0,
                    message = $" User Already Assigned file"


                };

            }




            var room = classroomdb.room.Include(c=>c.JoinedUsers). SingleOrDefault(room => room.Id == assigment.RoomID);


            var roomcheck = room.JoinedUsers.SingleOrDefault(i => i.Id==(currentuserId));
            if (roomcheck == null)
            {
                return new ControllerResponse
                {
                    code = 0,
                    message = $" User is not room to access Room Assigments"


                };



            }

            



            if (DateTime.Now > assigment.DueDate)
            {


                return new ControllerResponse
                {
                    code = 0,
                    message = $"Deadline, You can not upload Anymore"
                };


            }





            var studentassigment = new StudentAssigment {
                user = Currentuser,
                assigment= assigment,
                AssignedDate=DateTime.Now,
                 
            };

            var studentfile = new StudentFile
            {
                Studentassigment = studentassigment,
                FileName = dto.file.FileName,
                ContentType = dto.file.ContentType,
                FilePath = dto.file.FilePath,

            };
           
          
            



            try
            {
                classroomdb.studentAssigment.Add(studentassigment);
                classroomdb.studentFiles.Add(studentfile);
                await classroomdb.SaveChangesAsync();


                return new ControllerResponse
                {
                    code = 1,
                    message = $"Assigment Uploaded   Succesfully "
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
