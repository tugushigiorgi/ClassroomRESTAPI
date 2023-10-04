using System.ComponentModel.DataAnnotations;

namespace GoogleClassroom.Data_Transfer_Objects
{
    public class EditPostDto
    {

        [Required]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }

    }
}
