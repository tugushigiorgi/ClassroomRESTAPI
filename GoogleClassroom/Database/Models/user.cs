using Microsoft.AspNetCore.Identity;
using System.Reflection;
using System.Text.Json.Serialization;

namespace GoogleClassroom.Database.Models
{
    public class user:IdentityUser<Guid>
    {
        
        public string? ProfilePhoto { get; set; }
      
         public string Surname { get; set; }
        
        [JsonIgnore]
        public List<Room> JoinedRooms { get; set; } = new();
        [JsonIgnore]
        public List<StudentAssigment> StudentAssigments { get; set; } = new();

        [JsonIgnore]
        public List<Room> CreatedRooms { get; set; } = new();
        [JsonIgnore]
        public List <Comment> comments=new();
        [JsonIgnore]
        public List<post> Posts { get; set; } = new();

        [JsonIgnore]

        public List<PostComment>  postComments = new();
        [JsonIgnore]
        public override string PasswordHash
        {
            get => base.PasswordHash;
            set => base.PasswordHash = value;
        }
    }
}
