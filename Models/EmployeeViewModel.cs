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
        [Required(ErrorMessage = "Tên nhân viên bắt buộc")]
        public string EmployeeName { get; set; }
        public string Image { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public string Sex { get; set; }
        [Required(ErrorMessage = "Ngày sinh bắt buộc")]
        public DateTime DoB { get; set; }
        public string Birthplace { get; set; }
        public string HomeTown { get; set; }
        public string Nation { get; set; }
        [Required(ErrorMessage = "CMND bắt buộc")]
        [StringLength(maximumLength: 13, ErrorMessage = "Độ dài không hợp lệ", MinimumLength = 9)]
        public string Id { get; set; }
        [Required(ErrorMessage = "Số điện thoại bắt buộc")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Email bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
        public string City { get; set; }
        public string Ward { get; set; }
        public string Dictrict { get; set; }
        public string RoomID { get; set; }
        public string RoomName { get; set; }
        public string PositionID { get; set; }
        public string PositionName { get; set; }
        [Required(ErrorMessage = "Trình độ bắt buộc")]
        public string EducationName { get; set; }
        [Required(ErrorMessage = "Chuyên ngành bắt buộc")]
        public string MajorID { get; set; }
        public string MajorName { get; set; }
        [Required(ErrorMessage = "Ngày cấp bắt buộc")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Nơi đào tạo bắt buộc")]
        public string Place { get; set; }
        [Required(ErrorMessage = "Tên chứng chỉ bắt buộc")]
        public string CertificateName { get; set; }
        public string TypeCertificate { get; set; }
        [Required(ErrorMessage = "Ngày cấp bắt buộc")]
        public DateTime CertificateDate { get; set; }
        [Required(ErrorMessage = "Nơi cấp bắt buộc")]
        public string CertificatePlace { get; set; }
        [Required(ErrorMessage = "Mã hợp đồng bắt buộc")]
        [StringLength(maximumLength: 10, ErrorMessage = "Độ dài không hợp lệ", MinimumLength = 10)]
        public string ContractID { get; set; }
        public string ContractType { get; set; }
        [Required(ErrorMessage = "Ngày bắt đầu bắt buộc")]
        public DateTime? DateStartWork { get; set; }
        [Required(ErrorMessage = "Ngày kết thúc bắt buộc")]
        public DateTime? ContractExpirationDate { get; set; }
        [Required(ErrorMessage = "Lương cơ bản bắt buộc")]
        public int BasicSalary { get; set; }
        public int? TrialTime { get; set; }
        public bool HealthInsurance { get; set; }
        [Required(ErrorMessage = "Mã bảo hiểm y tế bắt buộc")]
        [StringLength(maximumLength: 15, ErrorMessage = "Mã bảo hiểm y tế có 15 ký tự", MinimumLength = 15)]
        public string HealthInsuranceID { get; set; }
        public bool DeductionPersonal { get; set; }
        [Required(ErrorMessage = "Số người đăng ký giảm trừ phụ thuộc bắt buộc")]
        public int DeductionDependent { get; set; }
        [Required(ErrorMessage = "Tên đường bắt buộc")]
        public string Street { get; set; }
        public string SocialInsuranceID { get; set; }
        [Required(ErrorMessage = "Mật khẩu bắt buộc")]
        public string Password { get; set; }
    }
}