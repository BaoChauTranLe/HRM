using HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HRM.Controllers
{
    public class HomeController : Controller
    {
        private hrmserver_HRMEntities db = new hrmserver_HRMEntities();
        public ActionResult Index()
        {
            if (Session["EmployeeID"] != null)
                return View();
            else
                return RedirectToAction("Login");
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(USER _user)
        {
            
            if (ModelState.IsValid)
            {
                var data = db.USERS.Where(s => s.EmployeeID.Equals(_user.EmployeeID) && s.Password.Equals(_user.Password));
                var valid_data = data.FirstOrDefault();
                if (valid_data != null)
                {
                    //add session
                    Session["EmployeeID"] = valid_data.EmployeeID;
                    EMPLOYEE e = db.EMPLOYEEs.Find(valid_data.EmployeeID);
                    Session["EmployeeName"] = e.EmployeeName;
                    Session["Position"] = e.POSITION.PositionName;
                    Session["Room"] = e.ROOM.RoomName;
                    Session["Image"] = e.Image;
                    return RedirectToAction("Details","EMPLOYEEs", new { id = valid_data.EmployeeID });
                }
                else
                {
                    ModelState.AddModelError("Password", "Tên đăng nhập hoặc mật khẩu không hợp lệ");
                    return View();
                }    
            }
            return View();
        }
        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}