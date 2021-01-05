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
            //var aDVANCEDs = db.ADVANCEDs.Include(a => a.EMPLOYEE);
            return View();
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

        // POST: ADVANCEDs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrEdit([Bind(Include = "EmployeeID,DateAdvanced,Value")] ADVANCED aDVANCED)
        {
            if (ModelState.IsValid)
            {
                DateTime minDate = new DateTime(1, 1, 1, 0, 0, 0);
                if(aDVANCED.DateAdvanced == minDate)
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

        // POST: ADVANCEDs/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string employee)
        {
            try
            {
                DateTime d = DateTime.Parse(id);
                ADVANCED aDVANCED = db.ADVANCEDs.Find(employee, d);
                db.ADVANCEDs.Remove(aDVANCED);
                db.SaveChanges();
            }
            catch
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
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
