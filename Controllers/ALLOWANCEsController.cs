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

        // GET: ALLOWANCEs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ALLOWANCEs/Index
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "AllowanceID,AllowanceName,Insurance,Tax,FreeTax")] ALLOWANCE aLLOWANCE)
        {
            if (ModelState.IsValid)
            {
                if (aLLOWANCE.AllowanceID == null)
                {
                    //auto create allowanceID
                    int n = 0;
                    var allowanceList = db.ALLOWANCEs.ToList();
                    if (allowanceList.Count > 0)
                    {
                        ALLOWANCE s = allowanceList.Last();
                        n = Int32.Parse(s.AllowanceID.Substring(s.AllowanceID.Length - 3)) + 1;
                    }

                    string id = String.Format("{0:000}", n);
                    aLLOWANCE.AllowanceID = "A" + id;

                    db.ALLOWANCEs.Add(aLLOWANCE);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    db.Entry(aLLOWANCE).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(aLLOWANCE);
        }

        // POST: ALLOWANCEs/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                ALLOWANCE aLLOWANCE = db.ALLOWANCEs.Find(id);
                db.ALLOWANCEs.Remove(aLLOWANCE);
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
