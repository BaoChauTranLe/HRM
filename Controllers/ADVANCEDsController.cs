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
                List<ADVANCED> list = db.ADVANCEDs.Include(a => a.EMPLOYEE).ToList();
                var AdvancedList = from a in list
                               select new { a.EmployeeID, a.DateAdvanced, a.Value };
                var result = new { list = AdvancedList, str = "success" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var result = new { str = "fail" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: ADVANCEDs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ADVANCED aDVANCED = db.ADVANCEDs.Find(id);
            if (aDVANCED == null)
            {
                return HttpNotFound();
            }
            return View(aDVANCED);
        }

        // GET: ADVANCEDs/Create
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
                db.ADVANCEDs.Add(aDVANCED);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", aDVANCED.EmployeeID);
            return View(aDVANCED);
        }

        // GET: ADVANCEDs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ADVANCED aDVANCED = db.ADVANCEDs.Find(id);
            if (aDVANCED == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", aDVANCED.EmployeeID);
            return View(aDVANCED);
        }

        // POST: ADVANCEDs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: ADVANCEDs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ADVANCED aDVANCED = db.ADVANCEDs.Find(id);
            if (aDVANCED == null)
            {
                return HttpNotFound();
            }
            return View(aDVANCED);
        }

        // POST: ADVANCEDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ADVANCED aDVANCED = db.ADVANCEDs.Find(id);
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
