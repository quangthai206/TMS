using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CNPM.Models
{
    public class GroupViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int course_id { get; set; }
        public int topic_id { get; set; }
        public List<string> members { get; set; }

    }
}