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
    
    public partial class hrmserver_HRMEntities : DbContext
    {
        public hrmserver_HRMEntities()
            : base("name=hrmserver_HRMEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ADVANCED> ADVANCEDs { get; set; }
        public virtual DbSet<ALLOWANCE> ALLOWANCEs { get; set; }
        public virtual DbSet<ALLOWANCEDETAIL> ALLOWANCEDETAILs { get; set; }
        public virtual DbSet<CERTIFICATEDETAIL> CERTIFICATEDETAILs { get; set; }
        public virtual DbSet<CONTRACT> CONTRACTs { get; set; }
        public virtual DbSet<DATEINFORMATION> DATEINFORMATIONs { get; set; }
        public virtual DbSet<EDUCATIONDETAIL> EDUCATIONDETAILs { get; set; }
        public virtual DbSet<EMPLOYEE> EMPLOYEEs { get; set; }
        public virtual DbSet<INSURANCEREPORT> INSURANCEREPORTs { get; set; }
        public virtual DbSet<MAJOR> MAJORs { get; set; }
        public virtual DbSet<PARAMETER> PARAMETERs { get; set; }
        public virtual DbSet<POSITION> POSITIONs { get; set; }
        public virtual DbSet<ROOM> ROOMs { get; set; }
        public virtual DbSet<SALARYREPORT> SALARYREPORTs { get; set; }
        public virtual DbSet<SHIFT> SHIFTs { get; set; }
        public virtual DbSet<SHIFTDETAIL> SHIFTDETAILs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TAXRATE> TAXRATEs { get; set; }
        public virtual DbSet<TAXREPORT> TAXREPORTs { get; set; }
        public virtual DbSet<TIMEKEEPING> TIMEKEEPINGs { get; set; }
        public virtual DbSet<TIMEKEEPINGREPORT> TIMEKEEPINGREPORTs { get; set; }
        public virtual DbSet<USER> USERS { get; set; }
    }
}
