using System.ComponentModel.DataAnnotations;

namespace GoogleClassroom.Data_Transfer_Objects
{
    public class FileDto
    {
        [Required]
        public string FileName { get; set; }
        [Required]
        public string ContentType { get; set; }
        [Required]
        public string FilePath { get; set; } 


    }
}
