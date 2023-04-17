using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace bigschool.Models
{
    public partial class bigschoolContext : DbContext
    {
        public bigschoolContext()
            : base("name=bigschoolContext")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<Following> Followings { get; set; }
        public virtual DbSet<Challenge> Challenges { get; set; }
        public virtual DbSet<Major> Majors { get; set; }
        public virtual DbSet<DetailClaim> DetailClaims { get; set; }
        public virtual DbSet<DetailChallengeTiles> DetailChallengeTiless { get; set; }
        public virtual DbSet<DetailKH> DetailKHs { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Courses)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);
        }
    }
}
