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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class SHIFT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SHIFT()
        {
            this.SHIFTDETAILs = new HashSet<SHIFTDETAIL>();
        }
    
        public string ShiftID { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên ca.")]
        public string ShiftName { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn loại ca.")]
        public string ShiftType { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn giờ bắt đầu.")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public System.DateTime StartTime { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn giờ kết thúc.")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [TimeGreaterThanAttribute(otherPropertyName = "StartTime", ErrorMessage = "Giờ bắt đầu phải sớm hơn giờ kết thúc")]
        public System.DateTime EndTime { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SHIFTDETAIL> SHIFTDETAILs { get; set; }
    }
}
