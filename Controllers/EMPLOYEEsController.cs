using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HRM.Models;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;
using System.Data.Entity.Validation;

namespace HRM.Controllers
{
    public class EMPLOYEEsController : Controller
    {
        private hrmserver_HRMEntities db = new hrmserver_HRMEntities();

        // GET: EMPLOYEEs
        public ActionResult Index(string searchString)
        {
            var eMPLOYEEs = db.EMPLOYEE.Include(e => e.CONTRACT).Include(e => e.POSITION).Include(e => e.ROOM).Include(e => e.USERS).ToList();
            var list = new List<EmployeeViewModel>();
            for (int i = 0; i < eMPLOYEEs.Count(); i++)
            {
                list[i].EmployeeID = eMPLOYEEs[i].EmployeeID;
                list[i].EmployeeName = eMPLOYEEs[i].EmployeeName;
                list[i].Image = eMPLOYEEs[i].Image;
                list[i].Sex = eMPLOYEEs[i].Sex;
                list[i].RoomName = eMPLOYEEs[i].ROOM.RoomName;
                list[i].PositionName = eMPLOYEEs[i].POSITION.PositionName;
            }
            ViewData.Model = list;
            if (!String.IsNullOrEmpty(searchString)) // kiểm tra chuỗi tìm kiếm có rỗng/null hay không
            {
                var eMPLOYEE = eMPLOYEEs.Where(s => s.EmployeeID.Contains(searchString)); //lọc theo chuỗi tìm kiếm
                return View(eMPLOYEE);
            }
            return View(eMPLOYEEs);
        }

        // GET: EMPLOYEEs/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLOYEE eMPLOYEE = await db.EMPLOYEE.FindAsync(id);
            if (eMPLOYEE == null)
            {
                return HttpNotFound();
            }
            return View(eMPLOYEE);
        }

        // GET: EMPLOYEEs/Create
        public ActionResult Create()
        {
            //Tao ID nhan vien tu dong
            var emloyeeList = db.EMPLOYEE.SqlQuery("Select * from EMPLOYEE").ToList();
            int n = 0;
            string id;
            bool flag;
            do
            {
                n++;
                flag = false;
                String formatted = String.Format("{0:000000}", n);
                id = "NV" + formatted;
                foreach (var i in emloyeeList)
                {
                    if (i.EmployeeID == id)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            while (flag == true);
            List<SelectListItem> city = new List<SelectListItem>();
            List<SelectListItem> ward = new List<SelectListItem>();
            List<SelectListItem> dictrict = new List<SelectListItem>();
            List<SelectListItem> nation = new List<SelectListItem>();
            List<SelectListItem> education = new List<SelectListItem>();
            List<SelectListItem> contractType = new List<SelectListItem>();
            List<SelectListItem> personalIncomeTax = new List<SelectListItem>();
            var Directory = AppDomain.CurrentDomain.BaseDirectory;
            //path of folder
            var path = Directory + "./File_Text/";
            var encoding = Encoding.UTF8;
            string[] lines1 = System.IO.File.ReadAllLines(path + "City.txt", encoding);
            string[] lines2 = System.IO.File.ReadAllLines(path + "Ward.txt", encoding);
            string[] lines3 = System.IO.File.ReadAllLines(path + "Dictrict.txt", encoding);
            string[] lines4 = System.IO.File.ReadAllLines(path + "Nation.txt", encoding);
            string[] lines5 = System.IO.File.ReadAllLines(path + "ContractType.txt", encoding);
            string[] lines6 = System.IO.File.ReadAllLines(path + "PersonalIncomeTax.txt", encoding);
            foreach (string line in lines1)
            {
                city.Add(new SelectListItem()
                {
                    Text = line,
                    Value = line
                });
            }
            foreach (string line in lines2)
            {
                ward.Add(new SelectListItem()
                {
                    Text = line,
                    Value = line
                });
            }
            foreach (string line in lines3)
            {
                dictrict.Add(new SelectListItem()
                {
                    Text = line,
                    Value = line
                });
            }
            foreach (string line in lines4)
            {
                nation.Add(new SelectListItem()
                {
                    Text = line,
                    Value = line
                });
            }
            foreach (string line in lines5)
            {
                contractType.Add(new SelectListItem()
                {
                    Text = line,
                    Value = line
                });
            }
            foreach (string line in lines6)
            {
                personalIncomeTax.Add(new SelectListItem()
                {
                    Text = line,
                    Value = line
                });
            }
            SelectList citylist = new SelectList(city, "Value", "Text");
            SelectList wardlist = new SelectList(city, "Value", "Text");
            SelectList dictrictlist = new SelectList(city, "Value", "Text");
            SelectList nationlist = new SelectList(nation, "Value", "Text");
            SelectList contracttypelist = new SelectList(contractType, "Value", "Text");
            SelectList personalincometaxlist = new SelectList(personalIncomeTax, "Value", "Text");
            // Set vào ViewBag
            ViewBag.Birthplace = citylist;
            ViewBag.HomeTown = citylist;
            ViewBag.nationList = nationlist;
            ViewBag.ContractType = contracttypelist;
            ViewBag.City = citylist;
            ViewBag.Ward = wardlist;
            ViewBag.Dictrict = dictrictlist;
            ViewBag.PositionID = new SelectList(db.POSITION, "PositionID", "PositionName");
            ViewBag.RoomID = new SelectList(db.ROOM, "RoomID", "RoomName");
            ViewBag.EducationID = new SelectList(db.EDUCATION, "EducationID", "EducationName");
            ViewBag.MajorID = new SelectList(db.MAJOR, "MajorID", "MajorName");
            ViewBag.TypeCertificateID = new SelectList(db.CERTIFICATE, "TypeCertificateID", "TypeCertificate");
            ViewBag.User = new SelectList(db.GROUPPERMISSION, "GroupPermission1", "GroupPermissionName");
            ViewBag.EmployeeID = id;
            ViewBag.PersonalIncomeTax = personalincometaxlist;
            return View();
        }

        // POST: EMPLOYEEs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EmployeeName,Image,Sex,DoB,Birthcity,HomeTown,Nation,Id,Phone,Email,City,Ward,Dictrict,Street,RoomID,PositionID,ContractID,HealthInsurance,HealthInsuranceID,DeductionPersonal,DeductionDependent,EducationID,MajorID,Date,city,CertificateName,TypeCertificateID,CertificateDate,Certificatecity,ContractID,ContractType,DateStartWork,ContractExpirationDate,BasicSalary,PersonalIncomeTax,TrialTime")] EmployeeViewModel employee)
        {
            //Tao ID nhan vien tu dong
            var emloyeeList = db.EMPLOYEE.SqlQuery("Select * from EMPLOYEE").ToList();
            int n = 0;
            string id;
            bool flag;
            do
            {
                n++;
                flag = false;
                String formatted = String.Format("{0:000000}", n);
                id = "NV" + formatted;
                foreach (var i in emloyeeList)
                {
                    if (i.EmployeeID == id)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            while (flag == true);
            if(employee.DateStartWork > employee.ContractExpirationDate)
                ModelState.AddModelError("ContractExpirationDate", "Ngay het han hop dong phai nho hon ngay vao lam");
            EMPLOYEE eMPLOYEE = new EMPLOYEE();
            eMPLOYEE.EmployeeID = id;
            eMPLOYEE.EmployeeName = FormatProperCase(employee.EmployeeName); //Chuan hoa chuoi
            eMPLOYEE.Image = employee.Image;
            eMPLOYEE.Sex = employee.Sex;
            eMPLOYEE.DoB = employee.DoB;
            eMPLOYEE.Birthplace = employee.Birthplace;
            eMPLOYEE.HomeTown = employee.HomeTown;
            eMPLOYEE.Nation = employee.Nation;
            eMPLOYEE.Id = employee.Id;
            eMPLOYEE.Phone = employee.Phone;
            eMPLOYEE.Email = employee.Email;
            eMPLOYEE.City = employee.City;
            eMPLOYEE.Ward = employee.Ward;
            eMPLOYEE.Dictrict = employee.Dictrict;
            eMPLOYEE.RoomID = employee.RoomID;
            eMPLOYEE.PositionID = employee.PositionID;
            eMPLOYEE.ContractID = employee.ContractID;
            eMPLOYEE.HealthInsurance = employee.HealthInsurance;
            eMPLOYEE.HealthInsuranceID = employee.HealthInsuranceID;
            eMPLOYEE.SocialInsuranceID = employee.HealthInsuranceID.Substring(employee.HealthInsuranceID.Length - 10, 10);
            eMPLOYEE.DeductionPersonal = employee.DeductionPersonal;
            eMPLOYEE.DeductionDependent = employee.DeductionDependent;
            if (ModelState.IsValid)
            {
                EDUCATIONDETAIL eDUCATIONDETAIL = new EDUCATIONDETAIL();
                for (int i = 0; i < employee.EducationID.Count(); i++)
                {
                    eDUCATIONDETAIL.EmployeeID = id;
                    eDUCATIONDETAIL.EducationID = employee.EducationID[i];
                    eDUCATIONDETAIL.MajorID = employee.MajorID[i];
                    eDUCATIONDETAIL.Date = employee.Date[i];
                    eDUCATIONDETAIL.Place = employee.Place[i];
                    db.EDUCATIONDETAIL.Add(eDUCATIONDETAIL);
                }
                CERTIFICATEDETAIL cERTIFICATEDETAIL = new CERTIFICATEDETAIL();
                for (int i = 0; i < employee.CertificateName.Count(); i++)
                {
                    cERTIFICATEDETAIL.EmployeeID = id;
                    cERTIFICATEDETAIL.CertificateName = employee.CertificateName[i];
                    cERTIFICATEDETAIL.CertificateDate = employee.CertificateDate[i];
                    cERTIFICATEDETAIL.CertificatePlace = employee.CertificatePlace[i];
                    cERTIFICATEDETAIL.TypeCertificateID = employee.TypeCertificateID[i];
                    db.CERTIFICATEDETAIL.Add(cERTIFICATEDETAIL);
                }
                CONTRACT cONTRACT = new CONTRACT();
                cONTRACT.ContractID = employee.ContractID;
                cONTRACT.ContractType = employee.ContractType;
                cONTRACT.DateStartWork = employee.DateStartWork;
                cONTRACT.ContractExpirationDate = employee.ContractExpirationDate;
                cONTRACT.BasicSalary = employee.BasicSalary;
                cONTRACT.PersonalIncomeTax = employee.PersonalIncomeTax;
                db.CONTRACT.Add(cONTRACT);
                db.EMPLOYEE.Add(eMPLOYEE);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: EMPLOYEEs/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLOYEE eMPLOYEE = await db.EMPLOYEE.FindAsync(id);
            if (eMPLOYEE == null)
            {
                return HttpNotFound();
            }
            ViewBag.PositionID = new SelectList(db.POSITION, "PositionID", "PositionName");
            ViewBag.RoomID = new SelectList(db.ROOM, "RoomID", "RoomName");
            ViewBag.EducationID = new SelectList(db.EDUCATION, "EducationID", "EducationName");
            ViewBag.MajorID = new SelectList(db.MAJOR, "MajorID", "MajorName");
            ViewBag.TypeCertificateID = new SelectList(db.CERTIFICATE, "TypeCertificateID", "TypeCertificate");
            ViewBag.User = new SelectList(db.GROUPPERMISSION, "GroupPermission1", "GroupPermissionName");
            return View(eMPLOYEE);
        }

        // POST: EMPLOYEEs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EmployeeID,EmployeeName,Image,Sex,DoB,Birthcity,HomeTown,Nation,Id,Phone,Email,City,Ward,Dictrict,RoomID,PositionID,ContractID,HealthInsurance,HealthInsuranceID,SocialInsuranceID,DeductionPersonal,DeductionDependent")] EMPLOYEE eMPLOYEE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eMPLOYEE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(eMPLOYEE);
        }

        // GET: EMPLOYEEs/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLOYEE eMPLOYEE = await db.EMPLOYEE.FindAsync(id);
            if (eMPLOYEE == null)
            {
                return HttpNotFound();
            }
            return View(eMPLOYEE);
        }

        // POST: EMPLOYEEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            EMPLOYEE eMPLOYEE = await db.EMPLOYEE.FindAsync(id);
            db.EMPLOYEE.Remove(eMPLOYEE);
            await db.SaveChangesAsync();
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
        public string FormatProperCase(string str)
        {
            CultureInfo cultureInfo = new CultureInfo("vi-VN");
            TextInfo textInfo = cultureInfo.TextInfo;
            str = textInfo.ToLower(str);
            // Recity multiple white space to 1 white  space
            str = System.Text.RegularExpressions.Regex.Replace(str, @"\s{2,}", " ");
            //Upcase like Title
            return textInfo.ToTitleCase(str);
        }
    }
}
