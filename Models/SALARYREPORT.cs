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
    
    public partial class SALARYREPORT
    {
        public System.DateTime Month { get; set; }
        public string EmployeeID { get; set; }
        public Nullable<int> Allowance { get; set; }
        public Nullable<int> SalaryStandard { get; set; }
        public Nullable<int> WorkDay { get; set; }
        public Nullable<int> SalaryWorkDay { get; set; }
        public Nullable<int> Bonus { get; set; }
        public Nullable<int> Insurance { get; set; }
        public Nullable<int> Overtime { get; set; }
        public Nullable<int> IncomeTax { get; set; }
        public Nullable<int> Advance { get; set; }
        public Nullable<int> RealSalary { get; set; }
    
        public virtual EMPLOYEE EMPLOYEE { get; set; }
    }
}
