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

    [Table("DetailKH")]
    public partial class DetailKH
    {
        public int Id { get; set; }
        public int IdCourse { get; set; }

        public string Link { get; set; }
        
        public static DetailKH findById(int Id)
        {
            using(bigschoolContext bg = new bigschoolContext())
                return bg.DetailKHs.FirstOrDefault(s => s.Id == Id);
        }
        
    }
}
