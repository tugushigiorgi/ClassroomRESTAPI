using System.ComponentModel.DataAnnotations;

namespace GoogleClassroom.Data_Transfer_Objects
{
    public class AssigmentGradeDto
    {
        [Required]
        public int StudentAssigmentID { get; set; }
        [Required]
        public double Grade { get; set; }



    }
}
