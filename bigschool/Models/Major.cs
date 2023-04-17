namespace bigschool.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Common;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Major")]
    public partial class Major
    {
        /*[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Major()
        {
            Courses = new HashSet<Course>();
            Challenges = new HashSet<Challenge>();
        }*/

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }
        /*
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Course> Courses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Challenge> Challenges { get; set; }
        */

        public static List<Major> GetAll()
        {
            using (bigschoolContext db = new bigschoolContext())
            {
                try
                {
                    return db.Majors.ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

    }
}
