//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CNPM.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class student_course
    {
        public int student_id { get; set; }
        public int course_id { get; set; }
        public System.DateTime create_at { get; set; }
    
        public virtual course course { get; set; }
        public virtual student student { get; set; }
    }
}
