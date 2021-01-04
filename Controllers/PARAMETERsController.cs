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
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Employee()
        {
            return View();
        }
        public ActionResult EmployeeEdit()
        {
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
            return View();
        }
        public ActionResult CoefficientsOTSalaryEdit()
        {
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
            return View();
        }
        public ActionResult IncomeTaxEdit()
        {
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
            return View();
        }
        public ActionResult InsuranceEdit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult InsuranceEdit(float SocialInsurance, float HealthInsurance, float WorkInsurance)
        {
            if (ModelState.IsValid)
            {
                db.PARAMETERs.Find("BHXH").Value = SocialInsurance;
                db.PARAMETERs.Find("BHYT").Value = HealthInsurance;
                db.PARAMETERs.Find("BHTN").Value = WorkInsurance;
                db.SaveChanges();
                return RedirectToAction("Insurance");
            }
            return View();
        }
        public ActionResult StandardWork()
        {
            return View();
        }
        public ActionResult StandardWorkEdit()
        {
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
            return View();
        }
        public ActionResult AdvanceEdit()
        {
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