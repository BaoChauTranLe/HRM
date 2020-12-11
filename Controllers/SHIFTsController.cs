using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HRM.Models;

namespace HRM.Controllers
{
    public class SHIFTsController : Controller
    {
        private hrmserver_HRMEntities1 db = new hrmserver_HRMEntities1();

        // GET: SHIFTs
        public async Task<ActionResult> Index()
        {
            var sHIFTs = db.SHIFTs.Include(s => s.EMPLOYEE);
            return View(await sHIFTs.ToListAsync());
        }

        // GET: SHIFTs/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SHIFT sHIFT = await db.SHIFTs.FindAsync(id);
            if (sHIFT == null)
            {
                return HttpNotFound();
            }
            return View(sHIFT);
        }

        // GET: SHIFTs/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName");
            return View();
        }

        // POST: SHIFTs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ShiftID,EmployeeID,ShiftName,ShiftType,StartTime,EndTime")] SHIFT sHIFT)
        {
            if (ModelState.IsValid)
            {
                db.SHIFTs.Add(sHIFT);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", sHIFT.EmployeeID);
            return View(sHIFT);
        }

        // GET: SHIFTs/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SHIFT sHIFT = await db.SHIFTs.FindAsync(id);
            if (sHIFT == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", sHIFT.EmployeeID);
            return View(sHIFT);
        }

        // POST: SHIFTs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ShiftID,EmployeeID,ShiftName,ShiftType,StartTime,EndTime")] SHIFT sHIFT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sHIFT).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", sHIFT.EmployeeID);
            return View(sHIFT);
        }

        // GET: SHIFTs/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SHIFT sHIFT = await db.SHIFTs.FindAsync(id);
            if (sHIFT == null)
            {
                return HttpNotFound();
            }
            return View(sHIFT);
        }

        // POST: SHIFTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            SHIFT sHIFT = await db.SHIFTs.FindAsync(id);
            db.SHIFTs.Remove(sHIFT);
            await db.SaveChangesAsync();
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
