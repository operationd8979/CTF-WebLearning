using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace bigschool.ViewModels
{
    public class CourseViewMode
    {
        [Required]
        public string Place { set; get; }
        [Required]
        public string Date { set; get; }
        [Required]
        public string Time { set; get; }
        [Required]
        public byte Category { set; get; }
    }
}