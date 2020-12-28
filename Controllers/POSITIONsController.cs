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
    public class POSITIONsController : Controller
    {
        private hrmserver_HRMEntities db = new hrmserver_HRMEntities();

        // GET: POSITIONs
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetPositionList()
        {
            try
            {
                List<POSITION> list = db.POSITIONs.ToList();
                var PositionList = from p in list
                               select new { p.PositionID, p.PositionName };
                var result = new { list = PositionList, str = "success" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var result = new { str = "fail" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: POSITIONs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrEdit([Bind(Include = "PositionID,PositionName,RoomID")] POSITION pOSITION)
        {
            if (ModelState.IsValid)
            {
                if (pOSITION.PositionID == null)
                {
                    //auto create allowanceID
                    int n = 0;
                    var positionList = db.POSITIONs.ToList();
                    if (positionList.Count > 0)
                    {
                        POSITION r = positionList.Last();
                        n = Int32.Parse(r.PositionID.Substring(r.PositionID.Length - 2)) + 1;
                    }

                    string id = String.Format("{0:00}", n);
                    pOSITION.PositionID = "P" + id;

                    db.POSITIONs.Add(pOSITION);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    db.Entry(pOSITION).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(pOSITION);
        }

        // POST: POSITIONs/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                POSITION pOSITION = db.POSITIONs.Find(id);
                db.POSITIONs.Remove(pOSITION);
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
