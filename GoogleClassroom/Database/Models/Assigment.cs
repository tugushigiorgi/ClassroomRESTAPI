using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

 
namespace GoogleClassroom.Database.Models
{
    public class Assigment
    {


        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Instructions { get; set; }
        [Required]
        public double Points { get; set; }

        public DateTime UploadeDate { get; set; }

        public DateTime DueDate { get; set; }
 
        public  AssigmentFile AttachedFile { set; get; }



        public Room Room { get; set; }

        public int RoomID { get; set; }



        [JsonIgnore]
        public List<StudentAssigment> StudentAssigments { get; set; } = new();
        [JsonIgnore]
        public List<Comment> comments { get; set; } = new() ;



     




    }
}
