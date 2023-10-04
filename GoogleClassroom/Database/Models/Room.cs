using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GoogleClassroom.Database.Models
{
    public class Room
    {

        public int Id { get; set; }
        public string ClassCode { get; set; }

        public string ClassName { get; set; }


        public string Subject { get; set; }

        public string? BackgroundPicture { get; set; }

        [JsonIgnore]
        public user RoomOwner { get; set; }

        public Guid RoomOwnerID { get; set; }
        [JsonIgnore]
        public List<post> Posts { get; set; }=new();
        [JsonIgnore]

        public List<Assigment> Assigments { get; set; } = new();

        [JsonIgnore]
        public List<user> JoinedUsers { get; set; } = new();


        public Room(List<Room> rooms) {
            ClassCode = GenerateCode(rooms);





        }
        public Room() { 
        
        }


        public   string GenerateCode(List<Room> rooms)
        {
            Random RandomC = new Random();
            string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
             
        
            string code = "";

            for (int i = 0; i < 6; i++)
            {

                code += validChars[RandomC.Next(validChars.Length)];
            }

            


          
             
            if (rooms.SingleOrDefault(a=>a.ClassCode==code)==null)
                {
              
                return code;
                 
                }


            return GenerateCode(rooms);
            
                
             
            





            




        }










    }
}