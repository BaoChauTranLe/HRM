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
    public class BONUSsController : Controller
    {
        private hrmserver_HRMEntities db = new hrmserver_HRMEntities();

        // GET: BONUSs
        public ActionResult Index()
        {
            return View(db.BONUS.ToList());
        }

        // GET: BONUSs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BONU bONU = db.BONUS.Find(id);
            if (bONU == null)
            {
                return HttpNotFound();
            }
            return View(bONU);
        }

        // GET: BONUSs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BONUSs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BonusID,BonusName")] BONU bONU)
        {
            if (ModelState.IsValid)
            {
                db.BONUS.Add(bONU);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bONU);
        }

        // GET: BONUSs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BONU bONU = db.BONUS.Find(id);
            if (bONU == null)
            {
                return HttpNotFound();
            }
            return View(bONU);
        }

        // POST: BONUSs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BonusID,BonusName")] BONU bONU)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bONU).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bONU);
        }

        // GET: BONUSs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BONU bONU = db.BONUS.Find(id);
            if (bONU == null)
            {
                return HttpNotFound();
            }
            return View(bONU);
        }

        // POST: BONUSs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            BONU bONU = db.BONUS.Find(id);
            db.BONUS.Remove(bONU);
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
