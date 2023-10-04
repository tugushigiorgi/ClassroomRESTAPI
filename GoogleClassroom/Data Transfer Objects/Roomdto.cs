using GoogleClassroom.Database.Models;

namespace GoogleClassroom.Data_Transfer_Objects
{
    public class Roomdto
    {
        public string classCode { get; set; }




        public string className { get; set; }

        public string subject { get; set; }


        public string  backgroundpicture { get; set; }
        public user roomowner { get ; set; }

          public List<post> posts { get; set; }
           public List<Assigment> assigments { get ; set; }   
                
            public List<user> JoinedUsers { get; set; }

    }
}
