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
    public class ROOMsController : Controller
    {
        private hrmserver_HRMEntities db = new hrmserver_HRMEntities();

        // GET: ROOMs
        public ActionResult Index()
        {
            Session["MainTitle"] = "Thiết lập quy định";
            Session["SubTitle"] = "Phòng ban";
            return View();
        }

        public JsonResult GetRoomList()
        {
            try
            {
                List<ROOM> list = db.ROOMs.ToList();
                var RoomList = from r in list
                                    select new { r.RoomID, r.RoomName};
                var result = new { list = RoomList, str = "success" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var result = new { str = "fail" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: ROOMs/CreateOrEdit
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrEdit([Bind(Include = "RoomID,RoomName")] ROOM rOOM)
        {
            if (ModelState.IsValid)
            {
                if (rOOM.RoomID == null)
                {
                    //auto create allowanceID
                    int n = 0;
                    var roomList = db.ROOMs.ToList();
                    if (roomList.Count > 0)
                    {
                        ROOM r = roomList.Last();
                        n = Int32.Parse(r.RoomID.Substring(r.RoomID.Length - 2)) + 1;
                    }

                    string id = String.Format("{0:00}", n);
                    rOOM.RoomID = "R" + id;

                    db.ROOMs.Add(rOOM);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    db.Entry(rOOM).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(rOOM);
        }
        
        // POST: ROOMs/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                ROOM rOOM = db.ROOMs.Find(id);
                db.ROOMs.Remove(rOOM);
                db.SaveChanges();
            }
            catch
            {
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
