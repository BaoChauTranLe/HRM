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
        public ActionResult CoefficientsOTSalary()
        {
            return View();
        }
        public ActionResult IncomeTax()
        {
            return View();
        }
        public int getValueByName(string name)
        {
            var parameter = db.PARAMETERs.Find(name);
            return parameter.Value;
        }
    }
}