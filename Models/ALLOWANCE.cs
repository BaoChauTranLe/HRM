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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ALLOWANCE()
        {
            this.ALLOWANCEDETAILs = new HashSet<ALLOWANCEDETAIL>();
        }
    
        public string AllowanceID { get; set; }
        public string AllowanceName { get; set; }
        public Nullable<bool> Insurance { get; set; }
        public Nullable<bool> Tax { get; set; }
        public Nullable<int> FreeTax { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ALLOWANCEDETAIL> ALLOWANCEDETAILs { get; set; }
    }
}
