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
        public int CalculateInsurancePay(EMPLOYEE e, int insurancePay)
        {
            if (e.HealthInsurance == false)
            {
                return (int)(insurancePay * db.PARAMETERs.Find("BHXH").Value + insurancePay
                                          * db.PARAMETERs.Find("BHYT").Value + insurancePay
                                          * db.PARAMETERs.Find("BHTN").Value);
            }
            return 0;
        }
        public int CalculateIncomeTax(EMPLOYEE e, int taxPay)
        {
            if (taxPay <= 0)
                return 0;
            int incomeTax = 0;
            int taxLevel = 1;
            var taxRateList = db.TAXRATEs.ToList();
            for (int i = taxRateList.Last().Rank; i > 1; i--)
                if (taxPay >= taxRateList[i].Min)
                {
                    taxLevel = i;
                    break;
                }
            for (int i = 1; i < taxLevel; i++)
            {
                incomeTax += (taxRateList[i + 1].Min - taxRateList[i].Min) * taxRateList[i].Rate / 100;
            }
            incomeTax += (taxPay - taxRateList[taxLevel].Min) * taxRateList[taxLevel].Rate / 100;
            return incomeTax;
        }
        public int CalculateTaxableIncome(EMPLOYEE e, int income, int insurancePay)
        {
            int taxPay = 0;
            if (income >= (int)db.PARAMETERs.Find("MucThuNhapToiThieuPhaiDongThue").Value)
            {
                taxPay = income - insurancePay;
                var allowanceList = db.ALLOWANCEs.ToList();
                var freeTaxList = allowanceList.Where(x => x.Tax == true).ToList();
                for (int i = 0; i < freeTaxList.Count; i++)
                    taxPay -= (int)freeTaxList[i].FreeTax;
                if (e.DeductionPersonal == true)
                    taxPay -= (int)db.PARAMETERs.Find("MucGiamTruBanThan").Value;
                if (e.DeductionDependent > 0)
                    taxPay -= (int)(db.PARAMETERs.Find("MucGiamTruNguoiPhuThuoc").Value * e.DeductionDependent);
            }
            return taxPay;
        }
        public int CalculateAdvanced(EMPLOYEE e)
        {
            int advanced = 0;
            var advancedList = db.ADVANCEDs.Where(x => x.EmployeeID == e.EmployeeID).ToList();
            for (int i = 0; i < advancedList.Count; i++)
                advanced += (int)advancedList[i].Value;
            return advanced;
        }
        public int CalculateSalary(string id)
        {
            EMPLOYEE e = db.EMPLOYEEs.Find(id);
            int income = (int)e.CONTRACT.BasicSalary;

            int insurance_pay = income;
            var allowanceList = db.ALLOWANCEs.ToList();
            for (int i = 0; i < allowanceList.Count; i++)
            {
                var val = allowanceList[i];
                if (val.Insurance)
                    insurance_pay += (int)val.Value; //Insurance pay only counted for some specific Allowances
                income += (int)val.Value; //Standard income = BasicSalary + all kind of Allowances
            }
            insurance_pay = CalculateInsurancePay(e, (int)insurance_pay);
            int incomeTax = CalculateTaxableIncome(e, income, insurance_pay);

            income = (int)(income * db.TIMEKEEPINGREPORTs.Find(id).SumWorkDay / db.PARAMETERs.Find("SoNgayCongChuan").Value);
            income = income - insurance_pay - incomeTax;
            income = income - CalculateAdvanced(e);
            
            return 0;
        }
        // GET: SALARYREPORTs
        public ActionResult Index()
        {
            var sALARYREPORTs = db.SALARYREPORTs.Include(s => s.EMPLOYEE);
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
