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
    
    public partial class TAXREPORT
    {
        public System.DateTime Month { get; set; }
        public string EmployeeID { get; set; }
        public Nullable<int> TotalIncome { get; set; }
        public Nullable<int> FreeTax { get; set; }
        public Nullable<int> Deduction { get; set; }
        public Nullable<int> OverTimeHour { get; set; }
        public Nullable<int> OverTimeFreeTax { get; set; }
        public Nullable<int> TaxableIncome { get; set; }
        public Nullable<int> IncomeTax { get; set; }
    
        public virtual EMPLOYEE EMPLOYEE { get; set; }
    }
}
