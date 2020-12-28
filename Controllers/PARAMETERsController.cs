using HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.Controllers
{
    public class PARAMETERsController : Controller
    {
        private hrmserver_HRMEntities db = new hrmserver_HRMEntities();
        // GET: PARAMETERs
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Employee()
        {
            return View();
        }
        public ActionResult EmployeeEdit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EmployeeEdit(int MinFemaleAge, int MaxFemaleAge, int MinMaleAge, int MaxMaleAge, int AreaMinSalary)
        {
            if (ModelState.IsValid)
            {
                db.PARAMETERs.Find("TuoiToiThieuNu").Value = MinFemaleAge;
                db.PARAMETERs.Find("TuoiToiDaNu").Value = MaxFemaleAge;
                db.PARAMETERs.Find("TuoiToiThieuNam").Value = MinMaleAge;
                db.PARAMETERs.Find("TuoiToiDaNam").Value = MaxMaleAge;
                db.PARAMETERs.Find("MucLuongToiThieuVung").Value = AreaMinSalary;
                db.SaveChanges();
                return RedirectToAction("Employee");
            }    
            return View();
        }
        public ActionResult CoefficientsOTSalary()
        {
            return View();
        }
        public ActionResult IncomeTax()
        {
            return View();
        }
        public int getValueByName(string paraName)
        {
            var parameter = db.PARAMETERs.Find(paraName);
            return parameter.Value;
        }
    }
}