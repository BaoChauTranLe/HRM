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
        public string EmployeeName { get; set; }
        public byte[] Image { get; set; }
        public string Sex { get; set; }
        [Required(ErrorMessage = "Ngay sinh la truong bat buoc")]
        public DateTime DoB { get; set; }
        public string Birthplace { get; set; }
        public string HomeTown { get; set; }
        public string Nation { get; set; }
        public string Id { get; set; }
        [Phone(ErrorMessage = "So dien thoai khong hop le")]
        public string Phone { get; set; }
        [EmailAddress(ErrorMessage = "Email khong hop le")]
        public string Email { get; set; }
        public string City { get; set; }
        public string Ward { get; set; }
        public string Dictrict { get; set; }
        public string RoomID { get; set; }
        public string RoomName { get; set; }
        public string PositionID { get; set; }
        public string PositionName { get; set; }
        public List<string> EducationID { get; set; }
        public List<string> MajorID { get; set; }
        public List<DateTime> Date { get; set; }
        public List<string> Place { get; set; }
        public List<string> CertificateName { get; set; }
        public List<string> TypeCertificateID { get; set; }
        public List<DateTime> CertificateDate { get; set; }
        public List<string> CertificatePlace { get; set; }
        public string ContractID { get; set; }
        public string ContractType { get; set; }
        public DateTime DateStartWork { get; set; }
        public DateTime ContractExpirationDate { get; set; }
        public int BasicSalary { get; set; }
        public string PersonalIncomeTax { get; set; }
        public int TrialTime { get; set; }
        public string HealthInsurance { get; set; }
        public string HealthInsuranceID { get; set; }
        public string DeductionPersonal { get; set; }
        public int DeductionDependent { get; set; }
    }
}