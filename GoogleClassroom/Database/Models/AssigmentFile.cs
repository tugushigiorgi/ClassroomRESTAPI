using System.Text.Json.Serialization;

namespace GoogleClassroom.Database.Models
{
    public class AssigmentFile
    {
        public int Id { get; set; }


        [JsonIgnore]
        public Assigment assigment { get; set; }

        public int AssigmentID { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
    }
}
