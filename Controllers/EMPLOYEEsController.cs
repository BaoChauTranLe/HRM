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
        public async Task<ActionResult> Index()
        {
            var eMPLOYEEs = db.EMPLOYEEs.Include(e => e.CONTRACT).Include(e => e.POSITION).Include(e => e.ROOM).Include(e => e.USER);
            return View(await eMPLOYEEs.ToListAsync());
        }

        // GET: EMPLOYEEs/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLOYEE eMPLOYEE = await db.EMPLOYEEs.FindAsync(id);
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
            var emloyeeList = db.EMPLOYEEs.SqlQuery("Select * from EMPLOYEE").ToList();
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
            List<SelectListItem> place = new List<SelectListItem>();
            List<SelectListItem> nation = new List<SelectListItem>();
            List<SelectListItem> education = new List<SelectListItem>();
            List<SelectListItem> contractType = new List<SelectListItem>();
            List<SelectListItem> personalIncomeTax = new List<SelectListItem>();
            var Directory = AppDomain.CurrentDomain.BaseDirectory;
            //path of file
            var path1 = Directory + "./File_Text/Place.txt";
            var path2 = Directory + "./File_Text/Nation.txt";
            var path4 = Directory + "./File_Text/ContractType.txt";
            var path5 = Directory + "./File_Text/PersonalIncomeTax.txt";
            string[] lines1 = System.IO.File.ReadAllLines(path1, Encoding.UTF8);
            string[] lines2 = System.IO.File.ReadAllLines(path2, Encoding.UTF8);
            string[] lines4 = System.IO.File.ReadAllLines(path4, Encoding.UTF8);
            string[] lines5 = System.IO.File.ReadAllLines(path5, Encoding.UTF8);
            foreach (string line in lines1)
            {
                place.Add(new SelectListItem()
                {
                    Text = line,
                    Value = line
                });
            }
            foreach (string line in lines2)
            {
                nation.Add(new SelectListItem()
                {
                    Text = line,
                    Value = line
                });
            }
            foreach (string line in lines4)
            {
                contractType.Add(new SelectListItem()
                {
                    Text = line,
                    Value = line
                });
            }
            foreach (string line in lines5)
            {
                personalIncomeTax.Add(new SelectListItem()
                {
                    Text = line,
                    Value = line
                });
            }
            SelectList placelist = new SelectList(place, "Value", "Text");
            SelectList nationlist = new SelectList(nation, "Value", "Text");
            SelectList contracttypelist = new SelectList(contractType, "Value", "Text");
            SelectList personalincometaxlist = new SelectList(personalIncomeTax, "Value", "Text");
            // Set vào ViewBag
            ViewBag.Birthplace = placelist;
            ViewBag.HomeTown = placelist;
            ViewBag.nationList = nationlist;
            ViewBag.ContractType = contracttypelist;
            ViewBag.City = placelist;
            ViewBag.Ward = placelist;
            ViewBag.Dictrict = placelist;
            ViewBag.PositionID = new SelectList(db.POSITIONs, "PositionID", "PositionName");
            ViewBag.RoomID = new SelectList(db.ROOMs, "RoomID", "RoomName");
            ViewBag.EducationID = new SelectList(db.EDUCATIONs, "EducationID", "EducationName");
            ViewBag.MajorID = new SelectList(db.MAJORs, "MajorID", "MajorName");
            ViewBag.TypeCertificateID = new SelectList(db.CERTIFICATEs, "TypeCertificateID", "TypeCertificate");
            ViewBag.User = new SelectList(db.GROUPPERMISSIONs, "GroupPermission1", "GroupPermissionName");
            ViewBag.EmployeeID = id;
            ViewBag.PersonalIncomeTax = personalincometaxlist;
            return View();
        }

        // POST: EMPLOYEEs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EmployeeName,Image,Sex,DoB,Birthplace,HomeTown,Nation,Id,Phone,Email,City,Ward,Dictrict,RoomID,PositionID,ContractID,HealthInsurance,HealthInsuranceID,DeductionPersonal,DeductionDependent,EducationID,MajorID,Date,Place,CertificateName,TypeCertificateID,CertificateDate,CertificatePlace,ContractID,ContractType,DateStartWork,ContractExpirationDate,BasicSalary,PersonalIncomeTax,TrialTime")] EmployeeViewModel employee)
        {
            //Tao ID nhan vien tu dong
            var emloyeeList = db.EMPLOYEEs.SqlQuery("Select * from EMPLOYEE").ToList();
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
            EMPLOYEE eMPLOYEE = new EMPLOYEE();
            eMPLOYEE.EmployeeID = id;
            eMPLOYEE.EmployeeName = FormatProperCase(employee.EmployeeName); //Chuan hoa chuoi
            eMPLOYEE.Image = employee.Image;
            eMPLOYEE.Sex = Convert.ToBoolean(employee.Sex);
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
            eMPLOYEE.HealthInsurance = Convert.ToBoolean(employee.HealthInsurance);
            eMPLOYEE.HealthInsuranceID = employee.HealthInsuranceID;
            eMPLOYEE.SocialInsuranceID = employee.HealthInsuranceID.Substring(employee.HealthInsuranceID.Length - 10, 10);
            eMPLOYEE.DeductionPersonal = Convert.ToBoolean(employee.DeductionPersonal);
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
                    db.EDUCATIONDETAILs.Add(eDUCATIONDETAIL);
                }
                CERTIFICATEDETAIL cERTIFICATEDETAIL = new CERTIFICATEDETAIL();
                for (int i = 0; i < employee.CertificateName.Count(); i++)
                {
                    cERTIFICATEDETAIL.EmployeeID = id;
                    cERTIFICATEDETAIL.CertificateName = employee.CertificateName[i];
                    cERTIFICATEDETAIL.CertificateDate = employee.CertificateDate[i];
                    cERTIFICATEDETAIL.CertificatePlace = employee.CertificatePlace[i];
                    cERTIFICATEDETAIL.TypeCertificateID = employee.TypeCertificateID[i];
                    db.CERTIFICATEDETAILs.Add(cERTIFICATEDETAIL);
                }
                CONTRACT cONTRACT = new CONTRACT();
                cONTRACT.ContractID = employee.ContractID;
                cONTRACT.ContractType = employee.ContractType;
                cONTRACT.DateStartWork = employee.DateStartWork;
                cONTRACT.ContractExpirationDate = employee.ContractExpirationDate;
                cONTRACT.BasicSalary = employee.BasicSalary;
                cONTRACT.PersonalIncomeTax = employee.PersonalIncomeTax;
                db.CONTRACTs.Add(cONTRACT);
                db.EMPLOYEEs.Add(eMPLOYEE);
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
            EMPLOYEE eMPLOYEE = await db.EMPLOYEEs.FindAsync(id);
            if (eMPLOYEE == null)
            {
                return HttpNotFound();
            }
            return View(eMPLOYEE);
        }

        // POST: EMPLOYEEs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EmployeeID,EmployeeName,Image,Sex,DoB,Birthplace,HomeTown,Nation,Id,Phone,Email,City,Ward,Dictrict,RoomID,PositionID,ContractID,HealthInsurance,HealthInsuranceID,SocialInsuranceID,DeductionPersonal,DeductionDependent")] EMPLOYEE eMPLOYEE)
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
            EMPLOYEE eMPLOYEE = await db.EMPLOYEEs.FindAsync(id);
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
            EMPLOYEE eMPLOYEE = await db.EMPLOYEEs.FindAsync(id);
            db.EMPLOYEEs.Remove(eMPLOYEE);
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
            // Replace multiple white space to 1 white  space
            str = System.Text.RegularExpressions.Regex.Replace(str, @"\s{2,}", " ");
            //Upcase like Title
            return textInfo.ToTitleCase(str);
        }
    }
}
