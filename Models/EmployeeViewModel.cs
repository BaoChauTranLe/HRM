using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRM.Models
{
    public class EmployeeViewModel
    {
        public string EmployeeID { get; set; }
        [Required(ErrorMessage = "Bat buoc")]
        public string EmployeeName { get; set; }
        public byte[] Image { get; set; }
        public string Sex { get; set; }
        [Required(ErrorMessage = "Bat buoc")]
        public DateTime? DoB { get; set; }
        public string Birthplace { get; set; }
        public string HomeTown { get; set; }
        public string Nation { get; set; }
        [Required(ErrorMessage = "Bat buoc")]
        public string Id { get; set; }
        [Required(ErrorMessage = "Bat buoc")]
        [Phone(ErrorMessage = "So dien thoai khong hop le")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Bat buoc")]
        [EmailAddress(ErrorMessage = "Email khong hop le")]
        public string Email { get; set; }
        public string City { get; set; }
        public string Ward { get; set; }
        public string Dictrict { get; set; }
        public string RoomID { get; set; }
        public string RoomName { get; set; }
        public string PositionID { get; set; }
        public string PositionName { get; set; }
        [Required(ErrorMessage = "Bat buoc")]
        public List<string> EducationID { get; set; }
        public List<string> MajorID { get; set; }
        [Required(ErrorMessage = "Bat buoc")]
        public List<DateTime?> Date { get; set; }
        [Required(ErrorMessage = "Bat buoc")]
        public List<string> Place { get; set; }
        [Required(ErrorMessage = "Bat buoc")]
        public List<string> CertificateName { get; set; }
        public List<string> TypeCertificateID { get; set; }
        [Required(ErrorMessage = "Bat buoc")]
        public List<DateTime?> CertificateDate { get; set; }
        [Required(ErrorMessage = "Bat buoc")]
        public List<string> CertificatePlace { get; set; }
        [Required(ErrorMessage = "Bat buoc")]
        public string ContractID { get; set; }
        public string ContractType { get; set; }
        [Required(ErrorMessage = "Bat buoc")]
        public DateTime? DateStartWork { get; set; }
        [Required(ErrorMessage = "Bat buoc")]
        public DateTime? ContractExpirationDate { get; set; }
        [Required(ErrorMessage = "Bat buoc")]
        public int BasicSalary { get; set; }
        [Required(ErrorMessage = "Bat buoc")]
        public string PersonalIncomeTax { get; set; }
        public int? TrialTime { get; set; }
        public string HealthInsurance { get; set; }
        [Required(ErrorMessage = "Bat buoc")]
        public string HealthInsuranceID { get; set; }
        public string DeductionPersonal { get; set; }
        [Required(ErrorMessage = "Bat buoc")]
        public int DeductionDependent { get; set; }
        [Required(ErrorMessage = "Bat buoc")]
        public string Street { get; set; }
    }
}