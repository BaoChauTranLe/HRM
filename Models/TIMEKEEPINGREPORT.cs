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
    
    public partial class TIMEKEEPINGREPORT
    {
        public System.DateTime Month { get; set; }
        public string EmployeeID { get; set; }
        public int SumWorkDay { get; set; }
        public int SumAbsentHaveSalary { get; set; }
        public int SumAbsentNoSalary { get; set; }
        public int SumHourNormal { get; set; }
        public int SumHourDayOff { get; set; }
        public int SumHourSpecialDayOff { get; set; }
        public int SumHourNightNormal { get; set; }
        public int SumHourNightDayOff { get; set; }
        public int SumHourNightSpecialDayOff { get; set; }
    
        public virtual EMPLOYEE EMPLOYEE { get; set; }
    }
}
