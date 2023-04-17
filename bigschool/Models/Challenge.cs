namespace bigschool.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Challenge")]
    public partial class Challenge
    {
        public int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        public string Des { get; set; }

        public int PointGet { get; set; }

        public DateTime TimePost { get; set; }

        [DisplayName("Flag")]
        public string flag { get; set; }


        [Required]
        [StringLength(128)]
        public string IdUser { get; set; }


        public static List<Challenge> GetAll()
        {
            using (bigschoolContext db = new bigschoolContext())
            {
                try
                {
                    return db.Challenges.ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static Challenge FindById(int id)
        {
            using (bigschoolContext db = new bigschoolContext())
            {
                try
                {
                    return db.Challenges.FirstOrDefault(c => c.Id == id);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
