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
    
    public partial class INSURANCEREPORT
    {
        public System.DateTime Month { get; set; }
        public string EmployeeID { get; set; }
        public Nullable<int> SalaryInsurance { get; set; }
        public Nullable<int> UnionFee { get; set; }
        public Nullable<int> SocialInsurance1 { get; set; }
        public Nullable<int> HealthInsurance1 { get; set; }
        public Nullable<int> UnemployeeInsurance1 { get; set; }
        public Nullable<int> InsuranceAndUnionFee { get; set; }
        public Nullable<int> SocialInsurance2 { get; set; }
        public Nullable<int> HealthInsurance2 { get; set; }
        public Nullable<int> UnemployeeInsurance2 { get; set; }
        public Nullable<int> SubSalary { get; set; }
    
        public virtual EMPLOYEE EMPLOYEE { get; set; }
    }
}
