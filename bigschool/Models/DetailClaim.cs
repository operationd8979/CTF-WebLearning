namespace bigschool.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("DetailClaim")]
    public partial class DetailClaim
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string IdUser { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdChallenge { get; set; }

        public DateTime Time { get; set; }

        public virtual Challenge Challenge { get; set; }


        public static List<DetailClaim> GetAll()
        {
            using (bigschoolContext db = new bigschoolContext())
            {
                try
                {
                    return db.DetailClaims.ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
