//------------------------------------------------------------------------------
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
    
    public partial class ALLOWANCE
    {
        public string AllowanceID { get; set; }
        public string AllowanceName { get; set; }
        public bool Insurance { get; set; }
        public bool FreeTax { get; set; }
        public Nullable<int> FreeTaxValue { get; set; }
        public int Value { get; set; }
    }
}
