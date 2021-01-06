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
        #region Utility methods
        public string CurrencyFormat(int num)
        {
            return String.Format("{0:n0}", num);
        }
        public double getValueByName(string paraName)
        {
            var parameter = db.PARAMETERs.Find(paraName);
            return parameter.Value;
        }
        public bool CheckLowerThan3Month(DateTime startDate, DateTime endDate)
        {
            DateTime threeMonthAfterStart = startDate.AddMonths(3);
            if (DateTime.Compare(endDate, threeMonthAfterStart) < 0)
                return true;
            return false;
        }
        public int getSumHourNormal(string id, DateTime month)
        {
            return db.TIMEKEEPINGREPORTs.Find(month, id).SumHourNormal;
        }
        public int getSumHourDayOff(string id, DateTime month)
        {
            return db.TIMEKEEPINGREPORTs.Find(month, id).SumHourDayOff;
        }
        public int getSumHourSpecialDayOff(string id, DateTime month)
        {
            return db.TIMEKEEPINGREPORTs.Find(month, id).SumHourSpecialDayOff;
        }
        public int getSumHourNightNormal(string id, DateTime month)
        {
            return db.TIMEKEEPINGREPORTs.Find(month, id).SumHourNightNormal;
        }
        public int getSumHourNightDayOff(string id, DateTime month)
        {
            return db.TIMEKEEPINGREPORTs.Find(month, id).SumHourNightDayOff;
        }
        public int getSumHourNightSpecialDayOff(string id, DateTime month)
        {
            return db.TIMEKEEPINGREPORTs.Find(month, id).SumHourNightSpecialDayOff;
        }
        #endregion


        #region Allowance Calculation
        public int CalculateCommonAllowance()
        {
            var commonList = db.ALLOWANCEs.Where(x => x.AllEmployee == true).ToList();
            int sum = 0;
            foreach (ALLOWANCE a in commonList)
            {
                sum += (int)a.Value;
            }
            return sum;
        }
        public int CalculateSpecificAllowance(EMPLOYEE e, DateTime month)
        {
            var allowanceList = db.ALLOWANCEDETAILs.Where(x => x.EmployeeID == e.EmployeeID && x.Month.Month == month.Month && x.Month.Year == month.Year).ToList();
            int sum = 0;
            foreach (ALLOWANCEDETAIL a in allowanceList)
            {
                sum += a.Value;
            }
            return sum;
        }
        public int CalculateTotalAllowance(EMPLOYEE e, DateTime month)
        {
            return CalculateCommonAllowance() + CalculateSpecificAllowance(e, month);
        }
        #endregion
        public int CalculateStandardHourSalary(EMPLOYEE e)
        {
            return (int)(e.CONTRACT.BasicSalary / db.PARAMETERs.Find("SoNgayCongChuan").Value / db.PARAMETERs.Find("SoGioCongChuan").Value);
        }

        public int CalculateStandardSalary(EMPLOYEE e, DateTime month)
        {
            return e.CONTRACT.BasicSalary + CalculateTotalAllowance(e, month);
        }

        public int CalculateWorkDaySalary(EMPLOYEE e, DateTime month)
        {
            var timekeepingInfo = db.TIMEKEEPINGREPORTs.Find(month, e.EmployeeID);
            return (int) (CalculateStandardSalary(e, month) / db.PARAMETERs.Find("SoNgayCongChuan").Value * timekeepingInfo.SumWorkDay);
        }

        #region Insurance Pay Calculation related
        public int CalculateMustPayInsuranceAllowance(EMPLOYEE e, DateTime month)
        {
            int sum = 0;
            var commonMustPayList = db.ALLOWANCEs.Where(x => x.Insurance == true && x.AllEmployee == true).ToList();
            foreach (ALLOWANCE a in commonMustPayList)
            {
                sum += (int) a.Value;
            }
            var specificMustPayList = db.ALLOWANCEDETAILs.Where(x => x.EmployeeID == e.EmployeeID && x.Month.Month == month.Month && x.Month.Year == month.Year && x.ALLOWANCE.Insurance == true).ToList();
            foreach (ALLOWANCEDETAIL a in specificMustPayList)
            {
                sum += a.Value;
            }    
            return sum;
        }
        public int CalculateInsurancePaySalary(EMPLOYEE e, DateTime month)
        {
            return e.CONTRACT.BasicSalary + CalculateMustPayInsuranceAllowance(e, month);
        }
        public int CalculateSocialInsurancePay(int insurancePaySalary)
        {
            return (int)(insurancePaySalary * db.PARAMETERs.Find("BHXH").Value) / 100;
        }
        public int CalculateHealthInsurancePay(int insurancePaySalary)
        {
            return (int)(insurancePaySalary * db.PARAMETERs.Find("BHYT").Value / 100);
        }
        public int CalculateWorkInsurancePay(int insurancePaySalary)
        {
            return (int)(insurancePaySalary * db.PARAMETERs.Find("BHTN").Value) / 100;
        }
        public int CalculateTotalInsurancePay(EMPLOYEE e, DateTime month)
        {
            if (e.FreeInsurance == true)
                return 0;
            int insurancePaySalary = CalculateInsurancePaySalary(e, month);
            return CalculateSocialInsurancePay(insurancePaySalary) + CalculateHealthInsurancePay(insurancePaySalary) + CalculateWorkInsurancePay(insurancePaySalary);
        }

        #endregion

        #region Overtime Salary Calculation related
        public int CalculateNormalOvertimeSalary(TIMEKEEPINGREPORT rp, int hourPay)
        {
            return (int)(hourPay * rp.SumHourNormal * db.PARAMETERs.Find("HSLNgayThuong").Value / 100);
        }
        public int CalculateDayOffOvertimeSalary(TIMEKEEPINGREPORT rp, int hourPay)
        {
            return (int)(hourPay * rp.SumHourDayOff * db.PARAMETERs.Find("HSLNgayNghi").Value / 100);
        }
        public int CalculateSpecialDayOffOvertimeSalary(TIMEKEEPINGREPORT rp, int hourPay)
        {
            return (int)(hourPay * rp.SumHourSpecialDayOff * db.PARAMETERs.Find("HSLNgayNghiCoLuong").Value / 100);
        }
        public int CalculateNightNormalOvertimeSalary(TIMEKEEPINGREPORT rp, int hourPay)
        {
            return (int)(hourPay * rp.SumHourNightNormal * db.PARAMETERs.Find("HSLDemNgayThuong").Value / 100);
        }
        public int CalculateNightDayOffOvertimeSalary(TIMEKEEPINGREPORT rp, int hourPay)
        {
            return (int)(hourPay * rp.SumHourNightDayOff * db.PARAMETERs.Find("HSLDemNgayNghi").Value / 100);
        }
        public int CalculateNightSpecialDayOffOvertimeSalary(TIMEKEEPINGREPORT rp, int hourPay)
        {
            return (int)(hourPay * rp.SumHourNightSpecialDayOff * db.PARAMETERs.Find("HSLDemNgayNghiCoLuong").Value / 100);
        }
        public int CalculateOvertimeSalary(EMPLOYEE e, DateTime month)
        {
            var timekeepingInfo = db.TIMEKEEPINGREPORTs.Find(month, e.EmployeeID );
            int hourPay = CalculateStandardHourSalary(e);
            return CalculateNormalOvertimeSalary(timekeepingInfo, hourPay) +
                   CalculateDayOffOvertimeSalary(timekeepingInfo, hourPay) +
                   CalculateSpecialDayOffOvertimeSalary(timekeepingInfo, hourPay) +
                   CalculateNightNormalOvertimeSalary(timekeepingInfo, hourPay) +
                   CalculateNightDayOffOvertimeSalary(timekeepingInfo, hourPay) +
                   CalculateNightSpecialDayOffOvertimeSalary(timekeepingInfo, hourPay);
        }
        #endregion

        public int CalculateAbsentSalary(EMPLOYEE e, DateTime month)
        {
            int absentAllowanceReceived = 0;
            var commonList = db.ALLOWANCEs.Where(x => x.Type == true && x.AllEmployee == true).ToList();
            foreach (ALLOWANCE a in commonList)
            {
                absentAllowanceReceived += (int) a.Value;
            }
            var specificList = db.ALLOWANCEDETAILs.Where(x => x.EmployeeID == e.EmployeeID && x.Month == month && x.ALLOWANCE.Type == true).ToList();
            foreach (ALLOWANCEDETAIL a in specificList)
            {
                absentAllowanceReceived += a.Value;
            }
            int absentHaveSalary = db.TIMEKEEPINGREPORTs.Find(month, e.EmployeeID).SumAbsentHaveSalary;
            return (int)((e.CONTRACT.BasicSalary + absentAllowanceReceived)/db.PARAMETERs.Find("SoNgayCongChuan").Value * absentHaveSalary);
        }

        #region Income Tax Calculation related
        public int CalculateAllowanceFreeTaxValue()
        {
            int sum = 0;
            var freeTaxList = db.ALLOWANCEs.Where(x => x.FreeTax == true).ToList();
            foreach (ALLOWANCE a in freeTaxList)
            {
                sum += (int)a.FreeTaxValue;
            }
            return sum;
        }
        public int CalculateTotalWorkHour(EMPLOYEE e, DateTime month)
        {
            var timekeepingInfo = db.TIMEKEEPINGREPORTs.Find(month, e.EmployeeID);
            return (int)(timekeepingInfo.SumHourNormal
                       + timekeepingInfo.SumHourDayOff
                       + timekeepingInfo.SumHourSpecialDayOff
                       + timekeepingInfo.SumHourNightNormal
                       + timekeepingInfo.SumHourNightDayOff
                       + timekeepingInfo.SumHourNightSpecialDayOff);
        }
        public int CalculateTaxableOvertimeSalary(EMPLOYEE e, DateTime month)
        {
            return CalculateStandardHourSalary(e) * CalculateTotalWorkHour(e, month);
        }
        public int CalculateTaxableIncome(EMPLOYEE e, DateTime month)
        {
            return CalculateStandardSalary(e, month)
                 + CalculateTaxableOvertimeSalary(e, month)
                 - CalculateAllowanceFreeTaxValue();
        }      
        public int CalculateAssessableIncome(EMPLOYEE e, DateTime month)
        {
            int taxable = CalculateTaxableIncome(e, month);
            if (CheckLowerThan3Month(e.CONTRACT.DateStartWork, e.CONTRACT.ContractExpirationDate))
            {
                if (taxable < db.PARAMETERs.Find("ThuNhapChiuThueToiThieuHDDuoi3Thang").Value)
                    return 0;
                return taxable;
            }   
            int deduction = (int)db.PARAMETERs.Find("MucGiamTruNguoiPhuThuoc").Value * e.DependentDeduction + CalculateTotalInsurancePay(e, month);
            if (e.SelfDeduction == true)
                deduction += (int)db.PARAMETERs.Find("MucGiamTruBanThan").Value;
            if (taxable > deduction)
                return taxable - deduction;
            return 0;
        }
        public int CalculateIncomeTax(EMPLOYEE e, DateTime month)
        {
            int incomeTax = 0;
            int assessable = CalculateAssessableIncome(e, month);
            if (assessable == 0)
                return 0;
            if (CheckLowerThan3Month(e.CONTRACT.DateStartWork, e.CONTRACT.ContractExpirationDate))
                return (int)(assessable * db.PARAMETERs.Find("HSThueHDDuoi3Thang").Value / 100);
            int taxLevel = 1;
            var taxRateList = db.TAXRATEs.ToList();
            for (int i = taxRateList.Count - 1; i > 0; i--)
                if (assessable >= taxRateList[i].Min)
                {
                    taxLevel = taxRateList[i].Rank;
                    break;
                }
            for (int i = 0; i < taxLevel - 1; i++)
            {
                incomeTax += (taxRateList[i + 1].Min - taxRateList[i].Min) * taxRateList[i].Rate / 100;
            }
            incomeTax += (assessable - taxRateList[taxLevel - 1].Min) * taxRateList[taxLevel - 1].Rate / 100;
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
        
        public int CalculateSalary(EMPLOYEE e, DateTime month)
        {
            return CalculateWorkDaySalary(e, month) + CalculateOvertimeSalary(e, month) + CalculateAbsentSalary(e, month) - CalculateTotalInsurancePay(e, month) - CalculateIncomeTax(e, month) - CalculateAdvanced(e, month);
        }
        // GET: SALARYREPORTs
        public ActionResult Index()
        {
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            var employeelist = db.EMPLOYEEs.Where(x => x.State == true).ToList();
            foreach (EMPLOYEE e in employeelist)
            {
                if (db.SALARYREPORTs.Count(x => x.EmployeeID == e.EmployeeID && x.Month == date) == 0)
                {
                    int hourPay = CalculateStandardHourSalary(e);
                    var report = db.TIMEKEEPINGREPORTs.Find(date, e.EmployeeID);
                    var sALARYREPORT = new SALARYREPORT
                    {
                        EmployeeID = e.EmployeeID,
                        EMPLOYEE = e,
                        Month = date,
                        AbsentHaveSalary = db.TIMEKEEPINGREPORTs.Where(x => x.EmployeeID == e.EmployeeID && x.Month.Month == date.Month && x.Month.Year == date.Year).First().SumAbsentHaveSalary,
                        AbsentHaveSalaryValue = CalculateAbsentSalary(e, date),
                        NormalOverTimeSalary = CalculateNormalOvertimeSalary(report, hourPay),
                        DayOffOverTimeSalary = CalculateDayOffOvertimeSalary(report, hourPay),
                        SpecialDayOffOverTimeSalary = CalculateSpecialDayOffOvertimeSalary(report, hourPay),
                        NightNormalOverTimeSalary = CalculateNightNormalOvertimeSalary(report, hourPay),
                        NightDayOffOverTimeSalary = CalculateNightDayOffOvertimeSalary(report, hourPay),
                        NightSpecialDayOffOverTimeSalary = CalculateNightSpecialDayOffOvertimeSalary(report, hourPay),
                        OverTimeSalary = CalculateOvertimeSalary(e, date),
                        Allowance = CalculateTotalAllowance(e, date),
                        Advance = CalculateAdvanced(e, date),
                        IncomeTax = CalculateIncomeTax(e, date),
                        StandardSalary = CalculateStandardSalary(e, date),
                        WorkDay = db.TIMEKEEPINGREPORTs.Where(x => x.EmployeeID == e.EmployeeID && x.Month.Month == date.Month && x.Month.Year == date.Year).First().SumWorkDay,
                        WorkDaySalary = CalculateWorkDaySalary(e, date),
                        TotalInsurancePay = CalculateTotalInsurancePay(e, date),
                        RealSalary = CalculateSalary(e, date)
                    };
                    db.SALARYREPORTs.Add(sALARYREPORT);
                    db.SaveChanges();
                }
                else continue;
            }

            return View(db.SALARYREPORTs.ToList());
        }

        [HttpPost]
        public ActionResult Index(SALARYREPORT rp)
		{
            var sALARYREPORT = db.SALARYREPORTs.Where(x=>x.Month==rp.Month).ToList();
            return View(sALARYREPORT);
		}

        // GET: SALARYREPORTs/Details/5
        public ActionResult Details(string id, DateTime month)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SALARYREPORT sALARYREPORT = db.SALARYREPORTs.Find(month, id);
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
