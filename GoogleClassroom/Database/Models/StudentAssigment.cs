using System.Text.Json.Serialization;

namespace GoogleClassroom.Database.Models
{
    public class StudentAssigment
    {

        public int Id { get; set; }

 
        public StudentFile File { get; set; }

        public DateTime  AssignedDate { get; set; }

        public double Grade { get; set; }


        [JsonIgnore]
        public Assigment assigment { get; set; }

        public int AssigmentId { get; set; }

        public user user { get; set; }
        public Guid UserId { get; set; }




       


    }
}
