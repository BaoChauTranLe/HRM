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
        
        public JsonResult GetShiftDetail()
        {
            try
            {
                List<SHIFTDETAIL> ShiftDetailList = db.SHIFTDETAILs.ToList();
                return Json(ShiftDetailList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        public JsonResult GetEmployeeById(string id)
        {
            EMPLOYEE employee = db.EMPLOYEEs.Find(id);
            return Json(employee, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeList()
        {
            try
            {
                List<EMPLOYEE> EmployeeList = db.EMPLOYEEs.ToList();
                return Json(EmployeeList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "ShiftID,ShiftName,ShiftType,StartTime,EndTime,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday")] SHIFT sHIFT)
        {
            

            return View();
        }
    }
   
}