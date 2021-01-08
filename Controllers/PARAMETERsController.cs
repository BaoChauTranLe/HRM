using HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.Controllers
{
    public class PARAMETERsController : Controller
    {
        private hrmserver_HRMEntities db = new hrmserver_HRMEntities();
        // GET: PARAMETERs
        public ActionResult Employee()
        {
            Session["MainTitle"] = "Thiết lập quy định";
            Session["SubTitle"] = "Hồ sơ nhân viên";
            return View();
        }
        public ActionResult EmployeeEdit()
        {
            Session["MainTitle"] = "Thiết lập quy định";
            Session["SubTitle"] = "Chỉnh sửa quy định hồ sơ nhân viên";
            return View();
        }
        [HttpPost]
        public ActionResult EmployeeEdit(int MinFemaleAge, int MaxFemaleAge, int MinMaleAge, int MaxMaleAge, int AreaMinSalary)
        {
            if (ModelState.IsValid)
            {
                db.PARAMETERs.Find("TuoiToiThieuNu").Value = MinFemaleAge;
                db.PARAMETERs.Find("TuoiToiDaNu").Value = MaxFemaleAge;
                db.PARAMETERs.Find("TuoiToiThieuNam").Value = MinMaleAge;
                db.PARAMETERs.Find("TuoiToiDaNam").Value = MaxMaleAge;
                db.PARAMETERs.Find("MucLuongToiThieuVung").Value = AreaMinSalary;
                db.SaveChanges();
                return RedirectToAction("Employee");
            }    
            return View();
        }
        public ActionResult CoefficientsOTSalary()
        {
            Session["MainTitle"] = "Thiết lập quy định";
            Session["SubTitle"] = "Hệ số lương làm thêm";
            return View();
        }
        public ActionResult CoefficientsOTSalaryEdit()
        {
            Session["MainTitle"] = "Thiết lập quy định";
            Session["SubTitle"] = "Chỉnh sửa hệ số lương làm thêm";
            return View();
        }
        [HttpPost]
        public ActionResult CoefficientsOTSalaryEdit(int WorkDay, int AnnualDayOff, int SpecialDayOff, int NightWorkDay, int NightAnnualDayOff, int NightSpecialDayOff)
        {
            if (ModelState.IsValid)
            {
                db.PARAMETERs.Find("HSLNgayThuong").Value = WorkDay;
                db.PARAMETERs.Find("HSLNgayNghi").Value = AnnualDayOff;
                db.PARAMETERs.Find("HSLNgayNghiCoLuong").Value = SpecialDayOff;
                db.PARAMETERs.Find("HSLDemNgayThuong").Value = NightWorkDay;
                db.PARAMETERs.Find("HSLDemNgayNghi").Value = NightAnnualDayOff;
                db.PARAMETERs.Find("HSLDemNgayNghiCoLuong").Value = NightSpecialDayOff;
                db.SaveChanges();
                return RedirectToAction("CoefficientsOTSalary");
            }
            return View();
        }
        public ActionResult IncomeTax()
        {
            Session["MainTitle"] = "Thiết lập quy định";
            Session["SubTitle"] = "Thuế thu nhập cá nhân";
            return View();
        }
        public ActionResult IncomeTaxEdit()
        {
            Session["MainTitle"] = "Thiết lập quy định";
            Session["SubTitle"] = "Chỉnh sửa quy định thuế thu nhập cá nhân";
            return View();
        }
        [HttpPost]
        public ActionResult IncomeTaxEdit(int SelfDeduction, int DependentDeduction, int MinAssessableIncome, float TaxRate)
        {
            if (ModelState.IsValid)
            {
                db.PARAMETERs.Find("MucGiamTruBanThan").Value = SelfDeduction;
                db.PARAMETERs.Find("MucGiamTruNguoiPhuThuoc").Value = DependentDeduction;
                db.PARAMETERs.Find("ThuNhapChiuThueToiThieuHDDuoi3Thang").Value = MinAssessableIncome;
                db.PARAMETERs.Find("HSThueHDDuoi3Thang").Value = TaxRate;
                db.SaveChanges();
                return RedirectToAction("IncomeTax");
            }
            return View();
        }
        public ActionResult Insurance()
        {
            Session["MainTitle"] = "Thiết lập quy định";
            Session["SubTitle"] = "Tỷ lệ trích bảo hiểm";
            return View();
        }
        public ActionResult InsuranceEdit()
        {
            Session["MainTitle"] = "Thiết lập quy định";
            Session["SubTitle"] = "Chỉnh sửa tỷ lệ trích bảo hiểm";
            return View();
        }
        [HttpPost]
        public ActionResult InsuranceEdit(float PersonalSocialInsurance, float PersonalHealthInsurance, float PersonalWorkInsurance, float BusinessSocialInsurance, float BusinessHealthInsurance, float BusinessWorkInsurance, float UnionFee)
        {
            if (ModelState.IsValid)
            {
                db.PARAMETERs.Find("BHXHLD").Value = PersonalSocialInsurance;
                db.PARAMETERs.Find("BHYTLD").Value = PersonalHealthInsurance;
                db.PARAMETERs.Find("BHTNLD").Value = PersonalWorkInsurance;
                db.PARAMETERs.Find("BHXHDN").Value = BusinessSocialInsurance;
                db.PARAMETERs.Find("BHYTDN").Value = BusinessHealthInsurance;
                db.PARAMETERs.Find("BHTNDN").Value = BusinessWorkInsurance;
                db.PARAMETERs.Find("KPCD").Value = UnionFee;
                db.SaveChanges();
                return RedirectToAction("Insurance");
            }
            return View();
        }
        public ActionResult StandardWork()
        {
            Session["MainTitle"] = "Thiết lập quy định";
            Session["SubTitle"] = "Số ngày/giờ công chuẩn";
            return View();
        }
        public ActionResult StandardWorkEdit()
        {
            Session["MainTitle"] = "Thiết lập quy định";
            Session["SubTitle"] = "Chỉnh sửa số ngày/giờ công chuẩn";
            return View();
        }
        [HttpPost]
        public ActionResult StandardWorkEdit(int WorkDayNum, int WorkHourNum)
        {
            if (ModelState.IsValid)
            {
                db.PARAMETERs.Find("SoNgayCongChuan").Value = WorkDayNum;
                db.PARAMETERs.Find("SoGioCongChuan").Value = WorkHourNum;
                db.SaveChanges();
                return RedirectToAction("StandardWork");
            }
            return View();
        }
        public ActionResult Advance()
        {
            Session["MainTitle"] = "Thiết lập quy định";
            Session["SubTitle"] = "Tạm ứng lương";
            return View();
        }
        public ActionResult AdvanceEdit()
        {
            Session["MainTitle"] = "Thiết lập quy định";
            Session["SubTitle"] = "Chỉnh sửa quy định tạm ứng lương";
            return View();
        }
        [HttpPost]
        public ActionResult AdvanceEdit(float MaxAdvanceRate)
        {
            if (ModelState.IsValid)
            {
                db.PARAMETERs.Find("TyLeTamUngToiDa").Value = MaxAdvanceRate;
                db.SaveChanges();
                return RedirectToAction("Advance");
            }
            return View();
        }
        public double getValueByName(string paraName)
        {
            var parameter = db.PARAMETERs.Find(paraName);
            return parameter.Value;
        }
    }
}