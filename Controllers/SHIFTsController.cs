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
            Session["MainTitle"] = "Quản lý chấm công";
            Session["SubTitle"] = "Danh sách ca làm việc";
            //db.SHIFTs.ToList();
            ViewBag.ListOfShiftType = SelectListItemHelper.GetShiftTypeList();
            return View();
        }

        public JsonResult GetShiftList()
        {
            try
            {
                List<SHIFT> list = db.SHIFTs.ToList();
                var ShiftList = from s in list
                               select new { s.ShiftID, s.ShiftName, s.ShiftType, s.StartTime, s.EndTime, s.Monday, s.Tuesday, s.Wednesday, s.Thursday, s.Friday, s.Saturday, s.Sunday };
                var result = new { list = ShiftList, str = "success" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var result = new { str = "fail" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        // Process Create and Edit Shifts
        // POST: SHIFTs/Index
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrEdit([Bind(Include = "ShiftID,ShiftName,ShiftType,StartTime,EndTime,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday")] SHIFT sHIFT)
        {
            if (ModelState.IsValid)
            {
                if (sHIFT.ShiftID == null)
                {
                    //auto create shiftID
                    int n = 0;
                    var shiftList = db.SHIFTs.ToList();
                    if (shiftList.Count > 0)
                    {
                        SHIFT s = shiftList.Last();
                        n = Int32.Parse(s.ShiftID.Substring(s.ShiftID.Length - 3)) + 1;
                    }

                    string id = String.Format("{0:000}", n);
                    sHIFT.ShiftID = "S" + id;

                    db.SHIFTs.Add(sHIFT);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    db.Entry(sHIFT).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(sHIFT);
        }

        // POST: SHIFTs/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                SHIFT sHIFT = db.SHIFTs.Find(id);
                db.SHIFTs.Remove(sHIFT);
                db.SaveChanges();
            }
            catch {
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
