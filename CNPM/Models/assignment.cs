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
    
    public partial class assignment
    {
        public int id { get; set; }
        public Nullable<int> course_id { get; set; }
        public string name { get; set; }
        public string body { get; set; }
        public Nullable<System.DateTime> deadline { get; set; }
    
        public virtual course course { get; set; }
    }
}
