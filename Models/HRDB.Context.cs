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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class hrmserver_HRMEntities1 : DbContext
    {
        public hrmserver_HRMEntities1()
            : base("name=hrmserver_HRMEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ABSENT> ABSENTs { get; set; }
        public virtual DbSet<ADVANCED> ADVANCEDs { get; set; }
        public virtual DbSet<ADVANCEDREPORT> ADVANCEDREPORTs { get; set; }
        public virtual DbSet<ALLOWANCE> ALLOWANCEs { get; set; }
        public virtual DbSet<ALLOWANCEDETAIL> ALLOWANCEDETAILs { get; set; }
        public virtual DbSet<ALLOWANCEREPORT> ALLOWANCEREPORTs { get; set; }
        public virtual DbSet<BONU> BONU { get; set; }
        public virtual DbSet<BONUSDETAIL> BONUSDETAILs { get; set; }
        public virtual DbSet<BONUSREPORT> BONUSREPORTs { get; set; }
        public virtual DbSet<CERTIFICATE> CERTIFICATEs { get; set; }
        public virtual DbSet<CERTIFICATEDETAIL> CERTIFICATEDETAILs { get; set; }
        public virtual DbSet<CONTRACT> CONTRACTs { get; set; }
        public virtual DbSet<DATEINFORMATION> DATEINFORMATIONs { get; set; }
        public virtual DbSet<EDUCATION> EDUCATIONs { get; set; }
        public virtual DbSet<EDUCATIONDETAIL> EDUCATIONDETAILs { get; set; }
        public virtual DbSet<EMPLOYEE> EMPLOYEEs { get; set; }
        public virtual DbSet<GR> GRs { get; set; }
        public virtual DbSet<GROUPPERMISSION> GROUPPERMISSIONs { get; set; }
        public virtual DbSet<INSURANCEREPORT> INSURANCEREPORTs { get; set; }
        public virtual DbSet<MAJOR> MAJORs { get; set; }
        public virtual DbSet<PERMISSION> PERMISSIONs { get; set; }
        public virtual DbSet<POSITION> POSITIONs { get; set; }
        public virtual DbSet<ROOM> ROOMs { get; set; }
        public virtual DbSet<SALARYREPORT> SALARYREPORTs { get; set; }
        public virtual DbSet<SHIFT> SHIFTs { get; set; }
        public virtual DbSet<SHIFTDETAIL> SHIFTDETAILs { get; set; }
        public virtual DbSet<TAXREPORT> TAXREPORTs { get; set; }
        public virtual DbSet<TIMEKEEPING> TIMEKEEPINGs { get; set; }
        public virtual DbSet<TIMEKEEPINGREPORT> TIMEKEEPINGREPORTs { get; set; }
        public virtual DbSet<USER> USERS { get; set; }
    }
}
