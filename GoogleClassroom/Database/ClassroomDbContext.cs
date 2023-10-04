using GoogleClassroom.Database.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GoogleClassroom.Database
{
    public class ClassroomDbContext : IdentityDbContext<user,userrole,Guid>
    {
        public DbSet<user> user { get; set; } 

        public DbSet<Room> room { get; set; }

        public DbSet<StudentAssigment> studentAssigment { get; set; }

        public DbSet<Assigment> assigment { get; set; }

        public DbSet<Comment> comment { get; set; }

        public DbSet<post> post { get; set; }

        public DbSet<AssigmentFile> assigmentfile { get; set; }
        public DbSet<StudentFile> studentFiles { get; set; }

        public DbSet<PostComment> postComments { get; set; }
        public ClassroomDbContext (DbContextOptions db) : base(db) {

 
        }
        public ClassroomDbContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<post>()
             .HasOne(u => u.author)
            .WithMany(p => p.Posts)
          .HasForeignKey(i => i.authorId)
           .OnDelete(DeleteBehavior.NoAction);

         
 

   modelBuilder.Entity<user>().HasMany(c => c.comments).WithOne(c => c.author)
          .HasForeignKey(f => f.authorid);

             

            modelBuilder.Entity<Assigment>().HasMany(c => c.comments).WithOne(c => c.assigment)
                .HasForeignKey(f => f.AssigmentID);

             

            modelBuilder.Entity<Assigment>().HasMany(c => c.StudentAssigments)
                .WithOne(c => c.assigment)
                 .HasForeignKey(f => f.AssigmentId);

             


            modelBuilder.Entity<user>().HasMany(c => c.StudentAssigments)
             .WithOne(c => c.user)
              .HasForeignKey(f => f.UserId);

             

            modelBuilder.Entity<Room>()
              .HasOne(r => r.RoomOwner)
                .WithMany(u => u.CreatedRooms)
               .HasForeignKey(r => r.RoomOwnerID)
             .OnDelete(DeleteBehavior.NoAction);

             
            modelBuilder.Entity<Room>()
                 .HasMany(r => r.Posts)
               .WithOne(p => p.room)
               .HasForeignKey(p => p.roomid);

             

            modelBuilder.Entity<Room>()
                .HasMany(r => r.Assigments)
                .WithOne(a => a.Room)
                .HasForeignKey(a => a.RoomID)
               ;




            modelBuilder.Entity<Room>()
                .HasMany(r => r.JoinedUsers)
                .WithMany(u => u.JoinedRooms)
                .UsingEntity(j => j.ToTable("UserRoom"));



            modelBuilder.Entity<Assigment>().HasOne(c => c.AttachedFile).WithOne(w => w.assigment)
              .HasForeignKey<AssigmentFile>(a => a.AssigmentID);





            modelBuilder.Entity<StudentAssigment>().HasOne(c => c.File).WithOne(w => w.Studentassigment)
             .HasForeignKey<StudentFile>(a => a.StudentassigmentID);

 

            modelBuilder.Entity<user>().HasMany(u => u.postComments).WithOne(c => c.author)
                .HasForeignKey(i => i.authorid);


            modelBuilder.Entity<post>().HasMany(u => u.postComments).WithOne(c => c.post)
               .HasForeignKey(i => i.Postid);

 



        }





    }
}
