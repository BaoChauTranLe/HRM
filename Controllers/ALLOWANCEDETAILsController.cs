﻿using System;
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
    public class ALLOWANCEDETAILsController : Controller
    {
        private hrmserver_HRMEntities db = new hrmserver_HRMEntities();

        // GET: ALLOWANCEDETAILs
        public ActionResult Index()
        {
            Session["MainTitle"] = "Quản lý tiền lương";
            Session["SubTitle"] = "Bảng chi tiết phụ cấp, trợ cấp, thưởng";
            var aLLOWANCEDETAILs = db.ALLOWANCEDETAILs.Include(a => a.ALLOWANCE).Include(a => a.EMPLOYEE);
            return View(aLLOWANCEDETAILs.ToList());
        }

        // GET: ALLOWANCEDETAILs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ALLOWANCEDETAIL aLLOWANCEDETAIL = db.ALLOWANCEDETAILs.Find(id);
            if (aLLOWANCEDETAIL == null)
            {
                return HttpNotFound();
            }
            return View(aLLOWANCEDETAIL);
        }
        public List<ALLOWANCE> GetAllowance()
        {
            List<ALLOWANCE> list = db.ALLOWANCEs.ToList();
            return list;
        }
        // GET: ALLOWANCEDETAILs/Create
        public ActionResult Create()
        {
            Session["MainTitle"] = "Quản lý tiền lương";
            Session["SubTitle"] = "Thêm chi tiết phụ cấp, trợ cấp, thưởng";
            ViewBag.AllowanceID = new SelectList(db.ALLOWANCEs.Where(i => i.AllEmployee == false).ToList(), "AllowanceID", "AllowanceName");
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName");
            return View();
        }

        // POST: ALLOWANCEDETAILs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AllowanceID,EmployeeID,Month,Value")] ALLOWANCEDETAIL aLLOWANCEDETAIL)
        {
            if (ModelState.IsValid)
            {
                db.ALLOWANCEDETAILs.Add(aLLOWANCEDETAIL);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AllowanceID = new SelectList(db.ALLOWANCEs.Where(i => i.AllEmployee == false).ToList(), "AllowanceID", "AllowanceName", aLLOWANCEDETAIL.AllowanceID);
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", aLLOWANCEDETAIL.EmployeeID);
            return View(aLLOWANCEDETAIL);
        }

        // GET: ALLOWANCEDETAILs/Edit/5
        public ActionResult Edit(string id, string employee,  DateTime month)
        {
            Session["MainTitle"] = "Quản lý tiền lương";
            Session["SubTitle"] = "Sửa chi tiết phụ cấp, trợ cấp, thưởng";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ALLOWANCEDETAIL aLLOWANCEDETAIL = db.ALLOWANCEDETAILs.Find(id, employee, month);
            if (aLLOWANCEDETAIL == null)
            {
                return HttpNotFound();
            }
            ViewBag.AllowanceID = new SelectList(db.ALLOWANCEs.Where(i => i.AllEmployee == false).ToList(), "AllowanceID", "AllowanceName", aLLOWANCEDETAIL.AllowanceID);
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", aLLOWANCEDETAIL.EmployeeID);
            return View(aLLOWANCEDETAIL);
        }

        // POST: ALLOWANCEDETAILs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AllowanceID,EmployeeID,Month,Value")] ALLOWANCEDETAIL aLLOWANCEDETAIL)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aLLOWANCEDETAIL).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AllowanceID = new SelectList(db.ALLOWANCEs.Where(i => i.AllEmployee == false).ToList(), "AllowanceID", "AllowanceName", aLLOWANCEDETAIL.AllowanceID);
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", aLLOWANCEDETAIL.EmployeeID);
            return View(aLLOWANCEDETAIL);
        }

        // GET: ALLOWANCEDETAILs/Delete/5
        public ActionResult Delete(string id, string employee, DateTime month)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ALLOWANCEDETAIL aLLOWANCEDETAIL = db.ALLOWANCEDETAILs.Find(id, employee, month);
            if (aLLOWANCEDETAIL == null)
            {
                return HttpNotFound();
            }
            return View(aLLOWANCEDETAIL);
        }

        // POST: ALLOWANCEDETAILs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string employee, DateTime month)
        {
            ALLOWANCEDETAIL aLLOWANCEDETAIL = db.ALLOWANCEDETAILs.Find(id, employee, month);
            db.ALLOWANCEDETAILs.Remove(aLLOWANCEDETAIL);
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
