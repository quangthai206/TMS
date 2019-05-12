using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CNPM.Models
{
    public class ManageGroupVM
    {
        public int group_id { get; set; }
        public string group_name { get; set; }
        public List<string> members { get; set; }
        public int course_id { get; set; }
        public int topic_id { get; set; }
        public string topic_name { get; set; }
        public string description { get; set; }
        public string file_detail { get; set; }
        

    }
}