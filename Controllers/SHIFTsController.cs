using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HRM.Models;
using HRM.Util;

namespace HRM.Controllers
{
    public class SHIFTsController : Controller
    {
        private hrmserver_HRMEntities db = new hrmserver_HRMEntities();

        // GET: SHIFTs
        public ActionResult Index()
        {
            //db.SHIFTs.ToList();
            ViewBag.ListOfShiftType = SelectListItemHelper.GetShiftTypeList();
            return View();
        }

        public JsonResult GetShiftList()
        {
            List<SHIFT> ShiftList = db.SHIFTs.ToList();
            return Json(ShiftList, JsonRequestBehavior.AllowGet);
        }

        // GET: SHIFTs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SHIFT sHIFT = db.SHIFTs.Find(id);
            if (sHIFT == null)
            {
                return HttpNotFound();
            }
            return View(sHIFT);
        }

        // GET: SHIFTs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SHIFTs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "ShiftID,ShiftName,ShiftType,StartTime,EndTime,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday")] SHIFT sHIFT)
        {
            //sHIFT.ShiftID = auto create shiftID
            if (ModelState.IsValid)
            {
                db.SHIFTs.Add(sHIFT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sHIFT);
        }

        // GET: SHIFTs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SHIFT sHIFT = db.SHIFTs.Find(id);
            if (sHIFT == null)
            {
                return HttpNotFound();
            }
            return View(sHIFT);
        }

        // POST: SHIFTs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShiftID,ShiftName,ShiftType,StartTime,EndTime,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday")] SHIFT sHIFT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sHIFT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sHIFT);
        }

        // GET: SHIFTs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SHIFT sHIFT = db.SHIFTs.Find(id);
            if (sHIFT == null)
            {
                return HttpNotFound();
            }
            return View(sHIFT);
        }

        // POST: SHIFTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SHIFT sHIFT = db.SHIFTs.Find(id);
            db.SHIFTs.Remove(sHIFT);
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
