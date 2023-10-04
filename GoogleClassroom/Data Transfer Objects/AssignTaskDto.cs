using System.ComponentModel.DataAnnotations;

namespace GoogleClassroom.Data_Transfer_Objects
{
    public class AssignTaskDto
    {

        [Required]
        public int assignmentId {  get; set; }
        [Required]
        public FileDto file { get; set; }




    }
}
