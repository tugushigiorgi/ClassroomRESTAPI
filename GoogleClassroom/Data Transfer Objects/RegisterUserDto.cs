using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GoogleClassroom.Data_Transfer_Objects
{
    public class RegisterUserDto
    {


        public string? ProfilePhoto { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }





    }
}
