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
    public class TAXRATEsController : Controller
    {
        private hrmserver_HRMEntities db = new hrmserver_HRMEntities();

        // GET: TAXRATEs
        public ActionResult Index()
        {
            return View(db.TAXRATEs.ToList());
        }

        // GET: TAXRATEs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAXRATE tAXRATE = db.TAXRATEs.Find(id);
            if (tAXRATE == null)
            {
                return HttpNotFound();
            }
            return View(tAXRATE);
        }

        // GET: TAXRATEs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TAXRATEs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Rank,Min,Max,Rate")] TAXRATE tAXRATE)
        {
            tAXRATE.Rank = RankGenerator();
            tAXRATE.Min = MinGenerator();
            if (tAXRATE.Max != null && tAXRATE.Max <= tAXRATE.Min)
                ModelState.AddModelError("Max", "Giá trị được nhập phải lớn hơn giá trị phía trước");
            if (ModelState.IsValid)
            {
                db.TAXRATEs.Add(tAXRATE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tAXRATE);
        }
        
        //Auto generate TAXRATE rank
        public int RankGenerator()
        {
            int taxRank = 1;
            var taxRateList = db.TAXRATEs.ToList();
            if (taxRateList.Count > 0)
                taxRank = taxRateList.Last().Rank + 1;
            return taxRank;
        }

        public ActionResult BackToIndex()
        {
            return RedirectToAction("Index");
        }

        public double? MinGenerator()
        {
            double? min = 0;
            var taxRateList = db.TAXRATEs.ToList();
            if (taxRateList.Count > 0)
                min = taxRateList.Last().Max;
            return min;
        }

        // GET: TAXRATEs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAXRATE tAXRATE = db.TAXRATEs.Find(id);
            if (tAXRATE == null)
            {
                return HttpNotFound();
            }
            return View(tAXRATE);
        }

        // POST: TAXRATEs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Rank,Min,Max,Rate")] TAXRATE tAXRATE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tAXRATE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tAXRATE);
        }

        // GET: TAXRATEs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAXRATE tAXRATE = db.TAXRATEs.Find(id);
            if (tAXRATE == null)
            {
                return HttpNotFound();
            }
            return View(tAXRATE);
        }

        // POST: TAXRATEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TAXRATE tAXRATE = db.TAXRATEs.Find(id);
            db.TAXRATEs.Remove(tAXRATE);
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
