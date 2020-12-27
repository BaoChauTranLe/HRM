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
using DHTMLX.Scheduler;

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
            try
            {
                List<SHIFTDETAIL> list = db.SHIFTDETAILs.ToList();
                var ShiftDetailList = from s in list
                                select new { s.ShiftID, s.EmployeeID, s.StartDate, s.EndDate };
                var result = new { list = ShiftDetailList, str = "success" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var result = new { str = "fail" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
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
        public ActionResult TimeKeepingView()
		{
            var scheduler = new DHXScheduler(this);
            return View(scheduler);

        }
    }
}