using GoogleClassroom.Data_Transfer_Objects;
using GoogleClassroom.Database;
using GoogleClassroom.Database.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GoogleClassroom.Repositories
{
    public class AssigmentRepository
    {
        private ClassroomDbContext dbContext;

        private UserManager<user> usrmanager;
     public AssigmentRepository(ClassroomDbContext db, UserManager<user>  manager) { 
        
            dbContext = db;
            usrmanager =  manager;
        }


        public async Task<ControllerResponse> SetGradeToAssigment(AssigmentGradeDto dto, Guid curretuserid) { 
        
                var currentuser=dbContext.user.SingleOrDefault(usr=>usr.Id==curretuserid );

          

            if (currentuser == null)
            {

                return new ControllerResponse { 
                    code=0,
                    message=$"User Not Found with id :{curretuserid}"
                
                };
            }
            var studentassigment = dbContext.studentAssigment.Include(c=>c.assigment).ThenInclude(d=>d.Room).SingleOrDefault(f => f.Id == dto.StudentAssigmentID);

            if (studentassigment == null)
            {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"Student Assigment Not Found with id :{dto.StudentAssigmentID}"

                };



            }


            if ( studentassigment.assigment.Room.RoomOwnerID!=curretuserid)
            {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"User Does not have permission to Assign Grade"

                };


            }




            studentassigment.Grade= dto.Grade;


         

            try {
                await dbContext.SaveChangesAsync();

                return new ControllerResponse
                {
                    code = 1,
                    message = "Grade Set Succesfuly "

                };

            } catch (DbUpdateException ex) {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"Error While Saving changes {ex}"

                };
            }




        }


      
        public async Task<ControllerResponse> AddAssigment(AddAssigmentDto dto, Guid currentuserid) {


            var CurrentUser = dbContext.user.SingleOrDefault(u => u.Id==currentuserid );
            
            if(CurrentUser == null)
            {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"User With id {currentuserid} Not Found"

                };


            }
            
            
            
            var currentroom = dbContext.room.SingleOrDefault(a => a.Id == dto.RoomID);

            if (currentroom == null) {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"Room not Found with id {dto.RoomID} "
};
 }


                
            
            
           
            
            
            
            
            
            if( CurrentUser.Id  !=currentroom.RoomOwnerID ) {

                return new ControllerResponse { 
                    code=0,
                    message ="User not allowed to add assgiment (not a owner)"
                
                
                };


            
            
            
            }

            var atachedfile = new AssigmentFile
            {
                FileName = dto.AttachedFile.FileName,
                ContentType = dto.AttachedFile.ContentType,
                FilePath = dto.AttachedFile.FilePath


            };

            var NewAssigment = new Assigment {
                Title = dto.Title,
                Instructions = dto.Instructions,
                Points = dto.Points,
                UploadeDate = DateTime.Now,
                DueDate = dto.DueDate,
                AttachedFile = atachedfile,
                Room = currentroom
 };





            try
            {
                dbContext.assigment.Add(NewAssigment);
                await dbContext.SaveChangesAsync();

                return new ControllerResponse
                {
                    code = 1,
                    message = $"New Assigment Add Succesfully "
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



       

        public async Task<ControllerResponse> DeleteAssigmentById(int assigmentId, Guid userid) { 
            

            var currentuser=dbContext.user.SingleOrDefault(a => a.Id== userid);
            if (currentuser == null) {
                return new ControllerResponse { 
                    code=0,
                    message= $"User not found with id {userid}"



                };
            
            
            }
            
             
            var asigment=dbContext.assigment.SingleOrDefault(a=>a.Id == assigmentId);

            if (asigment == null)
            {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"Assigment not found with id {assigmentId}"



                };


            }







            var room=dbContext.room.SingleOrDefault(b => b.Id==asigment.RoomID);
            if (room == null) {

                return new ControllerResponse { 
                    code=0,
                    message="Room not found "

                };

            
            }
            if (userid != room.RoomOwnerID) {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"User Does not have Permisson To Delete"


                        
                };
            }


 

            


            try
            {
                dbContext.assigment.Remove(asigment);
                await dbContext.SaveChangesAsync();

                return new ControllerResponse
                {
                    code = 1,
                    message = $" Assigment Removed Succesfully "
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

       
        public async Task<ControllerResponse> AddCommentToAssigment(AddCommentDto commentdto, Guid currentuserid) { 
        var CurrentUser=dbContext.user.SingleOrDefault(user => user.Id==currentuserid);
           if(CurrentUser== null)
            {
                return new ControllerResponse { 
                    code=0,
                    message=$"User with id {currentuserid} Not Found"
                
                };
            }
            
            var CurrentAssigment = dbContext.assigment.SingleOrDefault(i => i.Id == commentdto.Assigmentid);


            if (CurrentAssigment == null) {

                return new ControllerResponse { 
                    code=0,
                    message=$"Assigment not found With id {commentdto.Assigmentid} "
                
                
                
                };
            
            
            
            }



            var room = dbContext.room.Include(c=>c.JoinedUsers).SingleOrDefault(i => i.Id == CurrentAssigment!.RoomID);



            if (room == null)
            {

                return new ControllerResponse
                {
                    code = 0,
                    message = $"Room not found "



                };



            }




            var check=room.JoinedUsers.SingleOrDefault(a => a.Id== currentuserid);
            if(check==null) {


                return new ControllerResponse
                {
                    code = 0,
                    message = $"User does not have permission "



                };





            }

            var comment = new Comment {
                content = commentdto.Comment,
                assigment= CurrentAssigment,
                author= CurrentUser,
                CommentDate=DateTime.Now

            };



           


            try
            {
                dbContext.comment.Add(comment);
                await dbContext.SaveChangesAsync();

                return new ControllerResponse
                {
                    code = 1,
                    message = $"New Comment added succesfully "
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



        public   List<AssigmentDto> GetAssigmentById(int id, Guid userid) { 
        
        
      
 
 
            
          

          var listdata=  dbContext.assigment.Where(o=>o.Id==id). Select(data => new AssigmentDto {

                Title = data.Title,
                Instructions = data.Instructions,
                Points = data.Points,
                UploadeDate = data.UploadeDate,
                DueDate = data.DueDate,
                AttachedFile = data.AttachedFile,
                comments = data.comments,
                StudentFile = data.StudentAssigments.SingleOrDefault(i => i.UserId == userid).File,



            })  .ToList();


            return listdata;








        }



        public  DetailedAssigmentDto  CurrentUSerDetailedAssigment(int id, Guid currentuserid) {

        var CurrentUser = dbContext.user.SingleOrDefault(user => user.Id== currentuserid);
            if (CurrentUser == null) return null;
            var CurrentAssigment = dbContext.assigment.SingleOrDefault(assigment => assigment.Id == id);
            if (CurrentAssigment == null) return null;

            var Currentroom=dbContext.room.SingleOrDefault(room => room.Id == CurrentAssigment.RoomID);

            if (Currentroom == null) return null;

            if (CurrentUser.Id== Currentroom.RoomOwnerID) {

                var  Data = dbContext.assigment.
                    Include(c=>c.StudentAssigments)
                    .ThenInclude(d=>d.File).
                    
                    Where(i => i.Id == id).Select(
                   data =>
                   new DetailedAssigmentDto
                   {
                       Title = data.Title,
                       Instructions = data.Instructions,
                       Points = data.Points,
                       UploadeDate = data.UploadeDate,
                       DueDate = data.DueDate,
                       AttachedFile = new AssigmentFile {
                           
                           FileName=data.AttachedFile.FileName  , 
                           
                           
                           ContentType =data.AttachedFile.ContentType,
                           
                           
                           
                           FilePath=data.AttachedFile.FilePath },
                       comments = data.comments,


                      
                       AssignedStudentsinfo = data.StudentAssigments
                   }

                    ).SingleOrDefault();


                return Data;




            }




            return null;

        }



        public async Task<ControllerResponse> EditAssigment(EditAssigmentDto dto, Guid currentuserid) {
            var CurrentUser = dbContext.user.SingleOrDefault(user => user.Id== currentuserid) ;
            if(CurrentUser==null)
            {
                return new ControllerResponse { 
                    code=0,
                    message=$"User With Id {currentuserid} not found "
                     
                };

            }
            var CurrentAssigment = dbContext.assigment.Include(c=>c.AttachedFile).SingleOrDefault(i => i.Id == dto.Id);

            if(CurrentAssigment==null)
            {
                return new ControllerResponse {
                    code=0,
                    message=$"Assigment not Found with id {dto.Id}"
                
                };


            }


            if (dto.Title!=null && CurrentAssigment.Title != dto.Title) {
                CurrentAssigment.Title=dto.Title;
            }
            if (dto.Instructions!=null && CurrentAssigment.Instructions != dto.Instructions) {

                CurrentAssigment.Instructions = dto.Instructions;
            }
            if (dto.Points!=0 && CurrentAssigment.Points != dto.Points) {
                CurrentAssigment.Points=dto.Points;
            }
            if ( CurrentAssigment.DueDate != dto.DueDate) { 
                CurrentAssigment.DueDate=dto.DueDate;
            }
            if(CurrentAssigment.AttachedFile.FileName != dto.AttachedFile.FileName)
            {
                CurrentAssigment.AttachedFile.FileName  = dto.AttachedFile.FileName;

            }
            if (CurrentAssigment.AttachedFile.FilePath != dto.AttachedFile.FilePath) {
                CurrentAssigment.AttachedFile.FilePath = dto.AttachedFile.FilePath;



            }
            if (CurrentAssigment.AttachedFile.ContentType != dto.AttachedFile.ContentType) {

                CurrentAssigment.AttachedFile.ContentType = dto.AttachedFile.ContentType;


            }






            try
            {
                
                await dbContext.SaveChangesAsync();

                return new ControllerResponse
                {
                    code = 1,
                    message = $"Assigment Changed Succesfully "
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



        public async Task<ControllerResponse> DeleteCommentFromAssigment(int id, Guid userid) {

            var currentuser = dbContext.user.SingleOrDefault(usr => usr.Id==userid);

            if (currentuser == null) {
                return new ControllerResponse { code = 0, message = $"User Not Found wit id :{userid}" };
 
            }

            var CurrentComment = dbContext.comment.SingleOrDefault(i => i.Id == id);

            if(CurrentComment == null)
            {

                return new ControllerResponse() { code = 0, message = $"Comment Not Found With id {id}" };

            }
            if(CurrentComment.authorid != userid) {


                return new ControllerResponse() { code = 0, message = $"Current user is not Owner of the Comment To Delete" };


            }

           



            try
            {

                dbContext.comment.Remove(CurrentComment);
             await    dbContext.SaveChangesAsync();




                return new ControllerResponse
                {
                    code = 1,
                    message = $"Comment Removed Succesfully "
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
