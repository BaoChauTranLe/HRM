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
    public class TIMEKEEPINGTABLEController : Controller
    {
        private hrmserver_HRMEntities db = new hrmserver_HRMEntities();
        // GET: TIMEKEEPING
        public ActionResult Index()
        {
            ViewBag.ListOfShiftType = SelectListItemHelper.GetShiftTypeList();
            
            return View();
        }
        public JsonResult GetShiftDetailList()
        {
            var results = db.SHIFTDETAILs.Select(e => new
            {
                id = e.ShiftID,
                start_date = e.StartDate,
                end_date = e.EndDate,
                event_length="7200",
                rec_type =""
            }).ToList();

            return new JsonResult() { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetEmployeeList()
        {
            try
            {
                List<EMPLOYEE> list = db.EMPLOYEEs.ToList();
                var employee = from s in list
                                      select new { s.EmployeeID, s.EmployeeName, s.ROOM.RoomName, s.POSITION.PositionName };
                var result = new { list = employee, str = "success" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var result = new { str = "fail" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}