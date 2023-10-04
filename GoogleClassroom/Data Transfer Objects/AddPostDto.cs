using System.ComponentModel.DataAnnotations;

namespace GoogleClassroom.Data_Transfer_Objects
{
    public class AddPostDto
    {
        [Required]
        public string PostContent { get; set; }
        [Required]
        public int Roomid { get; set; }





    }
}
