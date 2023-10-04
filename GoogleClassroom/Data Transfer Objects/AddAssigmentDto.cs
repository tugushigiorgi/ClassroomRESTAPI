using GoogleClassroom.Database.Models;
using System.ComponentModel.DataAnnotations;

namespace GoogleClassroom.Data_Transfer_Objects
{
    public class AddAssigmentDto
    {
        


        [Required]
        public string Title { get; set; }

        public string  Instructions { get; set; }
        [Required]
        public double Points { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public AssigmentFileDto AttachedFile { set; get; }
        [Required]
        public int RoomID { get; set; }









    }
}
