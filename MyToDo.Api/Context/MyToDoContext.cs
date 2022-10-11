
using Microsoft.EntityFrameworkCore;


namespace MyToDo.Api.Context
{
    public class MyToDoContext : DbContext
    {
        public MyToDoContext(DbContextOptions<MyToDoContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<ToDo> ToDo { get; set; }
        public DbSet<Memo> Memo { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 


        }
    }
}
