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
    public class ALLOWANCEsController : Controller
    {
        private hrmserver_HRMEntities db = new hrmserver_HRMEntities();

        // GET: ALLOWANCEs
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllowanceList()
        {
            try
            {
                List<ALLOWANCE> list = db.ALLOWANCEs.ToList();
                var AllowanceList = from a in list
                                select new { a.AllowanceID, a.AllowanceName, a.Insurance, a.Tax, a.FreeTax};
                var result = new { list = AllowanceList, str = "success" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var result = new { str = "fail" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: ALLOWANCEs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ALLOWANCE aLLOWANCE = db.ALLOWANCEs.Find(id);
            if (aLLOWANCE == null)
            {
                return HttpNotFound();
            }
            return View(aLLOWANCE);
        }

        // GET: ALLOWANCEs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ALLOWANCEs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AllowanceID,AllowanceName,Insurance,Tax,FreeTax")] ALLOWANCE aLLOWANCE)
        {
            if (ModelState.IsValid)
            {
                db.ALLOWANCEs.Add(aLLOWANCE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aLLOWANCE);
        }

        // GET: ALLOWANCEs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ALLOWANCE aLLOWANCE = db.ALLOWANCEs.Find(id);
            if (aLLOWANCE == null)
            {
                return HttpNotFound();
            }
            return View(aLLOWANCE);
        }

        // POST: ALLOWANCEs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AllowanceID,AllowanceName,Insurance,Tax,FreeTax")] ALLOWANCE aLLOWANCE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aLLOWANCE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aLLOWANCE);
        }

        // GET: ALLOWANCEs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ALLOWANCE aLLOWANCE = db.ALLOWANCEs.Find(id);
            if (aLLOWANCE == null)
            {
                return HttpNotFound();
            }
            return View(aLLOWANCE);
        }

        // POST: ALLOWANCEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ALLOWANCE aLLOWANCE = db.ALLOWANCEs.Find(id);
            db.ALLOWANCEs.Remove(aLLOWANCE);
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
