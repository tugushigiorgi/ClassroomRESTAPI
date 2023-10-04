using GoogleClassroom.Data_Transfer_Objects;
using GoogleClassroom.Database;
using GoogleClassroom.Database.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Hosting;

namespace GoogleClassroom.Repositories
{
    public class PostRepositoy
    {
      private   ClassroomDbContext classroomcontext;

        public PostRepositoy(ClassroomDbContext db) {

            classroomcontext = db;

        }
       
        public async Task<ControllerResponse>  Addpost(AddPostDto dto, Guid currentuserId) { 
                
           
            var currentuser=classroomcontext.user.SingleOrDefault( u=>u.Id==currentuserId);
           
            if(currentuser==null)
            {
                return new ControllerResponse { 
                    code=0,
                    message=$" user with id {currentuserId} not found "
                
                
                };



            }
            
             
            var currentroom = classroomcontext.room.Include(c=>c.JoinedUsers).SingleOrDefault(u => u.Id == dto.Roomid);
           
             if(currentroom==null)
            {

                return new ControllerResponse
                {
                    code = 0,
                    message = $"Room with id {dto.Roomid} Not Found"

                };



            }   
              
              var ifinroom = currentroom.JoinedUsers.SingleOrDefault(u => u.Id==currentuserId);

              
                if(ifinroom == null) {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"User :{currentuserId} is not Joined in Room To Post  | RoomId :{currentroom.Id}"

                };


            }

 

            post postv = new post {
                Content=dto.PostContent,
                author= currentuser,
                room=currentroom,
                PostDate=DateTime.Now,

            };
           

            try
            {
                classroomcontext.post.Add(postv);
                await classroomcontext.SaveChangesAsync();

                return new ControllerResponse
                {
                    code = 1,
                    message = $"Post added succesfully "
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



        public async Task<ControllerResponse> EditPost(EditPostDto dto, Guid currentuserId)
        {


            var currentuser = classroomcontext.user.SingleOrDefault(u => u.Id==currentuserId);

            if (currentuser == null)
            {
                return new ControllerResponse
                {
                    code = 0,
                    message = $" user with id {currentuserId} not found "


                };
            }



            var Currentpost = classroomcontext.post.SingleOrDefault(u => u.Id == dto.Id);

            if (Currentpost == null)
            {
                return new ControllerResponse
                {
                    code = 0,
                    message = $" Post with id {dto.Id} not found "


                };
            }


            if (currentuser.Id!=Currentpost.authorId  )
            {

                return new ControllerResponse { code = 0, message = "User does not have permission" };

            }

            if (Currentpost.Content != dto.Content)
            {

                Currentpost.Content = dto.Content;
            }
            try
            {
               
                await classroomcontext.SaveChangesAsync();

                return new ControllerResponse
                {
                    code = 1,
                    message = $"Post Edited Succesfully "
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





            public async Task<ControllerResponse> DeletePost(int postid, Guid currentusrId) {

        var currentuser = classroomcontext.user.SingleOrDefault(u => u.Id==currentusrId);
     
        if(currentuser == null)
            {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"user with id {currentusrId} Not Found"


                };


            }        
            
      var posttodelete=classroomcontext.post.SingleOrDefault(u=>u.Id==postid);
            if (posttodelete == null)
            {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"Post with id {postid} Not Found"


                };


            }
             
                if (posttodelete.authorId!=currentuser.Id)
                {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"user with id {currentusrId} Not allowed To delete Post"


                };

            }



                 

            try
            {
                classroomcontext.post.Remove(posttodelete);
                await classroomcontext.SaveChangesAsync();

                return new ControllerResponse
                {
                    code = 1,
                    message = $"Post Deleted Succesfully "
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






        public PostDto GetPostById(int id , Guid currentuserid) {
            //ID to check if user has permission
            var currentuser = classroomcontext.user.SingleOrDefault(u => u.Id==currentuserid);
            if(currentuser == null) { return null; }
            var post = classroomcontext.post.SingleOrDefault(u => u.Id == id);
            if (post == null) return null;
            var room=classroomcontext.room.SingleOrDefault(u=>u.Id==post.roomid);
            if(room == null) { return null; }


         var data=   classroomcontext.post.Include(o=>o.postComments).Where(pp=>pp.Id==id).Select(p=> new PostDto
            {
                Id = p.Id,
                Content = p.Content,
                author = p.author,
                postComments = p.postComments,
                PostDate = p.PostDate 




            }).SingleOrDefault();





            return data  ;









        }


      
        public async Task<ControllerResponse> AddComment(PostCommentDto dto, Guid currentuserid) { 
        
        var CurrentUser = classroomcontext.user.SingleOrDefault(u=>u.Id==currentuserid);

            if (CurrentUser == null)
            {
                return new ControllerResponse
                {
                    code = 0,
                    message = $"user with id {currentuserid} Not Found"

                            
                };


            }






            var CurrentPost= classroomcontext.post.SingleOrDefault(p=>p.Id==dto.postid);
             
            
            if(CurrentPost == null)
            {

                return new ControllerResponse { 
                    code=0,
                    message=$"Post not found with id {dto.postid}"
                
                
                };


            }

             
            var CurrentRoom=classroomcontext.room.Include(j=>j.JoinedUsers).SingleOrDefault(r=>r.Id==CurrentPost.roomid);

             
            var check = CurrentRoom.JoinedUsers.SingleOrDefault(u => u.Id==(currentuserid) );


            if (check == null) {

                return new ControllerResponse
                {
                    code = 0,
                    message = "User is not Joined in the room and dont   permission to add comment"

                };
            
            }








            var postcomment = new PostComment
            {
                Comment = dto.comment,
                PostDate = DateTime.Now,
                author = CurrentUser,
                post = CurrentPost
            };

             







            try
            {
                classroomcontext.postComments.Add(postcomment);
                await classroomcontext.SaveChangesAsync();

                return new ControllerResponse
                {
                    code = 1,
                    message = $"Comment added succefully "
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


      
        public async Task<ControllerResponse> DeleteComment(int commentid, Guid currentuserid)
        {
            var CurrentUser = classroomcontext.user.SingleOrDefault(u => u.Id== currentuserid);

            if (CurrentUser == null)
            {

                return new ControllerResponse
                {
                    code = 0,
                    message = $" User with id {currentuserid} Not found"




                };



            }

            var CurrentComment = classroomcontext.postComments.SingleOrDefault(p => p.Id == commentid);


            if (CurrentComment == null)
            {

                return new ControllerResponse
                {
                    code = 0,
                    message = $"Comment With id {commentid} not found "

                };


            }





            if (CurrentComment.authorid!= CurrentUser.Id)
            {

                return new ControllerResponse
                {
                    code = 0,
                    message = "User dont have permission to delete Comment"

                };

            }




            try
            {
                classroomcontext.postComments.Remove(CurrentComment);

                await classroomcontext.SaveChangesAsync();

                return new ControllerResponse
                {
                    code = 1,
                    message = $"Comment Delete Succesfully "
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
