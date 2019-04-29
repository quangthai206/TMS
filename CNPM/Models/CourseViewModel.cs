using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CNPM.Models
{
    public class CourseViewModel
    {
        public int id { get; set; }
        public string code { get; set; }
        public int subject_id { get; set; }
        public int teacher_id { get; set; }
        public string subject_name { get; set; }
    }
}