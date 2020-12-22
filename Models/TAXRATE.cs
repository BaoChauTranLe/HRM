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
    using System.ComponentModel.DataAnnotations;

    public partial class TAXRATE
    {
        public int Rank { get; set; }
        
        [RegularExpression(@"[0-9]*\.?[0-9]*", ErrorMessage = "Giá trị được nhập phải là một số")]
        [Required(ErrorMessage = "Phần thu nhập tính thuế / tháng không được để trống")]
        public int Min { get; set; }
        
        [RegularExpression(@"[0-9]*\.?[0-9]*", ErrorMessage = "Giá trị được nhập phải là một số")]
        [Required(ErrorMessage = "Thuế suất (%) không được để trống")]
        public int Rate { get; set; }
    }
}
