﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRM.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TAXRATE
    {
        public int Rank { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]*", ErrorMessage = "Giá trị được nhập phải là một số")]
        public int Min { get; set; }
        [Required(ErrorMessage = "Thuế suất (%) không được để trống")]
        [RegularExpression(@"[0-9]*\.?[0-9]*", ErrorMessage = "Giá trị được nhập phải là một số")]
        public int Rate { get; set; }
    }
}
