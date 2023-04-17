namespace bigschool.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Attendance")]
    public partial class Attendance
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CourseId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Attendee { get; set; }

        public virtual Course Course { get; set; }



        public static List<Attendance> GetAll()
        {
            using (bigschoolContext db = new bigschoolContext())
            {
                try
                {
                    return db.Attendances.ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
