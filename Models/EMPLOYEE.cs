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
    
    public partial class EMPLOYEE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EMPLOYEE()
        {
            this.ADVANCEDs = new HashSet<ADVANCED>();
            this.ADVANCEDREPORTs = new HashSet<ADVANCEDREPORT>();
            this.ALLOWANCEDETAILs = new HashSet<ALLOWANCEDETAIL>();
            this.ALLOWANCEREPORTs = new HashSet<ALLOWANCEREPORT>();
            this.BONUSDETAILs = new HashSet<BONUSDETAIL>();
            this.BONUSREPORTs = new HashSet<BONUSREPORT>();
            this.CERTIFICATEDETAILs = new HashSet<CERTIFICATEDETAIL>();
            this.EDUCATIONDETAILs = new HashSet<EDUCATIONDETAIL>();
            this.INSURANCEREPORTs = new HashSet<INSURANCEREPORT>();
            this.SALARYREPORTs = new HashSet<SALARYREPORT>();
            this.SHIFTs = new HashSet<SHIFT>();
            this.TAXREPORTs = new HashSet<TAXREPORT>();
            this.TIMEKEEPINGs = new HashSet<TIMEKEEPING>();
            this.TIMEKEEPINGREPORTs = new HashSet<TIMEKEEPINGREPORT>();
            this.GRs = new HashSet<GR>();
        }
    
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public byte[] Image { get; set; }
        public Nullable<bool> Sex { get; set; }
        public Nullable<System.DateTime> DoB { get; set; }
        public string Birthplace { get; set; }
        public string HomeTown { get; set; }
        public string Nation { get; set; }
        public string Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Ward { get; set; }
        public string Dictrict { get; set; }
        public string RoomID { get; set; }
        public string PositionID { get; set; }
        public string ContractID { get; set; }
        public Nullable<bool> HealthInsurance { get; set; }
        public string HealthInsuranceID { get; set; }
        public string SocialInsuranceID { get; set; }
        public Nullable<bool> DeductionPersonal { get; set; }
        public Nullable<int> DeductionDependent { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ADVANCED> ADVANCEDs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ADVANCEDREPORT> ADVANCEDREPORTs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ALLOWANCEDETAIL> ALLOWANCEDETAILs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ALLOWANCEREPORT> ALLOWANCEREPORTs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BONUSDETAIL> BONUSDETAILs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BONUSREPORT> BONUSREPORTs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CERTIFICATEDETAIL> CERTIFICATEDETAILs { get; set; }
        public virtual CONTRACT CONTRACT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EDUCATIONDETAIL> EDUCATIONDETAILs { get; set; }
        public virtual POSITION POSITION { get; set; }
        public virtual ROOM ROOM { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INSURANCEREPORT> INSURANCEREPORTs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SALARYREPORT> SALARYREPORTs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SHIFT> SHIFTs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TAXREPORT> TAXREPORTs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TIMEKEEPING> TIMEKEEPINGs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TIMEKEEPINGREPORT> TIMEKEEPINGREPORTs { get; set; }
        public virtual USER USER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GR> GRs { get; set; }
    }
}
