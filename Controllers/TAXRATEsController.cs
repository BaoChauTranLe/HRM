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
        public ActionResult ToCreate()
        {
            return RedirectToAction("Create");
        }

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
        public ActionResult Create([Bind(Include = "Rank,Min,Rate")] TAXRATE tAXRATE)
        {
            var taxRateList = db.TAXRATEs.ToList();
            TAXRATE lastElement = taxRateList.Last();
            /* Rank Generator */
            if (taxRateList.Count > 0)
            {
                if (tAXRATE.Min <= lastElement.Min)
                    ModelState.AddModelError("Min", "Phần thu nhập tính thuế / tháng của bậc thuế này phải lớn hơn các giá trị đã có");
                if (tAXRATE.Rate <= lastElement.Rate)
                    ModelState.AddModelError("Rate", "Thuế suất của bậc thuế này phải lớn hơn các giá trị đã có");
                tAXRATE.Rank = lastElement.Rank + 1;
            }
            else
                tAXRATE.Rank = 1;

            if (ModelState.IsValid)
            {
                db.TAXRATEs.Add(tAXRATE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tAXRATE);
        }

        public ActionResult BackToIndex()
        {
            return RedirectToAction("Index");
        }

        // GET: BONUSs/Edit/5
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

        // POST: BONUSs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Rank,Min,Rate")] TAXRATE tAXRATE)
        {
            TAXRATE prevElement = db.TAXRATEs.Find(tAXRATE.Rank - 1);
            TAXRATE nextElement = db.TAXRATEs.Find(tAXRATE.Rank + 1);
            /* Validation */
            if (prevElement == null && nextElement != null)
            {
                if (tAXRATE.Min >= nextElement.Min)
                    ModelState.AddModelError("Min", "Phần thu nhập tính thuế / tháng của bậc thuế này phải nhỏ hơn giá trị của bậc thuế phía sau");
                if (tAXRATE.Rate >= nextElement.Rate)
                    ModelState.AddModelError("Rate", "Thuế suất của bậc thuế này phải nhỏ hơn giá trị của bậc thuế phía sau");
            }
            if (prevElement != null && nextElement == null)
            {
                if (tAXRATE.Min <= prevElement.Min)
                    ModelState.AddModelError("Min", "Phần thu nhập tính thuế / tháng của bậc thuế này phải lớn hơn giá trị của bậc thuế phía trước");
                if (tAXRATE.Rate <= prevElement.Rate)
                    ModelState.AddModelError("Rate", "Thuế suất của bậc thuế này phải lớn hơn giá trị của bậc thuế phía trước");
            }
            if (prevElement != null && nextElement != null)
            {
                if (tAXRATE.Min <= prevElement.Min || tAXRATE.Min >= nextElement.Min)
                    ModelState.AddModelError("Min", "Phần thu nhập tính thuế / tháng của bậc thuế này phải lớn hơn giá trị của bậc thuế phía trước và nhỏ hơn giá trị của bậc thuế phía sau");
                if (tAXRATE.Rate <= prevElement.Rate || tAXRATE.Rate >= nextElement.Rate)
                    ModelState.AddModelError("Rate", "Thuế suất của bậc thuế này phải lớn hơn giá trị của bậc thuế phía trước và nhỏ hơn giá trị của bậc thuế phía sau");
            }    
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
