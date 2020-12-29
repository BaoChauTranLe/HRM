﻿using System;
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
            var eMPLOYEEs = db.EMPLOYEEs.Include(e => e.CONTRACT).Include(e => e.POSITION).Include(e => e.ROOM).Include(e => e.USER).ToList();
            var list = new List<EmployeeViewModel>();
            foreach (var item in eMPLOYEEs)
            {
                list.Add(new EmployeeViewModel()
                {
                    EmployeeID = item.EmployeeID,
                    EmployeeName = item.EmployeeName,
                    Image = item.Image,
                    Sex = item.Sex,
                    RoomName = item.ROOM.RoomName,
                    PositionName = item.POSITION.PositionName,
                });
            }
            if (!String.IsNullOrEmpty(searchString)) // kiểm tra chuỗi tìm kiếm có rỗng/null hay không
            {
                IEnumerable<EmployeeViewModel> enumerable = list.Where(s => s.EmployeeID.Contains(searchString));
                return View(enumerable);
            }
            return View(list);
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
            EmployeeViewModel employee = new EmployeeViewModel();
            employee.EmployeeID = eMPLOYEE.EmployeeID;
            employee.EmployeeName = eMPLOYEE.EmployeeName;
            employee.Image = eMPLOYEE.Image;
            employee.Sex = eMPLOYEE.Sex;
            employee.DoB = eMPLOYEE.DoB;
            employee.Birthplace = eMPLOYEE.Birthplace;
            employee.HomeTown = eMPLOYEE.HomeTown;
            employee.Nation = eMPLOYEE.Nation;
            employee.Id = eMPLOYEE.Id;
            employee.Phone = eMPLOYEE.Phone;
            employee.Email = eMPLOYEE.Email;
            employee.City = eMPLOYEE.City;
            employee.Ward = eMPLOYEE.Ward;
            employee.Dictrict = eMPLOYEE.Dictrict;
            employee.RoomID = eMPLOYEE.ROOM.RoomName;
            employee.PositionID = eMPLOYEE.POSITION.PositionName;
            employee.ContractID = eMPLOYEE.ContractID;
            employee.HealthInsurance = eMPLOYEE.HealthInsurance;
            employee.HealthInsuranceID = eMPLOYEE.HealthInsuranceID;
            employee.SocialInsuranceID = eMPLOYEE.HealthInsuranceID.Substring(eMPLOYEE.HealthInsuranceID.Length - 10, 10);
            employee.DeductionPersonal = eMPLOYEE.DeductionPersonal;
            employee.DeductionDependent = (int)eMPLOYEE.DeductionDependent;
            List<EDUCATIONDETAIL> eDUCATIONDETAIL = db.EDUCATIONDETAILs.SqlQuery("Select * from EDUCATIONDETAIL where employeeID = '" + id + "'").ToList();
            CONTRACT cONTRACT = await db.CONTRACTs.FindAsync(eMPLOYEE.ContractID);
            List<CERTIFICATEDETAIL> cERTIFICATEDETAIL = db.CERTIFICATEDETAILs.SqlQuery("Select * from CERTIFICATEDETAIL where employeeID = '" + id + "'").ToList();
            //Trinh do
            employee.EducationName = eDUCATIONDETAIL[0].EDUCATION.EducationName;
            employee.MajorName = eDUCATIONDETAIL[0].MAJOR.MajorName;
            employee.Date = eDUCATIONDETAIL[0].Date;
            employee.Place = eDUCATIONDETAIL[0].Place;
            //Chung chi
            employee.CertificateName = cERTIFICATEDETAIL[0].CertificateName;
            employee.TypeCertificate = cERTIFICATEDETAIL[0].CERTIFICATE.TypeCertificate;
            employee.CertificateDate = cERTIFICATEDETAIL[0].CertificateDate;
            employee.CertificatePlace = cERTIFICATEDETAIL[0].CertificatePlace;
            //Hop dong
            employee.ContractID = cONTRACT.ContractID;
            employee.ContractType = cONTRACT.ContractType;
            employee.DateStartWork = cONTRACT.DateStartWork;
            employee.ContractExpirationDate = cONTRACT.ContractExpirationDate;
            employee.BasicSalary = (int)cONTRACT.BasicSalary;
            employee.PersonalIncomeTax = cONTRACT.PersonalIncomeTax;
            return View(employee);
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
            List<SelectListItem> city = new List<SelectListItem>();
            List<SelectListItem> ward = new List<SelectListItem>();
            List<SelectListItem> dictrict = new List<SelectListItem>();
            List<SelectListItem> nation = new List<SelectListItem>();
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
            SelectList wardlist = new SelectList(ward, "Value", "Text");
            SelectList dictrictlist = new SelectList(dictrict, "Value", "Text");
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
            ViewBag.PersonalIncomeTax = personalincometaxlist;
            ViewBag.PositionID = new SelectList(db.POSITIONs, "PositionID", "PositionName");
            ViewBag.RoomID = new SelectList(db.ROOMs, "RoomID", "RoomName");
            ViewBag.EducationID = new SelectList(db.EDUCATIONs, "EducationID", "EducationName");
            ViewBag.MajorID = new SelectList(db.MAJORs, "MajorID", "MajorName");
            ViewBag.TypeCertificateID = new SelectList(db.CERTIFICATEs, "TypeCertificateID", "TypeCertificate");
            ViewBag.User = new SelectList(db.GROUPPERMISSIONs, "GroupPermission1", "GroupPermissionName");
            ViewBag.EmployeeID = id;
            return View();
        }

        // POST: EMPLOYEEs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EmployeeName,Image,Sex,DoB,Birthplace,HomeTown,Nation,Id,Phone,Email,City,Ward,Dictrict,Street,RoomID,PositionID,ContractID,HealthInsurance,HealthInsuranceID,DeductionPersonal,DeductionDependent,EducationID,MajorID,Date,Place,CertificateName,TypeCertificateID,CertificateDate,CertificatePlace,ContractID,ContractType,DateStartWork,ContractExpirationDate,BasicSalary,PersonalIncomeTax,TrialTime,Password")] EmployeeViewModel employee)
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
            List<SelectListItem> city = new List<SelectListItem>();
            List<SelectListItem> ward = new List<SelectListItem>();
            List<SelectListItem> dictrict = new List<SelectListItem>();
            List<SelectListItem> nation = new List<SelectListItem>();
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
            SelectList wardlist = new SelectList(ward, "Value", "Text");
            SelectList dictrictlist = new SelectList(dictrict, "Value", "Text");
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
            ViewBag.PersonalIncomeTax = personalincometaxlist;
            ViewBag.PositionID = new SelectList(db.POSITIONs, "PositionID", "PositionName");
            ViewBag.RoomID = new SelectList(db.ROOMs, "RoomID", "RoomName");
            ViewBag.EducationID = new SelectList(db.EDUCATIONs, "EducationID", "EducationName");
            ViewBag.MajorID = new SelectList(db.MAJORs, "MajorID", "MajorName");
            ViewBag.TypeCertificateID = new SelectList(db.CERTIFICATEs, "TypeCertificateID", "TypeCertificate");
            ViewBag.User = new SelectList(db.GROUPPERMISSIONs, "GroupPermission1", "GroupPermissionName");
            ViewBag.EmployeeID = id;
            if (ModelState.IsValid)
            {
                EMPLOYEE eMPLOYEE = new EMPLOYEE();
                eMPLOYEE.EmployeeID = id;
                eMPLOYEE.EmployeeName = FormatProperCase(employee.EmployeeName); //Chuan hoa chuoi
                eMPLOYEE.Image = employee.Image;
                eMPLOYEE.Sex = employee.Sex;
                eMPLOYEE.DoB = (DateTime)employee.DoB;
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
                eMPLOYEE.DeductionPersonal = employee.DeductionPersonal;
                eMPLOYEE.DeductionDependent = employee.DeductionDependent;
                //Trinh do
                EDUCATIONDETAIL eDUCATIONDETAIL = new EDUCATIONDETAIL();
                eDUCATIONDETAIL.EmployeeID = id;
                eDUCATIONDETAIL.EducationID = employee.EducationID;
                if (eDUCATIONDETAIL.EducationID == "E01" || eDUCATIONDETAIL.EducationID == "E02" || eDUCATIONDETAIL.EducationID == "E03")
                    eDUCATIONDETAIL.MajorID = "M09";
                else
                    eDUCATIONDETAIL.MajorID = employee.MajorID;
                eDUCATIONDETAIL.Date = employee.Date;
                eDUCATIONDETAIL.Place = employee.Place;
                db.EDUCATIONDETAILs.Add(eDUCATIONDETAIL);
                //Chung chi
                CERTIFICATEDETAIL cERTIFICATEDETAIL = new CERTIFICATEDETAIL();
                cERTIFICATEDETAIL.EmployeeID = id;
                cERTIFICATEDETAIL.CertificateName = employee.CertificateName;
                cERTIFICATEDETAIL.CertificateDate = employee.CertificateDate;
                cERTIFICATEDETAIL.CertificatePlace = employee.CertificatePlace;
                cERTIFICATEDETAIL.TypeCertificateID = employee.TypeCertificateID;
                db.CERTIFICATEDETAILs.Add(cERTIFICATEDETAIL);
                //Hop dong
                CONTRACT cONTRACT = new CONTRACT();
                cONTRACT.ContractID = employee.ContractID;
                cONTRACT.ContractType = employee.ContractType;
                cONTRACT.DateStartWork = employee.DateStartWork;
                cONTRACT.ContractExpirationDate = employee.ContractExpirationDate;
                cONTRACT.BasicSalary = employee.BasicSalary;
                cONTRACT.PersonalIncomeTax = employee.PersonalIncomeTax;
                if (employee.TrialTime == null)
                    cONTRACT.TrialTime = 0;
                else
                {
                    if (cONTRACT.ContractType == "Hợp đồng thử việc")
                        cONTRACT.TrialTime = employee.TrialTime;
                    else
                        cONTRACT.TrialTime = 0;
                }
                //Tai khoan
                USER uSER = new USER();
                uSER.EmployeeID = employee.EmployeeID;
                uSER.Password = employee.Password;
                db.CONTRACTs.Add(cONTRACT);
                db.EMPLOYEEs.Add(eMPLOYEE);
                db.USERS.Add(uSER);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Error");
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
            EmployeeViewModel employee = new EmployeeViewModel();
            List<SelectListItem> city = new List<SelectListItem>();
            List<SelectListItem> ward = new List<SelectListItem>();
            List<SelectListItem> dictrict = new List<SelectListItem>();
            List<SelectListItem> nation = new List<SelectListItem>();
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
            SelectList wardlist = new SelectList(ward, "Value", "Text");
            SelectList dictrictlist = new SelectList(dictrict, "Value", "Text");
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
            ViewBag.PersonalIncomeTax = personalincometaxlist;
            ViewBag.PositionID = new SelectList(db.POSITIONs, "PositionID", "PositionName");
            ViewBag.RoomID = new SelectList(db.ROOMs, "RoomID", "RoomName");
            ViewBag.EducationID = new SelectList(db.EDUCATIONs, "EducationID", "EducationName");
            ViewBag.MajorID = new SelectList(db.MAJORs, "MajorID", "MajorName");
            ViewBag.TypeCertificateID = new SelectList(db.CERTIFICATEs, "TypeCertificateID", "TypeCertificate");
            ViewBag.User = new SelectList(db.GROUPPERMISSIONs, "GroupPermission1", "GroupPermissionName");
            employee.EmployeeID = eMPLOYEE.EmployeeID;
            employee.EmployeeName = eMPLOYEE.EmployeeName;
            employee.Image = eMPLOYEE.Image;
            employee.Sex = eMPLOYEE.Sex;
            employee.DoB = eMPLOYEE.DoB;
            employee.Birthplace = eMPLOYEE.Birthplace;
            employee.HomeTown = eMPLOYEE.HomeTown;
            employee.Nation = eMPLOYEE.Nation;
            employee.Id = eMPLOYEE.Id;
            employee.Phone = eMPLOYEE.Phone;
            employee.Email = eMPLOYEE.Email;
            employee.City = eMPLOYEE.City;
            employee.Ward = eMPLOYEE.Ward;
            employee.Dictrict = eMPLOYEE.Dictrict;
            employee.RoomID = eMPLOYEE.RoomID;
            employee.PositionID = eMPLOYEE.PositionID;
            employee.ContractID = eMPLOYEE.ContractID;
            employee.HealthInsurance = (bool)eMPLOYEE.HealthInsurance;
            employee.HealthInsuranceID = eMPLOYEE.HealthInsuranceID;
            employee.DeductionPersonal = (bool)eMPLOYEE.DeductionPersonal;
            employee.DeductionDependent = (int)eMPLOYEE.DeductionDependent;
            List<EDUCATIONDETAIL> eDUCATIONDETAIL = db.EDUCATIONDETAILs.SqlQuery("Select * from EDUCATIONDETAIL where employeeID = '" + id + "'").ToList();
            CONTRACT cONTRACT = await db.CONTRACTs.FindAsync(eMPLOYEE.ContractID);
            List<CERTIFICATEDETAIL> cERTIFICATEDETAIL = db.CERTIFICATEDETAILs.SqlQuery("Select * from CERTIFICATEDETAIL where employeeID = '" + id + "'").ToList();
            //Trinh do
            employee.EducationID = eDUCATIONDETAIL[0].EducationID;
            employee.MajorID = eDUCATIONDETAIL[0].MajorID;
            employee.Date = eDUCATIONDETAIL[0].Date;
            employee.Place = eDUCATIONDETAIL[0].Place;
            //Chung chi
            employee.CertificateName = cERTIFICATEDETAIL[0].CertificateName;
            employee.TypeCertificateID = cERTIFICATEDETAIL[0].TypeCertificateID;
            employee.CertificateDate = cERTIFICATEDETAIL[0].CertificateDate;
            employee.CertificatePlace = cERTIFICATEDETAIL[0].CertificatePlace;
            //Hop dong
            employee.ContractID = cONTRACT.ContractID;
            employee.ContractType = cONTRACT.ContractType;
            employee.DateStartWork = cONTRACT.DateStartWork;
            employee.ContractExpirationDate = cONTRACT.ContractExpirationDate;
            employee.BasicSalary = (int)cONTRACT.BasicSalary;
            employee.PersonalIncomeTax = cONTRACT.PersonalIncomeTax;
            return View(employee);
        }

        // POST: EMPLOYEEs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EmployeeID,EmployeeName,Image,Sex,DoB,Birthcity,HomeTown,Nation,Id,Phone,Email,City,Ward,Dictrict,Street,RoomID,PositionID,ContractID,HealthInsurance,HealthInsuranceID,DeductionPersonal,DeductionDependent,EducationID,MajorID,Date,city,CertificateName,TypeCertificateID,CertificateDate,Certificatecity,ContractID,ContractType,DateStartWork,ContractExpirationDate,BasicSalary,PersonalIncomeTax,TrialTime")] EmployeeViewModel employee)
        {
            List<SelectListItem> city = new List<SelectListItem>();
            List<SelectListItem> ward = new List<SelectListItem>();
            List<SelectListItem> dictrict = new List<SelectListItem>();
            List<SelectListItem> nation = new List<SelectListItem>();
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
            SelectList wardlist = new SelectList(ward, "Value", "Text");
            SelectList dictrictlist = new SelectList(dictrict, "Value", "Text");
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
            ViewBag.PersonalIncomeTax = personalincometaxlist;
            ViewBag.PositionID = new SelectList(db.POSITIONs, "PositionID", "PositionName");
            ViewBag.RoomID = new SelectList(db.ROOMs, "RoomID", "RoomName");
            ViewBag.EducationID = new SelectList(db.EDUCATIONs, "EducationID", "EducationName");
            ViewBag.MajorID = new SelectList(db.MAJORs, "MajorID", "MajorName");
            ViewBag.TypeCertificateID = new SelectList(db.CERTIFICATEs, "TypeCertificateID", "TypeCertificate");
            ViewBag.User = new SelectList(db.GROUPPERMISSIONs, "GroupPermission1", "GroupPermissionName");
            if (ModelState.IsValid)
            {
                EMPLOYEE eMPLOYEE = new EMPLOYEE();
                eMPLOYEE.EmployeeID = employee.EmployeeID;
                eMPLOYEE.EmployeeName = FormatProperCase(employee.EmployeeName); //Chuan hoa chuoi
                eMPLOYEE.Image = employee.Image;
                eMPLOYEE.Sex = employee.Sex;
                eMPLOYEE.DoB = (DateTime)employee.DoB;
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
                eMPLOYEE.DeductionPersonal = employee.DeductionPersonal;
                eMPLOYEE.DeductionDependent = employee.DeductionDependent;
                db.Entry(eMPLOYEE).State = EntityState.Modified;
                //Trinh do
                EDUCATIONDETAIL eDUCATIONDETAIL = new EDUCATIONDETAIL();
                eDUCATIONDETAIL.EmployeeID = employee.EmployeeID;
                eDUCATIONDETAIL.EducationID = employee.EducationID;
                if (eDUCATIONDETAIL.EducationID == "E01" || eDUCATIONDETAIL.EducationID == "E02" || eDUCATIONDETAIL.EducationID == "E03")
                    eDUCATIONDETAIL.MajorID = "M09";
                else
                    eDUCATIONDETAIL.MajorID = employee.MajorID;
                eDUCATIONDETAIL.Date = employee.Date;
                eDUCATIONDETAIL.Place = employee.Place;
                //Chung chi
                CERTIFICATEDETAIL cERTIFICATEDETAIL = new CERTIFICATEDETAIL();
                cERTIFICATEDETAIL.EmployeeID = employee.EmployeeID;
                cERTIFICATEDETAIL.CertificateName = employee.CertificateName;
                cERTIFICATEDETAIL.CertificateDate = employee.CertificateDate;
                cERTIFICATEDETAIL.CertificatePlace = employee.CertificatePlace;
                cERTIFICATEDETAIL.TypeCertificateID = employee.TypeCertificateID;
                //Hop dong
                CONTRACT cONTRACT = new CONTRACT();
                cONTRACT.ContractID = employee.ContractID;
                cONTRACT.ContractType = employee.ContractType;
                cONTRACT.DateStartWork = employee.DateStartWork;
                cONTRACT.ContractExpirationDate = employee.ContractExpirationDate;
                cONTRACT.BasicSalary = employee.BasicSalary;
                cONTRACT.PersonalIncomeTax = employee.PersonalIncomeTax;
                if (employee.TrialTime == null)
                    cONTRACT.TrialTime = 0;
                else
                {
                    if (cONTRACT.ContractType == "Hợp đồng thử việc")
                        cONTRACT.TrialTime = employee.TrialTime;
                    else
                        cONTRACT.TrialTime = 0;
                }
                db.Entry(eDUCATIONDETAIL).State = EntityState.Modified;
                db.Entry(cERTIFICATEDETAIL).State = EntityState.Modified;
                db.Entry(cONTRACT).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(employee);
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
            List<EDUCATIONDETAIL> eDUCATIONDETAIL = db.EDUCATIONDETAILs.SqlQuery("Select * from EDUCATIONDETAIL where employeeID = '" + id + "'").ToList();
            CONTRACT cONTRACT = await db.CONTRACTs.FindAsync(eMPLOYEE.ContractID);
            List<CERTIFICATEDETAIL> cERTIFICATEDETAIL = db.CERTIFICATEDETAILs.SqlQuery("Select * from CERTIFICATEDETAIL where employeeID = '" + id + "'").ToList();
            List<SHIFTDETAIL> sHIFTDETAILs = db.SHIFTDETAILs.SqlQuery("Select * from SHIFTDETAIL where employeeID = '" + id + "'").ToList();
            if (sHIFTDETAILs.Count() != 0)
            {
                return RedirectToAction("Error");
            }
            foreach (var item in eDUCATIONDETAIL)
            {
                db.EDUCATIONDETAILs.Remove(item);
            }
            foreach (var item in cERTIFICATEDETAIL)
            {
                db.CERTIFICATEDETAILs.Remove(item);
            }
            db.CONTRACTs.Remove(cONTRACT);
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
            // Recity multiple white space to 1 white  space
            str = System.Text.RegularExpressions.Regex.Replace(str, @"\s{2,}", " ");
            //Upcase like Title
            return textInfo.ToTitleCase(str);
        }
    }
}
