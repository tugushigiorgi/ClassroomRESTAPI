using GoogleClassroom.Database.Models;
using System.ComponentModel.DataAnnotations;

namespace GoogleClassroom.NewFolder
{
    public class CreateRoomDto
    {
        [Required]
        public string ClassName { get; set; }
        [Required]
        public string Subject { get; set; }
        public string? BackgroundPicture { get; set; }
       



    }
}
