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
    public class TAXREPORTsController : Controller
    {
        private hrmserver_HRMEntities db = new hrmserver_HRMEntities();

        // GET: TAXREPORTs
        public ActionResult Index()
        {
            var tAXREPORTs = db.TAXREPORTs.Include(t => t.EMPLOYEE);
            return View(tAXREPORTs.ToList());
        }

        // GET: TAXREPORTs/Details/5
        public ActionResult Details(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAXREPORT tAXREPORT = db.TAXREPORTs.Find(id);
            if (tAXREPORT == null)
            {
                return HttpNotFound();
            }
            return View(tAXREPORT);
        }

        // GET: TAXREPORTs/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName");
            return View();
        }

        // POST: TAXREPORTs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Month,EmployeeID,TotalIncome,FreeTax,Deduction,OverTimeHour,OverTimeFreeTax,TaxableIncome,IncomeTax")] TAXREPORT tAXREPORT)
        {
            if (ModelState.IsValid)
            {
                db.TAXREPORTs.Add(tAXREPORT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", tAXREPORT.EmployeeID);
            return View(tAXREPORT);
        }

        // GET: TAXREPORTs/Edit/5
        public ActionResult Edit(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAXREPORT tAXREPORT = db.TAXREPORTs.Find(id);
            if (tAXREPORT == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", tAXREPORT.EmployeeID);
            return View(tAXREPORT);
        }

        // POST: TAXREPORTs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Month,EmployeeID,TotalIncome,FreeTax,Deduction,OverTimeHour,OverTimeFreeTax,TaxableIncome,IncomeTax")] TAXREPORT tAXREPORT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tAXREPORT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", tAXREPORT.EmployeeID);
            return View(tAXREPORT);
        }

        // GET: TAXREPORTs/Delete/5
        public ActionResult Delete(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAXREPORT tAXREPORT = db.TAXREPORTs.Find(id);
            if (tAXREPORT == null)
            {
                return HttpNotFound();
            }
            return View(tAXREPORT);
        }

        // POST: TAXREPORTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(DateTime id)
        {
            TAXREPORT tAXREPORT = db.TAXREPORTs.Find(id);
            db.TAXREPORTs.Remove(tAXREPORT);
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
