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
    public class ADVANCEDsController : Controller
    {
        private hrmserver_HRMEntities db = new hrmserver_HRMEntities();

        // GET: ADVANCEDs
        public ActionResult Index()
        {
            var aDVANCEDs = db.ADVANCEDs.Include(a => a.EMPLOYEE);
            return View(aDVANCEDs.ToList());
        }

        public JsonResult GetAdvancedList()
        {
            try
            {
                List<ADVANCED> lista = db.ADVANCEDs.Include(a => a.EMPLOYEE).ToList();
                var AdvancedList = from a in lista
                               select new { a.EmployeeID, a.DateAdvanced, a.Value };
                List<EMPLOYEE> liste = db.EMPLOYEEs.ToList();
                var EmployeeList = from e in liste
                                   select new { e.EmployeeID, e.EmployeeName };
                var result = new { lista = AdvancedList, liste = EmployeeList, str = "success" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var result = new { str = "fail" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Create()
        {
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName");
            return View();
        }
        // POST: ADVANCEDs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,DateAdvanced,Value")] ADVANCED aDVANCED)
        {
            if (ModelState.IsValid)
			{
				DateTime minDate = new DateTime(1, 1, 1, 0, 0, 0);
				if (aDVANCED.DateAdvanced == minDate)
				{
					DateTime now = DateTime.Now;
					aDVANCED.DateAdvanced = new DateTime(now.Year, now.Month, now.Day);
					db.ADVANCEDs.Add(aDVANCED);
					db.SaveChanges();
					return RedirectToAction("Index");
				}
				else
				{
					db.Entry(aDVANCED).State = EntityState.Modified;
					db.SaveChanges();
					return RedirectToAction("Index");
				}
			}
			ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", aDVANCED.EmployeeID);
			return View(aDVANCED);
		}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateOrEdit([Bind(Include = "EmployeeID,DateAdvanced,Value")] ADVANCED aDVANCED)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        DateTime minDate = new DateTime(1, 1, 1, 0, 0, 0);
        //        if(aDVANCED.DateAdvanced == minDate)
        //        {
        //            DateTime now = DateTime.Now;
        //            aDVANCED.DateAdvanced = new DateTime(now.Year, now.Month, now.Day);
        //            db.ADVANCEDs.Add(aDVANCED);
        //            db.SaveChanges();
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            db.Entry(aDVANCED).State = EntityState.Modified;
        //            db.SaveChanges();
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", aDVANCED.EmployeeID);
        //    return View(aDVANCED);
        //}
        public ActionResult Edit(string id, DateTime date)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ADVANCED aDVANCED = db.ADVANCEDs.Find(id, date);
            if (aDVANCED == null)
            {
                return HttpNotFound();
            }
            //ViewBag.AllowanceID = new SelectList(db.ALLOWANCEs, "AllowanceID", "AllowanceName", aLLOWANCEDETAIL.AllowanceID);
            //ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", aLLOWANCEDETAIL.EmployeeID);
            return View(aDVANCED);
        }
        [HttpPost, ActionName("Edit")]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,DateAdvanced,Value")] ADVANCED aDVANCED)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aDVANCED).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", aDVANCED.EmployeeID);
            return View(aDVANCED);
        }
        // GET: ALLOWANCEDETAILs/Delete/5
        public ActionResult Delete(string id, DateTime date)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ADVANCED aDVANCED = db.ADVANCEDs.Find(id, date);
            if (aDVANCED == null)
            {
                return HttpNotFound();
            }
            return View(aDVANCED);
        }
        // POST: ADVANCEDs/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, DateTime date)
        {
            
                ADVANCED aDVANCED = db.ADVANCEDs.Find(id, date);
                db.ADVANCEDs.Remove(aDVANCED);
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
