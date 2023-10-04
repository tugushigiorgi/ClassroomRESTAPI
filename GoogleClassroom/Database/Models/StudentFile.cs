using System.Text.Json.Serialization;

namespace GoogleClassroom.Database.Models
{
    public class StudentFile
    {
        public int Id { get; set; }

        [JsonIgnore]
        public StudentAssigment Studentassigment { get; set; }
        public int StudentassigmentID { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;  
        public string FilePath { get; set; }=string.Empty;
    }
}
