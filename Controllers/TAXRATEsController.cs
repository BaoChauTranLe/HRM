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
        int? selectedRank;
        public int? getselectedRank()
        {
            return selectedRank;
        }
        public void setselectedRank(int? rank)
        {
            selectedRank = rank;
        }
        private hrmserver_HRMEntities db = new hrmserver_HRMEntities();

        // GET: TAXRATEs
        public ActionResult Index()
        {
            return View(db.TAXRATEs.ToList());
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
            {
                taxRank = taxRateList.Last().Rank + 1;
                //if (taxRateList.Last().Max != null)
                    //taxRank = taxRateList.Last().Rank + 1;
                //else
                    //taxRank = taxRateList.Last().Rank;
            }
            return taxRank;
        }

        public double? MinGenerator()
        {
            double? min = 0;
            var taxRateList = db.TAXRATEs.ToList();
            if (taxRateList.Count > 0)
            {
                min = taxRateList.Last().Max;
                //double? max = taxRateList.Last().Max;
                //if (max == null)
                //min = taxRateList.Last().Min;
                //else
                //min = max;
            }    
            return min;
        }
        public ActionResult BackToIndex()
        {
            return RedirectToAction("Index");
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
