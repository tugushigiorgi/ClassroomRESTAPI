using GoogleClassroom.Database.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GoogleClassroom.Data_Transfer_Objects
{
    public class AssigmentDto
    {

        public string Title { get; set; }

        public string? Instructions { get; set; }
      
        public double Points { get; set; }

        public DateTime UploadeDate { get; set; }

        public DateTime DueDate { get; set; }

        public AssigmentFile AttachedFile { set; get; }

        public List<Comment> comments { get; set; }

        [AllowNull]
        public StudentFile?  StudentFile { set; get; }








    }
}
