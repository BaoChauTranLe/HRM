using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HRM.Models;

namespace HRM.Controllers
{
    public class SALARYREPORTsController : Controller
    {
        private hrmserver_HRMEntities db = new hrmserver_HRMEntities();
        public int CalculateTotalAllowance()
        {
            int sum = 0;
            var allowanceList = db.ALLOWANCEs.ToList();
            for (int i = 0; i < allowanceList.Count; i++)
                sum += allowanceList[i].Value;
            return sum;
        }
        public int CalculateStandardSalary(EMPLOYEE e)
        {
            return e.CONTRACT.BasicSalary + CalculateTotalAllowance();
        }
        public int CalculateWorkDaySalary(EMPLOYEE e, DateTime month)
        {
            var timekeepingInfo = db.TIMEKEEPINGREPORTs.Where(x => x.EmployeeID == e.EmployeeID && x.Month.Month == month.Month && x.Month.Year == month.Year).ToList().First();
            return (int) (CalculateStandardSalary(e) * timekeepingInfo.SumWorkDay / db.PARAMETERs.Find("SoNgayCongChuan").Value);
        }

        #region Insurance Pay Calculation related
        public int CalculateMustPayInsuranceAllowance()
        {
            int sum = 0;
            var mustPayList = db.ALLOWANCEs.Where(x => x.Insurance == true).ToList();
            for (int i = 0; i < mustPayList.Count; i++)
                sum += mustPayList[i].Value;
            return sum;
        }
        public int CalculateInsurancePaySalary(EMPLOYEE e)
        {
            return e.CONTRACT.BasicSalary + CalculateMustPayInsuranceAllowance();
        }
        public int CalculateSocialInsurancePay(int insurancePaySalary)
        {
            return (int)(insurancePaySalary * db.PARAMETERs.Find("BHXH").Value);
        }
        public int CalculateHealthInsurancePay(int insurancePaySalary)
        {
            return (int)(insurancePaySalary * db.PARAMETERs.Find("BHYT").Value);
        }
        public int CalculateWorkInsurancePay(int insurancePaySalary)
        {
            return (int)(insurancePaySalary * db.PARAMETERs.Find("BHTN").Value);
        }
        public int CalculateTotalInsurancePay(EMPLOYEE e)
        {
            if (e.FreeInsurance == true)
                return 0;
            int insurancePaySalary = CalculateInsurancePaySalary(e);
            return CalculateSocialInsurancePay(insurancePaySalary) + CalculateHealthInsurancePay(insurancePaySalary) + CalculateWorkInsurancePay(insurancePaySalary);
        }

        #endregion

        #region Income Tax Calculation related
        public int CalculateAllowanceFreeTaxValue()
        {
            int sum = 0;
            var freeTaxList = db.ALLOWANCEs.Where(x => x.FreeTax == true).ToList();
            for (int i = 0; i < freeTaxList.Count; i++)
                sum += (int)freeTaxList[i].FreeTaxValue;
            return sum;
        }
        public int CalculateStandardHourSalary(EMPLOYEE e)
        {
            return (int) (e.CONTRACT.BasicSalary / db.PARAMETERs.Find("SoNgayCongChuan").Value / db.PARAMETERs.Find("SoGioCongChuan").Value);
        }
        public int CalculateTotalWorkHour(EMPLOYEE e, DateTime month)
        {
            var timekeepingInfo = db.TIMEKEEPINGREPORTs.Where(x => x.EmployeeID == e.EmployeeID && x.Month.Month == month.Month && x.Month.Year == month.Year).ToList().First();
            return (int)(timekeepingInfo.SumHourNormal
                       + timekeepingInfo.SumHourDayOff
                       + timekeepingInfo.SumHourSpecialDayOff
                       + timekeepingInfo.SumHourNightNormal
                       + timekeepingInfo.SumHourNightDayOff
                       + timekeepingInfo.SumHourNightSpecialDayOff
                       + timekeepingInfo.SumHourNightNormalExtra
                       + timekeepingInfo.SumHourNightDayOffExtra
                       + timekeepingInfo.SumHourNightSpecialDayOffExtra);
        }
        public int CalculateTaxableOvertimeSalary(EMPLOYEE e, DateTime month)
        {

            return CalculateStandardHourSalary(e) * CalculateTotalWorkHour(e, month);
        }
        public int CalculateTaxableIncome(EMPLOYEE e, DateTime month)
        {
            return CalculateStandardSalary(e) 
                 + CalculateTaxableOvertimeSalary(e, month) 
                 - CalculateTotalInsurancePay(e) 
                 - CalculateAllowanceFreeTaxValue();
        }
        public int CalculateAssessableIncome(EMPLOYEE e, DateTime month)
        {
            int taxable = CalculateTaxableIncome(e, month);
            int deduction = (int)db.PARAMETERs.Find("MucGiamTruNguoiPhuThuoc").Value * e.DependentDeduction;
            if (e.SelfDeduction == true)
                deduction += (int)db.PARAMETERs.Find("MucGiamTruBanThan").Value;
            if (taxable > deduction)
                return taxable - deduction;
            return 0;
        }
        public int CalculateIncomeTax(EMPLOYEE e, DateTime month)
        {
            int assessable = CalculateAssessableIncome(e, month);
            if (assessable == 0)
                return 0;
            int incomeTax = 0;
            int taxLevel = 1;
            var taxRateList = db.TAXRATEs.ToList();
            for (int i = taxRateList.Last().Rank; i > 1; i--)
                if (assessable >= taxRateList[i].Min)
                {
                    taxLevel = i;
                    break;
                }
            for (int i = 1; i < taxLevel; i++)
            {
                incomeTax += (taxRateList[i + 1].Min - taxRateList[i].Min) * taxRateList[i].Rate / 100;
            }
            incomeTax += (assessable - taxRateList[taxLevel].Min) * taxRateList[taxLevel].Rate / 100;
            return incomeTax;
        }
        #endregion
        public int CalculateAdvanced(EMPLOYEE e, DateTime month)
        {
            int advanced = 0;
            var advancedList = db.ADVANCEDs.Where(x => x.EmployeeID == e.EmployeeID && x.DateAdvanced.Month == month.Month && x.DateAdvanced.Year == month.Year).ToList();
            for (int i = 0; i < advancedList.Count; i++)
                advanced += (int)advancedList[i].Value;
            return advanced;
        }
        public int CalculateOvertimeSalary(EMPLOYEE e, DateTime month)
        {
            var timekeepingInfo = db.TIMEKEEPINGREPORTs.Where(x => x.EmployeeID == e.EmployeeID && x.Month.Month == month.Month && x.Month.Year == month.Year).ToList().First();
            int hourPay = CalculateStandardHourSalary(e);
            return (int)(hourPay * timekeepingInfo.SumHourNormal * db.PARAMETERs.Find("HSLNgayThuong").Value)
                 + (int)(hourPay * timekeepingInfo.SumHourDayOff * db.PARAMETERs.Find("HSLNgayNghi").Value)
                 + (int)(hourPay * timekeepingInfo.SumHourSpecialDayOff * db.PARAMETERs.Find("HSLNgayNghiCoLuong").Value)
                 + (int)(hourPay * timekeepingInfo.SumHourNightNormal * db.PARAMETERs.Find("HSLDemNgayThuong").Value)
                 + (int)(hourPay * timekeepingInfo.SumHourNightDayOff * db.PARAMETERs.Find("HSLDemNgayNghi").Value)
                 + (int)(hourPay * timekeepingInfo.SumHourNightSpecialDayOff * db.PARAMETERs.Find("HSLDemNgayNghiCoLuong").Value)
                 + (int)(hourPay * timekeepingInfo.SumHourNightNormalExtra * db.PARAMETERs.Find("HSLDemNgayThuongDaLamNgay").Value)
                 + (int)(hourPay * timekeepingInfo.SumHourNightDayOffExtra * db.PARAMETERs.Find("HSLDemNgayNghiDaLamNgay").Value)
                 + (int)(hourPay * timekeepingInfo.SumHourNightSpecialDayOffExtra * db.PARAMETERs.Find("HSLDemNgayNghiCoLuongDaLamNgay").Value);

        }
        public int CalculateSalary(EMPLOYEE e, DateTime month)
        {
            return CalculateWorkDaySalary(e, month) + CalculateOvertimeSalary(e, month) - CalculateTotalInsurancePay(e) - CalculateIncomeTax(e, month) - CalculateAdvanced(e, month); ;
        }
        // GET: SALARYREPORTs
        public ActionResult Index()
        {
            //var sALARYREPORTs = db.SALARYREPORTs.Include(s => s.EMPLOYEE);
            var sALARYREPORTs = db.SALARYREPORTs.Include(s => s.EMPLOYEE);
            foreach (SALARYREPORT rp in sALARYREPORTs)
            {
                rp.Allowance = CalculateTotalAllowance();
                rp.Advance = CalculateAdvanced(rp.EMPLOYEE, rp.Month);
                rp.IncomeTax = CalculateIncomeTax(rp.EMPLOYEE, rp.Month);
                rp.StandardSalary = CalculateStandardSalary(rp.EMPLOYEE);
                rp.WorkDay = db.TIMEKEEPINGREPORTs.Where(x => x.EmployeeID == rp.EmployeeID && x.Month.Month == rp.Month.Month && x.Month.Year == rp.Month.Year).First().SumWorkDay;
                rp.WorkDaySalary = CalculateWorkDaySalary(rp.EMPLOYEE, rp.Month);
                rp.OvertimeSalary = CalculateOvertimeSalary(rp.EMPLOYEE, rp.Month);
                rp.InsurancePay = CalculateTotalInsurancePay(rp.EMPLOYEE);
                rp.RealSalary = CalculateSalary(rp.EMPLOYEE, rp.Month);
            }    
            return View(sALARYREPORTs.ToList());
        }

        // GET: SALARYREPORTs/Details/5
        public ActionResult Details(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SALARYREPORT sALARYREPORT = db.SALARYREPORTs.Find(id);
            if (sALARYREPORT == null)
            {
                return HttpNotFound();
            }
            return View(sALARYREPORT);
        }

        // GET: SALARYREPORTs/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName");
            return View();
        }

        // POST: SALARYREPORTs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Month,EmployeeID,Allowance,SalaryStandard,WorkDay,SalaryWorkDay,Bonus,Insurance,Overtime,IncomeTax,Advance,RealSalary")] SALARYREPORT sALARYREPORT)
        {
            if (ModelState.IsValid)
            {
                db.SALARYREPORTs.Add(sALARYREPORT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", sALARYREPORT.EmployeeID);
            return View(sALARYREPORT);
        }

        // GET: SALARYREPORTs/Edit/5
        public ActionResult Edit(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SALARYREPORT sALARYREPORT = db.SALARYREPORTs.Find(id);
            if (sALARYREPORT == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", sALARYREPORT.EmployeeID);
            return View(sALARYREPORT);
        }

        // POST: SALARYREPORTs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Month,EmployeeID,Allowance,SalaryStandard,WorkDay,SalaryWorkDay,Bonus,Insurance,Overtime,IncomeTax,Advance,RealSalary")] SALARYREPORT sALARYREPORT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sALARYREPORT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", sALARYREPORT.EmployeeID);
            return View(sALARYREPORT);
        }

        // GET: SALARYREPORTs/Delete/5
        public ActionResult Delete(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SALARYREPORT sALARYREPORT = db.SALARYREPORTs.Find(id);
            if (sALARYREPORT == null)
            {
                return HttpNotFound();
            }
            return View(sALARYREPORT);
        }

        // POST: SALARYREPORTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(DateTime id)
        {
            SALARYREPORT sALARYREPORT = db.SALARYREPORTs.Find(id);
            db.SALARYREPORTs.Remove(sALARYREPORT);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
