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
    public class EMPLOYEEsController : Controller
    {
        private hrmserver_HRMEntities db = new hrmserver_HRMEntities();

        // GET: EMPLOYEEs
        public async Task<ActionResult> Index()
        {
            var eMPLOYEEs = db.EMPLOYEEs.Include(e => e.CONTRACT).Include(e => e.POSITION).Include(e => e.ROOM).Include(e => e.USER);
            return View(await eMPLOYEEs.ToListAsync());
        }

        // GET: EMPLOYEEs/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLOYEE eMPLOYEE = await db.EMPLOYEEs.FindAsync(id);
            if (eMPLOYEE == null)
            {
                return HttpNotFound();
            }
            return View(eMPLOYEE);
        }

        // GET: EMPLOYEEs/Create
        public ActionResult Create()
        {
            ViewBag.ContractID = new SelectList(db.CONTRACTs, "ContractID", "ContractType");
            ViewBag.PositionID = new SelectList(db.POSITIONs, "PositionID", "PositionName");
            ViewBag.RoomID = new SelectList(db.ROOMs, "RoomID", "RoomName");
            ViewBag.EmployeeID = new SelectList(db.USERS, "EmployeeID", "Password");
            return View();
        }

        // POST: EMPLOYEEs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EmployeeID,EmployeeName,Image,Sex,DoB,Birthplace,HomeTown,Nation,Id,Phone,Email,City,Ward,Dictrict,RoomID,PositionID,ContractID,HealthInsurance,HealthInsuranceID,SocialInsuranceID,DeductionPersonal,DeductionDependent")] EMPLOYEE eMPLOYEE)
        {
            if (ModelState.IsValid)
            {
                db.EMPLOYEEs.Add(eMPLOYEE);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ContractID = new SelectList(db.CONTRACTs, "ContractID", "ContractType", eMPLOYEE.ContractID);
            ViewBag.PositionID = new SelectList(db.POSITIONs, "PositionID", "PositionName", eMPLOYEE.PositionID);
            ViewBag.RoomID = new SelectList(db.ROOMs, "RoomID", "RoomName", eMPLOYEE.RoomID);
            ViewBag.EmployeeID = new SelectList(db.USERS, "EmployeeID", "Password", eMPLOYEE.EmployeeID);
            return View(eMPLOYEE);
        }

        // GET: EMPLOYEEs/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLOYEE eMPLOYEE = await db.EMPLOYEEs.FindAsync(id);
            if (eMPLOYEE == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContractID = new SelectList(db.CONTRACTs, "ContractID", "ContractType", eMPLOYEE.ContractID);
            ViewBag.PositionID = new SelectList(db.POSITIONs, "PositionID", "PositionName", eMPLOYEE.PositionID);
            ViewBag.RoomID = new SelectList(db.ROOMs, "RoomID", "RoomName", eMPLOYEE.RoomID);
            ViewBag.EmployeeID = new SelectList(db.USERS, "EmployeeID", "Password", eMPLOYEE.EmployeeID);
            return View(eMPLOYEE);
        }

        // POST: EMPLOYEEs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EmployeeID,EmployeeName,Image,Sex,DoB,Birthplace,HomeTown,Nation,Id,Phone,Email,City,Ward,Dictrict,RoomID,PositionID,ContractID,HealthInsurance,HealthInsuranceID,SocialInsuranceID,DeductionPersonal,DeductionDependent")] EMPLOYEE eMPLOYEE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eMPLOYEE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ContractID = new SelectList(db.CONTRACTs, "ContractID", "ContractType", eMPLOYEE.ContractID);
            ViewBag.PositionID = new SelectList(db.POSITIONs, "PositionID", "PositionName", eMPLOYEE.PositionID);
            ViewBag.RoomID = new SelectList(db.ROOMs, "RoomID", "RoomName", eMPLOYEE.RoomID);
            ViewBag.EmployeeID = new SelectList(db.USERS, "EmployeeID", "Password", eMPLOYEE.EmployeeID);
            return View(eMPLOYEE);
        }

        // GET: EMPLOYEEs/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLOYEE eMPLOYEE = await db.EMPLOYEEs.FindAsync(id);
            if (eMPLOYEE == null)
            {
                return HttpNotFound();
            }
            return View(eMPLOYEE);
        }

        // POST: EMPLOYEEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            EMPLOYEE eMPLOYEE = await db.EMPLOYEEs.FindAsync(id);
            db.EMPLOYEEs.Remove(eMPLOYEE);
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
