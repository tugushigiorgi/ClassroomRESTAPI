using GoogleClassroom.Database.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GoogleClassroom.Data_Transfer_Objects
{
    public class EditAssigmentDto
    {
        [Required]
        public int Id { get; set; }
      
        public string Title { get; set; }
     
        public string  Instructions { get; set; }
     
        public double Points { get; set; }
     
        public DateTime DueDate { get; set; }
       
        public FileDto AttachedFile { set; get; }

        

       







    }
}
