namespace bigschool.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Spatial;
    using System.Data.SqlTypes;
    using System.Linq;

    [Table("Course")]
    public partial class Course
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        [StringLength(128)]
        public string LecturerId { get; set; }

        [Required]
        [StringLength(255)]
        public string Place { get; set; }
 
        public DateTime DateTime { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public double Fee { get; set; }

        public virtual Category Category { get; set; }

        public List<Category> ListCategory = new List<Category>();

        public bool? IsCanceled { get; set; }

        [StringLength(50)]
        public string Pic { get; set; }
        public string Des { get; set; }

        public bool isShowGoing  = false;
        public bool isShowFollow  = false;


        public Course()
        {

        }
        public Course(Course obj)
        {
            this.Id = obj.Id;
            this.Name = obj.Name;
            this.Place = obj.Place;
            this.LecturerId = obj.LecturerId;
            this.DateTime = obj.DateTime;
            this.CategoryId = obj.CategoryId;
            this.Fee = obj.Fee;
            this.Category = obj.Category;
            this.Pic = obj.Pic;
            this.IsCanceled = obj.IsCanceled;
            this.isShowFollow = obj.isShowFollow;
        }

        public void Insert()
        {
            bigschoolContext dbcontext = new bigschoolContext();
            try
            {
                dbcontext.Courses.Add(this);
                dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Course FindById(int? id)
        {
            bigschoolContext dbcontext = new bigschoolContext();
            return dbcontext.Courses.FirstOrDefault(s => s.Id == id);
        }
        public void Remove()
        {
            using (bigschoolContext db = new bigschoolContext())
            {
                try
                {
                    db.Entry(this).State = EntityState.Modified;
                    db.Courses.Remove(this);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
        public void Update()
        {
            bigschoolContext db = new bigschoolContext();
            try
            {
                Course find = db.Courses.FirstOrDefault(r => r.Id == this.Id);
                if (find != null)
                {
                    find.Name = this.Name;
                    find.DateTime = this.DateTime;
                    find.Place = this.Place;
                    find.CategoryId = this.CategoryId;
                }
                db.Courses.AddOrUpdate(this);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
