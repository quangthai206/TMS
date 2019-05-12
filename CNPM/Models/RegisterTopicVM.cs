using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CNPM.Models
{
    public class RegisterTopicVM
    {
        public int id { get; set; }
        public string name { get; set; }
        public int course_id { get; set; }
        public string description { get; set; }
        public string file_detail { get; set; }
        public int topic_id { get; set; }
        public List<GroupViewModel> groups {get; set;}
    }
}