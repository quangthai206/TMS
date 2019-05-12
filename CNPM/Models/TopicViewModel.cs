using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CNPM.Models
{
    public class TopicViewModel
    {
        public int id { get; set; }
        [Display(Name = "Tên đề tài")]
        [Required(ErrorMessage = "Tên đề tài là bắt buộc")]
        public string name { get; set; }
        public int course_id { get; set; }
        [Display(Name = "Mô tả")]
        [Required(ErrorMessage = "Mô tả là bắt buộc")]
        public string description { get; set; }
        [Display(Name = "File chi tiết")]
        public string file_detail { get; set; }
        public int totalGroup { get; set; }
    
    }
}