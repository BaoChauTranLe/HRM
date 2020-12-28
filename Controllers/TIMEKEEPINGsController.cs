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
    public class TIMEKEEPINGsController : Controller
    {
        private hrmserver_HRMEntities db = new hrmserver_HRMEntities();

        // GET: TIMEKEEPINGs
        public ActionResult Index()
        {
            var tIMEKEEPINGs = db.TIMEKEEPINGs.Include(t => t.DATEINFORMATION).Include(t => t.EMPLOYEE);
            return View(tIMEKEEPINGs.ToList());
        }
        private List<EMPLOYEE> GetEmployee()
        {
            List<EMPLOYEE> list = db.EMPLOYEEs.ToList();
            return list;
        }

        public List<SHIFT> GetShift()
        {
            List<SHIFT> list = db.SHIFTs.ToList();
            return list;
        }

        public List<ROOM> GetRoom()
        {
            List<ROOM> list = db.ROOMs.ToList();
            return list;
        }
        public List<POSITION> GetPosition()
        {
            List<POSITION> list = db.POSITIONs.ToList();
            return list;
        }
        
        // GET: TIMEKEEPINGs/Create
        public ActionResult Create()
        {
            SetShiftViewModel mymodel = new SetShiftViewModel();
            mymodel.eMPLOYEEs = GetEmployee();
            mymodel.sHIFTs = GetShift();

            List<EMPLOYEE> a = db.EMPLOYEEs.ToList();

            ViewBag.Room = GetRoom();
            ViewBag.Position = GetPosition();
            //ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName");
            return View(mymodel);
        }

        // POST: TIMEKEEPINGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SetShiftViewModel vmTK)
        {
           
            if (ModelState.IsValid)
            {
                db.TIMEKEEPINGs.Add(vmTK.tIMEKEEPING);
               
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.Date = new SelectList(db.DATEINFORMATIONs, "Date", "Date", tIMEKEEPING.Date);
            //ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", tIMEKEEPING.EmployeeID);
            return View(vmTK);
        }

        // GET: TIMEKEEPINGs/Edit/5
        public ActionResult Edit(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIMEKEEPING tIMEKEEPING = db.TIMEKEEPINGs.Find(id);
            if (tIMEKEEPING == null)
            {
                return HttpNotFound();
            }
            ViewBag.Date = new SelectList(db.DATEINFORMATIONs, "Date", "Date", tIMEKEEPING.Date);
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", tIMEKEEPING.EmployeeID);
            return View(tIMEKEEPING);
        }

        // POST: TIMEKEEPINGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Date,EmployeeID,State,AbsentID,HoursWorkDay,HoursWorkNight")] TIMEKEEPING tIMEKEEPING)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tIMEKEEPING).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Date = new SelectList(db.DATEINFORMATIONs, "Date", "Date", tIMEKEEPING.Date);
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", tIMEKEEPING.EmployeeID);
            return View(tIMEKEEPING);
        }

        // GET: TIMEKEEPINGs/Delete/5
        public ActionResult Delete(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIMEKEEPING tIMEKEEPING = db.TIMEKEEPINGs.Find(id);
            if (tIMEKEEPING == null)
            {
                return HttpNotFound();
            }
            return View(tIMEKEEPING);
        }

        // POST: TIMEKEEPINGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(DateTime id)
        {
            TIMEKEEPING tIMEKEEPING = db.TIMEKEEPINGs.Find(id);
            db.TIMEKEEPINGs.Remove(tIMEKEEPING);
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
