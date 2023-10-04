using System.ComponentModel.DataAnnotations;

namespace GoogleClassroom.Data_Transfer_Objects
{
    public class PostCommentDto
    {
        [Required]
        public int postid { get; set; }
        [Required]
        public string comment { get; set; }




    }
}
