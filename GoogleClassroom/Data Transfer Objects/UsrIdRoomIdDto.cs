using System.ComponentModel.DataAnnotations;

namespace GoogleClassroom.Data_Transfer_Objects
{
    public class UsrIdRoomIdDto
    {
        [Required]
        public Guid usrid { get; set; }
        [Required]
        public int roomid { get; set; }


    }
}
