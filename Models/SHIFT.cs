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
    
    public partial class SHIFT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SHIFT()
        {
            this.SHIFTDETAILs = new HashSet<SHIFTDETAIL>();
        }
    
        public string ShiftID { get; set; }
        public string EmployeeID { get; set; }
        public string ShiftName { get; set; }
        public string ShiftType { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
    
        public virtual EMPLOYEE EMPLOYEE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SHIFTDETAIL> SHIFTDETAILs { get; set; }
    }
}
