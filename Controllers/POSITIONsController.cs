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
            var pOSITIONs = db.POSITIONs;
            return View(pOSITIONs.ToList());
        }

        // GET: POSITIONs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            POSITION pOSITION = db.POSITIONs.Find(id);
            if (pOSITION == null)
            {
                return HttpNotFound();
            }
            return View(pOSITION);
        }

        // GET: POSITIONs/Create
        public ActionResult Create()
        {
            ViewBag.RoomID = new SelectList(db.ROOMs, "RoomID", "RoomName");
            return View();
        }

        // POST: POSITIONs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PositionID,PositionName,RoomID")] POSITION pOSITION)
        {
            if (ModelState.IsValid)
            {
                db.POSITIONs.Add(pOSITION);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoomID = new SelectList(db.ROOMs, "RoomID", "RoomName");
            return View(pOSITION);
        }

        // GET: POSITIONs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            POSITION pOSITION = db.POSITIONs.Find(id);
            if (pOSITION == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomID = new SelectList(db.ROOMs, "RoomID", "RoomName");
            return View(pOSITION);
        }

        // POST: POSITIONs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PositionID,PositionName,RoomID")] POSITION pOSITION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pOSITION).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomID = new SelectList(db.ROOMs, "RoomID", "RoomName");
            return View(pOSITION);
        }

        // GET: POSITIONs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            POSITION pOSITION = db.POSITIONs.Find(id);
            if (pOSITION == null)
            {
                return HttpNotFound();
            }
            return View(pOSITION);
        }

        // POST: POSITIONs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            POSITION pOSITION = db.POSITIONs.Find(id);
            db.POSITIONs.Remove(pOSITION);
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
