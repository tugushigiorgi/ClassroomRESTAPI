using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GoogleClassroom.Database.Models
{
    public class Comment
    {


           public int Id { get; set; }

        [Required]

        public string content { get; set; }

            public DateTime CommentDate { get; set; }

      
                

        public int AssigmentID { get; set; }
        [JsonIgnore]
        public Assigment assigment { get; set; }



           public user author { get; set; }
             public Guid authorid { get; set; }




    }
}
