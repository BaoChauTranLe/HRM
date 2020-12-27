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
    public class INSURANCEREPORTsController : Controller
    {
        private hrmserver_HRMEntities db = new hrmserver_HRMEntities();

        // GET: INSURANCEREPORTs
        public ActionResult Index()
        {
            var iNSURANCEREPORTs = db.INSURANCEREPORTs.Include(i => i.EMPLOYEE);
            return View(iNSURANCEREPORTs.ToList());
        }

        // GET: INSURANCEREPORTs/Details/5
        public ActionResult Details(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            INSURANCEREPORT iNSURANCEREPORT = db.INSURANCEREPORTs.Find(id);
            if (iNSURANCEREPORT == null)
            {
                return HttpNotFound();
            }
            return View(iNSURANCEREPORT);
        }

        // GET: INSURANCEREPORTs/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName");
            return View();
        }

        // POST: INSURANCEREPORTs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Month,EmployeeID,SalaryInsurance,UnionFee,SocialInsurance1,HealthInsurance1,UnemployeeInsurance1,InsuranceAndUnionFee,SocialInsurance2,HealthInsurance2,UnemployeeInsurance2,SubSalary")] INSURANCEREPORT iNSURANCEREPORT)
        {
            if (ModelState.IsValid)
            {
                db.INSURANCEREPORTs.Add(iNSURANCEREPORT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", iNSURANCEREPORT.EmployeeID);
            return View(iNSURANCEREPORT);
        }

        // GET: INSURANCEREPORTs/Edit/5
        public ActionResult Edit(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            INSURANCEREPORT iNSURANCEREPORT = db.INSURANCEREPORTs.Find(id);
            if (iNSURANCEREPORT == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", iNSURANCEREPORT.EmployeeID);
            return View(iNSURANCEREPORT);
        }

        // POST: INSURANCEREPORTs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Month,EmployeeID,SalaryInsurance,UnionFee,SocialInsurance1,HealthInsurance1,UnemployeeInsurance1,InsuranceAndUnionFee,SocialInsurance2,HealthInsurance2,UnemployeeInsurance2,SubSalary")] INSURANCEREPORT iNSURANCEREPORT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iNSURANCEREPORT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", iNSURANCEREPORT.EmployeeID);
            return View(iNSURANCEREPORT);
        }

        // GET: INSURANCEREPORTs/Delete/5
        public ActionResult Delete(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            INSURANCEREPORT iNSURANCEREPORT = db.INSURANCEREPORTs.Find(id);
            if (iNSURANCEREPORT == null)
            {
                return HttpNotFound();
            }
            return View(iNSURANCEREPORT);
        }

        // POST: INSURANCEREPORTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(DateTime id)
        {
            INSURANCEREPORT iNSURANCEREPORT = db.INSURANCEREPORTs.Find(id);
            db.INSURANCEREPORTs.Remove(iNSURANCEREPORT);
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
