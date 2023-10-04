using GoogleClassroom.Database.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GoogleClassroom.Data_Transfer_Objects
{
    public class PostDto
    {

        public int Id { get; set; }
     
        public string Content { get; set; }

       
        public user author { get; set; }
     

     
      


       

        public List<PostComment> postComments  { get; set; } =new ()  ;
   

        public DateTime PostDate { get; set; }













    }
}
