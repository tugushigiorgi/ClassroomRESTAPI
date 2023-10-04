using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GoogleClassroom.Database.Models
{
    public class post
    {


        public int Id { get; set; }
        [Required]
        public string Content { get; set; }

        [JsonIgnore]
        public user author { get; set; }
        public Guid authorId { get; set; }

        [JsonIgnore]
        public Room room { get; set; }
        public int roomid { get; set; }


        [JsonIgnore]

        public List<PostComment> postComments { get; set; } = new();

        public DateTime PostDate { get; set; }

      

    }
}
