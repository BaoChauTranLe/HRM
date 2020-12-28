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
    public class TIMEKEEPINGREPORTsController : Controller
    {
        private hrmserver_HRMEntities db = new hrmserver_HRMEntities();

        // GET: TIMEKEEPINGREPORTs
        public ActionResult Index()
        {
            var tIMEKEEPINGREPORTs = db.TIMEKEEPINGREPORTs.Include(t => t.EMPLOYEE);
            return View(tIMEKEEPINGREPORTs.ToList());
        }
        // GET: TIMEKEEPINGREPORTs
        public ActionResult OvertimeIndex()
        {
            var tIMEKEEPINGREPORTs = db.TIMEKEEPINGREPORTs.Include(t => t.EMPLOYEE);
            return View(tIMEKEEPINGREPORTs.ToList());
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
