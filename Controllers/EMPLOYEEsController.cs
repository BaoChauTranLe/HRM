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
using System.IO;

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
                    Phone = item.Phone,
                    RoomName = item.ROOM.RoomName,
                    PositionName = item.POSITION.PositionName,
                    State = (bool)item.State,
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
            employee.IdNumber = eMPLOYEE.IdNumber;
            employee.Phone = eMPLOYEE.Phone;
            employee.Email = eMPLOYEE.Email;
            employee.City = eMPLOYEE.City;
            employee.Ward = eMPLOYEE.Ward;
            employee.Dictrict = eMPLOYEE.Dictrict;
            employee.RoomName = eMPLOYEE.ROOM.RoomName;
            employee.PositionName = eMPLOYEE.POSITION.PositionName;
            employee.ContractID = eMPLOYEE.ContractID;
            employee.HealthInsurance = eMPLOYEE.FreeInsurance;
            employee.HealthInsuranceID = eMPLOYEE.HealthInsuranceID;
            employee.SocialInsuranceID = eMPLOYEE.HealthInsuranceID.Substring(eMPLOYEE.HealthInsuranceID.Length - 10, 10);
            employee.DeductionPersonal = eMPLOYEE.SelfDeduction;
            employee.DeductionDependent = (int)eMPLOYEE.DependentDeduction;
            employee.State = (bool)eMPLOYEE.State;
            List<EDUCATIONDETAIL> eDUCATIONDETAIL = db.EDUCATIONDETAILs.SqlQuery("Select * from EDUCATIONDETAIL where employeeID = '" + id + "'").ToList();
            CONTRACT cONTRACT = await db.CONTRACTs.FindAsync(eMPLOYEE.ContractID);
            List<CERTIFICATEDETAIL> cERTIFICATEDETAIL = db.CERTIFICATEDETAILs.SqlQuery("Select * from CERTIFICATEDETAIL where employeeID = '" + id + "'").ToList();
            //Trinh do
            employee.EducationName = eDUCATIONDETAIL[0].EducationName;
            employee.MajorName = eDUCATIONDETAIL[0].MAJOR.MajorName;
            employee.Date = eDUCATIONDETAIL[0].Date;
            employee.Place = eDUCATIONDETAIL[0].Place;
            //Chung chi
            employee.CertificateName = cERTIFICATEDETAIL[0].CertificateName;
            employee.TypeCertificate = cERTIFICATEDETAIL[0].TypeCertificate;
            employee.CertificateDate = cERTIFICATEDETAIL[0].CertificateDate;
            employee.CertificatePlace = cERTIFICATEDETAIL[0].CertificatePlace;
            //Hop dong
            employee.ContractID = cONTRACT.ContractID;
            employee.ContractType = cONTRACT.ContractType;
            employee.DateStartWork = cONTRACT.DateStartWork;
            employee.ContractExpirationDate = cONTRACT.ContractExpirationDate;
            employee.BasicSalary = (int)cONTRACT.BasicSalary;
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
            List<SelectListItem> education = new List<SelectListItem>();
            List<SelectListItem> typecertificate = new List<SelectListItem>();
            var Directory = AppDomain.CurrentDomain.BaseDirectory;
            //path of folder
            var path = Directory + "./File_Text/";
            var encoding = Encoding.UTF8;
            string[] lines1 = System.IO.File.ReadAllLines(path + "City.txt", encoding);
            string[] lines2 = System.IO.File.ReadAllLines(path + "Ward.txt", encoding);
            string[] lines3 = System.IO.File.ReadAllLines(path + "Dictrict.txt", encoding);
            string[] lines4 = System.IO.File.ReadAllLines(path + "Nation.txt", encoding);
            string[] lines5 = System.IO.File.ReadAllLines(path + "ContractType.txt", encoding);
            string[] lines6 = System.IO.File.ReadAllLines(path + "Education.txt", encoding);
            string[] lines7 = System.IO.File.ReadAllLines(path + "TypeCertificate.txt", encoding);
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
                education.Add(new SelectListItem()
                {
                    Text = line,
                    Value = line
                });
            }
            foreach (string line in lines7)
            {
                typecertificate.Add(new SelectListItem()
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
            SelectList educationlist = new SelectList(education, "Value", "Text");
            SelectList certificatelist = new SelectList(typecertificate, "Value", "Text");
            // Set vào ViewBag
            ViewBag.Birthplace = citylist;
            ViewBag.HomeTown = citylist;
            ViewBag.nationList = nationlist;
            ViewBag.ContractType = contracttypelist;
            ViewBag.City = citylist;
            ViewBag.Ward = wardlist;
            ViewBag.Dictrict = dictrictlist;
            ViewBag.EducationName = educationlist;
            ViewBag.PositionID = new SelectList(db.POSITIONs, "PositionID", "PositionName");
            ViewBag.RoomID = new SelectList(db.ROOMs, "RoomID", "RoomName");
            ViewBag.MajorID = new SelectList(db.MAJORs, "MajorID", "MajorName");
            ViewBag.TypeCertificate = certificatelist;
            ViewBag.EmployeeID = id;
            return View();
        }

        // POST: EMPLOYEEs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EmployeeName,ImageFile,Sex,DoB,Birthplace,HomeTown,Nation,IdNumber,Phone,Email,City,Ward,Dictrict,Street,RoomID,PositionID,ContractID,HealthInsurance,HealthInsuranceID,DeductionPersonal,DeductionDependent,EducationName,MajorID,Date,Place,CertificateName,TypeCertificate,CertificateDate,CertificatePlace,ContractID,ContractType,DateStartWork,ContractExpirationDate,BasicSalary,Password")] EmployeeViewModel employee)
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
            List<SelectListItem> education = new List<SelectListItem>();
            List<SelectListItem> typecertificate = new List<SelectListItem>();
            var Directory = AppDomain.CurrentDomain.BaseDirectory;
            //path of folder
            var path = Directory + "./File_Text/";
            var encoding = Encoding.UTF8;
            string[] lines1 = System.IO.File.ReadAllLines(path + "City.txt", encoding);
            string[] lines2 = System.IO.File.ReadAllLines(path + "Ward.txt", encoding);
            string[] lines3 = System.IO.File.ReadAllLines(path + "Dictrict.txt", encoding);
            string[] lines4 = System.IO.File.ReadAllLines(path + "Nation.txt", encoding);
            string[] lines5 = System.IO.File.ReadAllLines(path + "ContractType.txt", encoding);
            string[] lines6 = System.IO.File.ReadAllLines(path + "Education.txt", encoding);
            string[] lines7 = System.IO.File.ReadAllLines(path + "TypeCertificate.txt", encoding);
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
                education.Add(new SelectListItem()
                {
                    Text = line,
                    Value = line
                });
            }
            foreach (string line in lines7)
            {
                typecertificate.Add(new SelectListItem()
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
            SelectList educationlist = new SelectList(education, "Value", "Text");
            SelectList certificatelist = new SelectList(typecertificate, "Value", "Text");
            // Set vào ViewBag
            ViewBag.Birthplace = citylist;
            ViewBag.HomeTown = citylist;
            ViewBag.nationList = nationlist;
            ViewBag.ContractType = contracttypelist;
            ViewBag.City = citylist;
            ViewBag.Ward = wardlist;
            ViewBag.Dictrict = dictrictlist;
            ViewBag.EducationName = educationlist;
            ViewBag.PositionID = new SelectList(db.POSITIONs, "PositionID", "PositionName");
            ViewBag.RoomID = new SelectList(db.ROOMs, "RoomID", "RoomName");
            ViewBag.MajorID = new SelectList(db.MAJORs, "MajorID", "MajorName");
            ViewBag.TypeCertificate = certificatelist;
            ViewBag.EmployeeID = id;
            if (employee.ContractExpirationDate < employee.DateStartWork)
                ModelState.AddModelError("ContractExpirationDate", "Ngày kết thúc phải lớn hơn ngày bắt đầu");
            var Parameter = db.PARAMETERs.ToList();
            var tuoi = DateTime.Now.Year - employee.DoB.Year;
            if (employee.Sex == "Nam")
            {
                if (tuoi < Parameter[19].Value) //tuoi toi thieu nam
                    ModelState.AddModelError("DoB", "Tuổi không hợp lệ");
                if (tuoi > Parameter[17].Value) //tuoi toi da nam
                    ModelState.AddModelError("DoB", "Tuổi không hợp lệ");
            }
            else
            {
                if (tuoi < Parameter[17].Value) //tuoi toi thieu nu
                    ModelState.AddModelError("DoB", "Tuổi không hợp lệ");
                if (tuoi > Parameter[18].Value) //tuoi toi da nu
                    ModelState.AddModelError("DoB", "Tuổi không hợp lệ");
            }
            if (ModelState.IsValid)
            {
                EMPLOYEE eMPLOYEE = new EMPLOYEE();
                eMPLOYEE.EmployeeID = id;
                eMPLOYEE.EmployeeName = FormatProperCase(employee.EmployeeName); //Chuan hoa chuoi
                if (employee.ImageFile != null)
                {
                    BinaryReader b = new BinaryReader(employee.ImageFile.InputStream);
                    byte[] binData = b.ReadBytes(employee.ImageFile.ContentLength);
                    eMPLOYEE.Image = binData;
                }
                eMPLOYEE.Sex = employee.Sex;
                eMPLOYEE.DoB = (DateTime)employee.DoB;
                eMPLOYEE.Birthplace = employee.Birthplace;
                eMPLOYEE.HomeTown = employee.HomeTown;
                eMPLOYEE.Nation = employee.Nation;
                eMPLOYEE.IdNumber = employee.IdNumber;
                eMPLOYEE.Phone = employee.Phone;
                eMPLOYEE.Email = employee.Email;
                eMPLOYEE.City = employee.City;
                eMPLOYEE.Ward = employee.Ward;
                eMPLOYEE.Street = employee.Street;
                eMPLOYEE.Dictrict = employee.Dictrict;
                eMPLOYEE.RoomID = employee.RoomID;
                eMPLOYEE.PositionID = employee.PositionID;
                eMPLOYEE.ContractID = employee.ContractID;
                eMPLOYEE.FreeInsurance = Convert.ToBoolean(employee.HealthInsurance);
                eMPLOYEE.HealthInsuranceID = employee.HealthInsuranceID;
                eMPLOYEE.SelfDeduction = Convert.ToBoolean(employee.DeductionPersonal);
                eMPLOYEE.DependentDeduction = employee.DeductionDependent;
                eMPLOYEE.State = true;
                //Trinh do
                EDUCATIONDETAIL eDUCATIONDETAIL = new EDUCATIONDETAIL();
                eDUCATIONDETAIL.EmployeeID = id;
                eDUCATIONDETAIL.EducationName = employee.EducationName;
                if (eDUCATIONDETAIL.EducationName == "10" || eDUCATIONDETAIL.EducationName == "11" || eDUCATIONDETAIL.EducationName == "12")
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
                cERTIFICATEDETAIL.TypeCertificate = employee.TypeCertificate;
                db.CERTIFICATEDETAILs.Add(cERTIFICATEDETAIL);
                //Hop dong
                CONTRACT cONTRACT = new CONTRACT();
                cONTRACT.ContractID = employee.ContractID;
                cONTRACT.ContractType = employee.ContractType;
                cONTRACT.DateStartWork = (DateTime)employee.DateStartWork;
                cONTRACT.ContractExpirationDate = (DateTime)employee.ContractExpirationDate;
                cONTRACT.BasicSalary = employee.BasicSalary;
                //Tai khoan
                USER uSER = new USER();
                uSER.EmployeeID = id;
                uSER.Password = employee.Password;
                db.CONTRACTs.Add(cONTRACT);
                db.EMPLOYEEs.Add(eMPLOYEE);
                db.USERS.Add(uSER);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(employee);
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
            List<SelectListItem> education = new List<SelectListItem>();
            List<SelectListItem> typecertificate = new List<SelectListItem>();
            var Directory = AppDomain.CurrentDomain.BaseDirectory;
            //path of folder
            var path = Directory + "./File_Text/";
            var encoding = Encoding.UTF8;
            string[] lines1 = System.IO.File.ReadAllLines(path + "City.txt", encoding);
            string[] lines2 = System.IO.File.ReadAllLines(path + "Ward.txt", encoding);
            string[] lines3 = System.IO.File.ReadAllLines(path + "Dictrict.txt", encoding);
            string[] lines4 = System.IO.File.ReadAllLines(path + "Nation.txt", encoding);
            string[] lines5 = System.IO.File.ReadAllLines(path + "ContractType.txt", encoding);
            string[] lines6 = System.IO.File.ReadAllLines(path + "Education.txt", encoding);
            string[] lines7 = System.IO.File.ReadAllLines(path + "TypeCertificate.txt", encoding);
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
                education.Add(new SelectListItem()
                {
                    Text = line,
                    Value = line
                });
            }
            foreach (string line in lines7)
            {
                typecertificate.Add(new SelectListItem()
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
            SelectList educationlist = new SelectList(education, "Value", "Text");
            SelectList certificatelist = new SelectList(typecertificate, "Value", "Text");
            // Set vào ViewBag
            ViewBag.Birthplace = citylist;
            ViewBag.HomeTown = citylist;
            ViewBag.nationList = nationlist;
            ViewBag.ContractType = contracttypelist;
            ViewBag.City = citylist;
            ViewBag.Ward = wardlist;
            ViewBag.Dictrict = dictrictlist;
            ViewBag.EducationName = educationlist;
            ViewBag.PositionID = new SelectList(db.POSITIONs, "PositionID", "PositionName");
            ViewBag.RoomID = new SelectList(db.ROOMs, "RoomID", "RoomName");
            ViewBag.MajorID = new SelectList(db.MAJORs, "MajorID", "MajorName");
            ViewBag.TypeCertificate = certificatelist;

            employee.EmployeeID = eMPLOYEE.EmployeeID;
            employee.EmployeeName = eMPLOYEE.EmployeeName;
            employee.Image = eMPLOYEE.Image;
            employee.Sex = eMPLOYEE.Sex;
            employee.DoB = eMPLOYEE.DoB;
            employee.Birthplace = eMPLOYEE.Birthplace;
            employee.HomeTown = eMPLOYEE.HomeTown;
            employee.Nation = eMPLOYEE.Nation;
            employee.IdNumber = eMPLOYEE.IdNumber;
            employee.Phone = eMPLOYEE.Phone;
            employee.Email = eMPLOYEE.Email;
            employee.City = eMPLOYEE.City;
            employee.Ward = eMPLOYEE.Ward;
            employee.Dictrict = eMPLOYEE.Dictrict;
            employee.Street = eMPLOYEE.Street;
            employee.RoomID = eMPLOYEE.RoomID;
            employee.PositionID = eMPLOYEE.PositionID;
            employee.ContractID = eMPLOYEE.ContractID;
            employee.HealthInsurance = eMPLOYEE.FreeInsurance;
            employee.HealthInsuranceID = eMPLOYEE.HealthInsuranceID;
            employee.DeductionPersonal = eMPLOYEE.SelfDeduction;
            employee.DeductionDependent = (int)eMPLOYEE.DependentDeduction;
            List<EDUCATIONDETAIL> eDUCATIONDETAIL = db.EDUCATIONDETAILs.SqlQuery("Select * from EDUCATIONDETAIL where employeeID = '" + id + "'").ToList();
            CONTRACT cONTRACT = await db.CONTRACTs.FindAsync(eMPLOYEE.ContractID);
            List<CERTIFICATEDETAIL> cERTIFICATEDETAIL = db.CERTIFICATEDETAILs.SqlQuery("Select * from CERTIFICATEDETAIL where employeeID = '" + id + "'").ToList();
            USER uSER = await db.USERS.FindAsync(id);
            //Trinh do
            employee.EducationName = eDUCATIONDETAIL[0].EducationName;
            employee.MajorID = eDUCATIONDETAIL[0].MajorID;
            employee.Date = eDUCATIONDETAIL[0].Date;
            employee.Place = eDUCATIONDETAIL[0].Place;
            //Chung chi
            employee.CertificateName = cERTIFICATEDETAIL[0].CertificateName;
            employee.TypeCertificate = cERTIFICATEDETAIL[0].TypeCertificate;
            employee.CertificateDate = cERTIFICATEDETAIL[0].CertificateDate;
            employee.CertificatePlace = cERTIFICATEDETAIL[0].CertificatePlace;
            //Hop dong
            employee.ContractID = cONTRACT.ContractID;
            employee.ContractType = cONTRACT.ContractType;
            employee.DateStartWork = cONTRACT.DateStartWork;
            employee.ContractExpirationDate = cONTRACT.ContractExpirationDate;
            employee.BasicSalary = cONTRACT.BasicSalary;
            //Tai khoan
            employee.Password = uSER.Password;
            return View(employee);
        }

        // POST: EMPLOYEEs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EmployeeID,EmployeeName,Image,Sex,DoB,Birthplace,HomeTown,Nation,IdNumber,Phone,Email,City,Ward,Dictrict,Street,RoomID,PositionID,ContractID,HealthInsurance,HealthInsuranceID,DeductionPersonal,DeductionDependent,EducationName,MajorID,Date,Place,CertificateName,TypeCertificate,CertificateDate,CertificatePlace,ContractID,ContractType,DateStartWork,ContractExpirationDate,BasicSalary,Password")] EmployeeViewModel employee)
        {
            List<SelectListItem> city = new List<SelectListItem>();
            List<SelectListItem> ward = new List<SelectListItem>();
            List<SelectListItem> dictrict = new List<SelectListItem>();
            List<SelectListItem> nation = new List<SelectListItem>();
            List<SelectListItem> contractType = new List<SelectListItem>();
            List<SelectListItem> education = new List<SelectListItem>();
            List<SelectListItem> typecertificate = new List<SelectListItem>();
            var Directory = AppDomain.CurrentDomain.BaseDirectory;
            //path of folder
            var path = Directory + "./File_Text/";
            var encoding = Encoding.UTF8;
            string[] lines1 = System.IO.File.ReadAllLines(path + "City.txt", encoding);
            string[] lines2 = System.IO.File.ReadAllLines(path + "Ward.txt", encoding);
            string[] lines3 = System.IO.File.ReadAllLines(path + "Dictrict.txt", encoding);
            string[] lines4 = System.IO.File.ReadAllLines(path + "Nation.txt", encoding);
            string[] lines5 = System.IO.File.ReadAllLines(path + "ContractType.txt", encoding);
            string[] lines6 = System.IO.File.ReadAllLines(path + "Education.txt", encoding);
            string[] lines7 = System.IO.File.ReadAllLines(path + "TypeCertificate.txt", encoding);
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
                education.Add(new SelectListItem()
                {
                    Text = line,
                    Value = line
                });
            }
            foreach (string line in lines7)
            {
                typecertificate.Add(new SelectListItem()
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
            SelectList educationlist = new SelectList(education, "Value", "Text");
            SelectList certificatelist = new SelectList(typecertificate, "Value", "Text");
            // Set vào ViewBag
            ViewBag.Birthplace = citylist;
            ViewBag.HomeTown = citylist;
            ViewBag.nationList = nationlist;
            ViewBag.ContractType = contracttypelist;
            ViewBag.City = citylist;
            ViewBag.Ward = wardlist;
            ViewBag.Dictrict = dictrictlist;
            ViewBag.EducationName = educationlist;
            ViewBag.PositionID = new SelectList(db.POSITIONs, "PositionID", "PositionName");
            ViewBag.RoomID = new SelectList(db.ROOMs, "RoomID", "RoomName");
            ViewBag.MajorID = new SelectList(db.MAJORs, "MajorID", "MajorName");
            ViewBag.TypeCertificate = certificatelist;
            if (employee.ContractExpirationDate < employee.DateStartWork)
                ModelState.AddModelError("ContractExpirationDate", "Ngày kết thúc phải lớn hơn ngày bắt đầu");
            var Parameter = db.PARAMETERs.ToList();
            var tuoi = DateTime.Now.Year - employee.DoB.Year;
            if (employee.Sex == "Nam")
            {
                if (tuoi < Parameter[19].Value) //tuoi toi thieu nam
                    ModelState.AddModelError("DoB", "Tuổi không hợp lệ");
                if (tuoi > Parameter[17].Value) //tuoi toi da nam
                    ModelState.AddModelError("DoB", "Tuổi không hợp lệ");
            }
            else
            {
                if (tuoi < Parameter[17].Value) //tuoi toi thieu nu
                    ModelState.AddModelError("DoB", "Tuổi không hợp lệ");
                if (tuoi > Parameter[18].Value) //tuoi toi da nu
                    ModelState.AddModelError("DoB", "Tuổi không hợp lệ");
            }
            if (ModelState.IsValid)
            {
                EMPLOYEE eMPLOYEE = new EMPLOYEE();
                eMPLOYEE.EmployeeID = employee.EmployeeID;
                eMPLOYEE.EmployeeName = FormatProperCase(employee.EmployeeName); //Chuan hoa chuoi
                if (employee.ImageFile != null)
                    employee.ImageFile.SaveAs(Server.MapPath("/images") + "/" + employee.EmployeeID + ".jpg");
                eMPLOYEE.Sex = employee.Sex;
                eMPLOYEE.DoB = (DateTime)employee.DoB;
                eMPLOYEE.Birthplace = employee.Birthplace;
                eMPLOYEE.HomeTown = employee.HomeTown;
                eMPLOYEE.Nation = employee.Nation;
                eMPLOYEE.IdNumber = employee.IdNumber;
                eMPLOYEE.Phone = employee.Phone;
                eMPLOYEE.Email = employee.Email;
                eMPLOYEE.City = employee.City;
                eMPLOYEE.Ward = employee.Ward;
                eMPLOYEE.Dictrict = employee.Dictrict;
                eMPLOYEE.Street = employee.Street;
                eMPLOYEE.RoomID = employee.RoomID;
                eMPLOYEE.PositionID = employee.PositionID;
                eMPLOYEE.ContractID = employee.ContractID;
                eMPLOYEE.FreeInsurance = Convert.ToBoolean(employee.HealthInsurance);
                eMPLOYEE.HealthInsuranceID = employee.HealthInsuranceID;
                eMPLOYEE.SelfDeduction = Convert.ToBoolean(employee.DeductionPersonal);
                eMPLOYEE.DependentDeduction = employee.DeductionDependent;
                db.Entry(eMPLOYEE).State = EntityState.Modified;
                //Trinh do
                EDUCATIONDETAIL eDUCATIONDETAIL = new EDUCATIONDETAIL();
                eDUCATIONDETAIL.EmployeeID = employee.EmployeeID;
                eDUCATIONDETAIL.EducationName = employee.EducationName;
                if (eDUCATIONDETAIL.EducationName == "10" || eDUCATIONDETAIL.EducationName == "11" || eDUCATIONDETAIL.EducationName == "12")
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
                cERTIFICATEDETAIL.TypeCertificate = employee.TypeCertificate;

                //Hop dong
                CONTRACT cONTRACT = new CONTRACT();
                cONTRACT.ContractID = employee.ContractID;
                cONTRACT.ContractType = employee.ContractType;
                cONTRACT.DateStartWork = (DateTime)employee.DateStartWork;
                cONTRACT.ContractExpirationDate = (DateTime)employee.ContractExpirationDate;
                cONTRACT.BasicSalary = employee.BasicSalary;
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
            eMPLOYEE.State = false;
            db.Entry(eMPLOYEE).State = EntityState.Modified;
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

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
    }
}
