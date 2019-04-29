﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CNPM.Models
{
    public class AddTopicVM
    {
        public int id { get; set; }
        [Display(Name = "Tên đề tài")]
        [Required(ErrorMessage = "This field is required")]
        public string name { get; set; }
        public int course_id { get; set; }
        [Display(Name = "Mô tả")]
        [Required(ErrorMessage = "This field is required")]
        public string description { get; set; }
        [Display(Name = "File chi tiết")]
        public HttpPostedFileBase file_detail { get; set; }
    }
}