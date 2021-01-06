using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using LinqToExcel;
using System.Data.SqlClient;
using HRM.Models;
namespace HRM.Controllers
{
    public class TIMEKEEPINGREPORTsController : Controller
    {
        private hrmserver_HRMEntities db = new hrmserver_HRMEntities();

        // GET: TIMEKEEPINGREPORTs
        public ActionResult Index(/*DateTime date*/)
        {
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            var tIMEKEEPINGREPORTs = db.TIMEKEEPINGREPORTs.Where(x => x.Month == date).ToList();
            //var tIMEKEEPINGREPORTs = db.TIMEKEEPINGREPORTs.Include(t => t.EMPLOYEE);
            return View(tIMEKEEPINGREPORTs);
        }
        [HttpPost]
        public ActionResult Index(TIMEKEEPINGREPORT rp)
        {
            var tIMEKEEPINGREPORTs = db.TIMEKEEPINGREPORTs.Where(x => x.Month == rp.Month).ToList();
            if (tIMEKEEPINGREPORTs == null)
            {
                return View();
            }
            return View(tIMEKEEPINGREPORTs);
        }
        // GET: TIMEKEEPINGREPORTs
        [HttpPost]
        public ActionResult UploadExcel(TIMEKEEPINGREPORT timekeepingsRP, HttpPostedFileBase FileUpload)
        {
            List<string> data = new List<string>();
            if (FileUpload != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string filename = FileUpload.FileName;
                    string targetpath = Server.MapPath("~/Doc/");
                    FileUpload.SaveAs(targetpath + filename);
                    string pathToExcelFile = targetpath + filename;
                    var connectionString = "";
                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                    }

                    var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                    var ds = new DataSet();

                    adapter.Fill(ds, "ExcelTable");

                    DataTable dtable = ds.Tables["ExcelTable"];

                    string sheetName = "Sheet1";

                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var artistAlbums = from a in excelFile.Worksheet<TIMEKEEPINGREPORT>(sheetName) select a;
                    //DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 12, 0, 0, 0, 0);
                    foreach (var a in artistAlbums)
                    {
                        if(db.TIMEKEEPINGREPORTs.Count(x => x.EmployeeID == a.EmployeeID && x.Month == timekeepingsRP.Month) == 0)
						{
                            try
                            {
                                if (a.EmployeeID != null)
                                {
                                    TIMEKEEPINGREPORT TU = new TIMEKEEPINGREPORT();
                                    TU.Month = timekeepingsRP.Month;
                                    TU.EmployeeID = a.EmployeeID;
                                    TU.SumWorkDay = a.SumWorkDay;
                                    TU.SumAbsentHaveSalary = a.SumAbsentHaveSalary;
                                    TU.SumAbsentNoSalary = a.SumAbsentNoSalary;
                                    TU.SumHourNormal = a.SumHourNormal;
                                    TU.SumHourDayOff = a.SumHourDayOff;
                                    TU.SumHourSpecialDayOff = a.SumHourSpecialDayOff;
                                    TU.SumHourNightSpecialDayOff = a.SumHourNightSpecialDayOff;
                                    //TU.SumHourNightSpecialDayOffExtra = a.SumHourNightSpecialDayOff;
                                    TU.SumHourNightNormal = a.SumHourNightNormal;
                                    //TU.SumHourNightNormalExtra = a.SumHourNightNormalExtra;
                                    TU.SumHourNightDayOff = a.SumHourNightDayOff;
                                    //TU.SumHourNightDayOffExtra = a.SumHourNightDayOffExtra;
                                    db.TIMEKEEPINGREPORTs.Add(TU);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    break;
                                }
                            }
                            catch (DbEntityValidationException ex)
                            {
                                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                                {
                                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                                    {
                                        Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                                    }
                                }
                            }
                        }
						else
						{
                            continue;
						}
                    }
                    //deleting excel file from folder  
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Format file không đúng');</script>");
                }
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Chưa có file');</script>");
            }

        }
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
