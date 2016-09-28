using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TransferDesk.Services.Manuscript;
using TransferDesk.Services.Manuscript.ViewModel;
using TransferDesk.Contracts.Manuscript.Repositories;
using TransferDesk.DAL.Manuscript.Repositories;
using System;
using TransferDesk.Contracts.Manuscript.Entities;
using Newtonsoft.Json;
using System.Configuration;
using System.Data;
using System.Diagnostics.PerformanceData;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Web.UI;
using DTOs = TransferDesk.Contracts.Manuscript.DTO;
using System.Web.UI.WebControls;
using TransferDesk.Contracts.Logging;
namespace TransferDesk.MS.Web.Controllers
{
    public class AdminDashboardController : Controller
    {
        private AdminDasboardVM adminDasboardVM;
        private AdminDashBoardService adminDashBoardService;
        private AdminDashBoardReposistory _adminDashBoardReposistory;
        private readonly ManuscriptDBRepositoryReadSide _manuscriptDBRepositoryReadSide;
        private ILogger _logger;
        public AdminDashboardController(ILogger logger)
        {
            _logger = logger;
            string conString = string.Empty;
            conString = Convert.ToString(ConfigurationManager.AppSettings["dbTransferDeskService"]);
            adminDasboardVM = new AdminDasboardVM();
            adminDashBoardService = new AdminDashBoardService(conString);
            _adminDashBoardReposistory = new AdminDashBoardReposistory(conString);
            _manuscriptDBRepositoryReadSide = new ManuscriptDBRepositoryReadSide(conString);
        }

        [HttpGet]
        public ActionResult AdminDashBoard()
        {
            adminDasboardVM.jobsdetails = _adminDashBoardReposistory.pr_GetAllJobsDetails();
            return View(adminDasboardVM);
        }

        [HttpGet]
        public JsonResult GetAdmininDashboardJson()
        {
            adminDasboardVM.jobsdetails = _adminDashBoardReposistory.pr_GetAllJobsDetails();
            return Json(adminDasboardVM, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool AllocateManuscriptToUser(AdminFunctionalityEnum[] adminFunctionalityEnum)
        {

            DataTable table = new DataTable();
            table.Columns.Add("ReceivedDate", typeof(DateTime));
            table.Columns.Add("JournalTitle", typeof(string));
            table.Columns.Add("Msid", typeof(string));
            bool flg = true; string strmsids = ""; string associateUserName = "";
            var adminUserId = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");            //On-Hold and allocate functionality
            foreach (var item in adminFunctionalityEnum)
            {
                _logger.Log(item.JobType + " job is allocating to " + item.AssociateNameVM + " with id" + Convert.ToString(item.CrestIdVM));
                var adminDash = adminDashBoardService.CreateAdminDasboardVm(item);
                if (adminDashBoardService.AllocateManuscriptToUser(adminDash))
                {
                    _logger.Log(item.JobType + " job is allocating to " + item.AssociateNameVM + " with id" + Convert.ToString(item.CrestIdVM));
                    if (item.JobType == "Journal" && item.ServiceTypeVM == "Manuscript Screening")
                    {
                        associateUserName =Convert.ToString(item.AssociateNameVM);
                        DataRow row = table.NewRow();
                        row["ReceivedDate"] = item.ReceivedDate;
                        row["JournalTitle"] = item.JournalTitle;
                        row["Msid"] = item.Msid;
                        table.Rows.Add(row);
                        strmsids += item.Msid + " ,";
                    }
                }
                else
                {
                    flg = false;
                    _logger.Log(item.JobType + " job is failed allocate to the " + item.AssociateNameVM + " with id" + Convert.ToString(item.CrestIdVM));

                }
            }
            if (flg == true)
            {
                Dictionary<String, String> dicReplace = new Dictionary<String, String>();
                adminDashBoardService.GetMailDetails(dicReplace, adminUserId, associateUserName.ToLower().Trim());
                if (Convert.ToString(dicReplace["[adminUserEmail]"]) != "" &&
                       Convert.ToString(dicReplace["[associateQaEmail]"]) != "")
                {
                    SendMail(associateUserName, table, "~/EmailTemplate/MSscreeningMulJobsAlloFor_Journals.html",
                        "Manuscript ID/s allocated for MS Screening : " + strmsids.TrimEnd(','),
                        Convert.ToString(dicReplace["[adminUserEmail]"]), Convert.ToString(dicReplace["[associateQaEmail]"]),
                        Convert.ToString(dicReplace["[adminUserEmail]"]), "", Convert.ToString(dicReplace["[adminUserName]"]));
                }

            }

            return flg;
        }

        [HttpPost]
        public bool UnallocateManuscriptFromUser(AdminFunctionalityEnum[] adminFunctionalityEnum)
        {
            bool flag = false;
            //Un allocate functionality
            foreach (var item in adminFunctionalityEnum)
            {
                _logger.Log(item.JobType + " job is Unallocating from " + item.AssociateNameVM + " with id" + Convert.ToString(item.CrestIdVM));
                var adminDash = adminDashBoardService.CreateAdminDasboardVm(item);
                if (adminDashBoardService.UnallocateManuscriptFromUser(adminDash))
                {
                    _logger.Log(item.JobType + " job is unallocated from " + item.AssociateNameVM + " with id" +
                                Convert.ToString(item.CrestIdVM));
                    flag = true;
                }
                else
                {
                    _logger.Log(item.JobType + " job is failed to unallocate from " + item.AssociateNameVM + " with id" +
                                Convert.ToString(item.CrestIdVM));
                    flag = false;
                }
            }
            return flag;
        }
        [HttpPost]
        public bool OnHoldManuscript(AdminFunctionalityEnum[] adminFunctionalityEnum)
        {
            bool flag = false;
            //Hold functionality
            foreach (var item in adminFunctionalityEnum)
            {
                var adminDash = adminDashBoardService.CreateAdminDasboardVm(item);
                if (adminDashBoardService.OnHoldManuscript(adminDash))
                {
                    _logger.Log(item.JobType + " job is successfully on hold with id" + Convert.ToString(item.CrestIdVM));
                    flag = true;
                }
                else
                {
                    _logger.Log(item.JobType + " job is failed to on hold with id" + Convert.ToString(item.CrestIdVM));
                    flag = false;
                }
            }
            return flag;
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetAssociateName(string searchAssociate, string RoleName)
        {
            return this.Json(_adminDashBoardReposistory.GetAssociateResult(searchAssociate, RoleName), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string GetLastAssociateName(int crestid, string servicetype, string JobType)
        {
            var LastAssociateName = _adminDashBoardReposistory.GetLastNameOfAssociate_ForHoldJob(crestid, servicetype, JobType);
            if (LastAssociateName.Count > 0)
            {
                foreach (var item in LastAssociateName)
                {
                    return item.empname;
                }
            }
            return null;

        }

        public void SendMail(string associateUserName, DataTable dtDetails, String emailTemplatePath, String emailSubject, String emailFrom, String emailTo, String emailCC, String emailBCC, string adminUserName)
        {
            emailTemplatePath = Server.MapPath(emailTemplatePath);
            StringBuilder emailBody = new StringBuilder(System.IO.File.ReadAllText(emailTemplatePath));

            emailBody = emailBody.Replace("[Details]", ReplaceString(dtDetails)).Replace("[AnalystName]", associateUserName).Replace("[adminUserName]",adminUserName);
            //foreach (KeyValuePair<String, String> kvp in dicReplace)
            //{
            //    emailBody.Replace(kvp.Key, kvp.Value);
            //}
            Email objEmail = new Email();
            objEmail.SendEmail(emailTo, emailFrom, emailCC, emailBCC, emailSubject, Convert.ToString(emailBody));

        }
        private string ReplaceString(DataTable dtReplace)
        {
            string replaceAllData = "";
            if (dtReplace.Rows.Count > 0)
            {
                for (int count = 0; dtReplace.Rows.Count > count; count++)
                {
                    string replaceData = "<tr><td>" + Convert.ToDateTime(dtReplace.Rows[count][0]).ToString("dd-MM-yyyy") + "</td><td>" + Convert.ToString(dtReplace.Rows[count][1]) + "</td><td>" + Convert.ToString(dtReplace.Rows[count][2]) + "</td></tr>";
                    replaceAllData = replaceAllData + replaceData;
                }
            }
            return replaceAllData;
        }




        public ActionResult AdminDashBoardExportToExcelData(string FromDate, string ToDate)
        {

            if (FromDate == "" || ToDate == "")
            {
                TempData["msg"] = "<script>alert('Please select Date');</script>";
                return RedirectToAction("AdminDashBoard");
            }

            var grid = new GridView();
            var countData = _adminDashBoardReposistory.GetAdminDashBoardExportToExcel(FromDate, ToDate).ToList().Count;
            if (countData > 0)
            {
                grid.DataSource = _adminDashBoardReposistory.GetAdminDashBoardExportToExcel(FromDate, ToDate);
                grid.DataBind();
                grid.HeaderStyle.Font.Bold = true;
                grid.HeaderRow.BackColor = System.Drawing.Color.LightGray;
                grid.HeaderRow.Cells[0].Text = "Sr. No.";
                grid.HeaderRow.Cells[1].Text = "Crest ID";
                grid.HeaderRow.Cells[2].Text = "Job Type";
                grid.HeaderRow.Cells[3].Text = "Service Type";
                grid.HeaderRow.Cells[4].Text = "MSID/Chp No.";
                grid.HeaderRow.Cells[5].Text = "Journal Name/Book title";
                grid.HeaderRow.Cells[6].Text = "PageCount";
                grid.HeaderRow.Cells[7].Text = "Name";
                grid.HeaderRow.Cells[8].Text = "Role";
                grid.HeaderRow.Cells[9].Text = "Status";
                grid.HeaderRow.Cells[10].Text = "Task";
                grid.HeaderRow.Cells[11].Text = "Revision No.";
                grid.HeaderRow.Cells[12].Text = "Group No.";
                grid.HeaderRow.Cells[13].Text = "Received Date";
                grid.HeaderRow.Cells[14].Text = "Logged in date/time";
                grid.HeaderRow.Cells[15].Text = "Fetch date/time";
                grid.HeaderRow.Cells[16].Text = "Age (days)";
                grid.HeaderRow.Cells[17].Text = "Handling time";
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition",
                    string.Format("attachment; filename={0}", "ManuscriptAdminDashBoard" + DateTime.Now.ToShortDateString() + ".xls"));
                Response.ContentType = "application/ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                grid.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.Flush();
                Response.End();
                return View();
            }
            else
            {
                TempData["msg"] = "<script>alert('No Record Found');</script>";
                return RedirectToAction("AdminDashBoard");
            }
        }
    }

}