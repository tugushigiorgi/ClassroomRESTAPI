using System.Text.Json.Serialization;

namespace GoogleClassroom.Database.Models
{
    public class PostComment
    {


        public int Id { get; set; }


        public string Comment { get; set; }

        public DateTime PostDate { get; set; }

        public int Postid { get; set; }

        [JsonIgnore]
        public post post { get; set; }


    public Guid authorid { get; set; }

       
    public user author { get; set; }



    }
}
