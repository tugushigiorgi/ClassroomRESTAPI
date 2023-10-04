using System.ComponentModel.DataAnnotations;

namespace GoogleClassroom.Data_Transfer_Objects
{
    public class EditRoomDto
    {


        public int RoomId { get; set; }
        public string? className { get; set; }

        public string? subject { get; set; }


        public string? backgroundpicture { get; set; }







    }
}
