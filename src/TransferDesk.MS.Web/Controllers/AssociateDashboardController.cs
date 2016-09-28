using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using TransferDesk.Contracts.Logging;
using TransferDesk.DAL.Manuscript.Repositories;
using TransferDesk.Logger;
using TransferDesk.Services.Manuscript;
using TransferDesk.Services.Manuscript.ViewModel;

namespace TransferDesk.MS.Web.Controllers
{
    public class AssociateDashboardController : Controller
    {
        //
        // GET: /AssociateDashboard/


        private readonly ManuscriptDBRepositoryReadSide _manuscriptDbRepositoryReadSide;
        private AssociateDashboardVM associateDasboardVM;
        private AssociateDashBoardReposistory _associateDashBoardReposistory;
        public ILogger _logger;

        public AssociateDashboardController(ILogger logger)
        {
            _logger = logger;
            var conString = Convert.ToString(ConfigurationManager.AppSettings["dbTransferDeskService"]);
            _manuscriptDbRepositoryReadSide = new ManuscriptDBRepositoryReadSide(conString);
            associateDasboardVM = new AssociateDashboardVM();
            _associateDashBoardReposistory = new AssociateDashBoardReposistory(conString);
        }

        [HttpGet]
        public ActionResult AssociateDashboard()
        {

            var userId = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            int serviceTypeId = _associateDashBoardReposistory.GetServiceTypeOnUserId(userId);
            associateDasboardVM.specificAssociatedetails = _associateDashBoardReposistory.pr_GetAllAssociatesAssignedJobs(userId, serviceTypeId);

            associateDasboardVM.TotalAssociateJournalMSPendingJobs =
                _associateDashBoardReposistory.TotalAssociateJournalMSPendingJobs();
            associateDasboardVM.TotalAssociateJournalRSPendingJobs =
                _associateDashBoardReposistory.TotalAssociateJournalRSPendingJobs();
            associateDasboardVM.TotalAssociateBookMSPendingJobs =
                _associateDashBoardReposistory.TotalAssociateBookMSPendingJobs();
            associateDasboardVM.TotalAssociateBookRSPendingJobs =
                _associateDashBoardReposistory.TotalAssociateBookRSPendingJobs();
            return View(associateDasboardVM);
        }

        public bool IsJobFetched(string userId, int serviceTypeId)
        {
            var fetchedJobCount = _associateDashBoardReposistory.IsJobFetched(userId, serviceTypeId);
            if (fetchedJobCount.FetchedJobCount > 0)
                return false;
            else
                return true;
        }

        public JsonResult OpenManuscript(string crestID)
        {
            string userId = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            try
            {
                _logger.Log(" I am in OpenManuscript: " + userId);
                string MSID = _associateDashBoardReposistory.GetMSIDOnCrestId(crestID);
                int ManuscriptID = _associateDashBoardReposistory.GetManuscriptIDOnMSID(MSID, crestID);
                associateDasboardVM.manuscriptsIDVM = ManuscriptID;
                _logger.Log(" MSID Get : " + MSID +" "+ userId);
                var jdata =new { ManuscriptID = ManuscriptID, returnValue = "true", jobType = crestID.StartsWith("J") ? "J" : "B" };
                return this.Json(jdata, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logger.Log(" MSID Get : " + ex + " " + userId);
                throw;
            }
        }
        
        public JsonResult FetchJob()
        {
            string userId = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            try
            {
                int serviceTypeId = _associateDashBoardReposistory.GetServiceTypeOnUserId(userId);
                _logger.Log(" Find service type of user: " + userId);
                if (IsJobFetched(userId, serviceTypeId))
                {
                    _logger.Log(" Job fetching started: " + userId);
                    var fetchedJobs = _associateDashBoardReposistory.JobTobeFetched(userId, serviceTypeId, 0);
                    if (fetchedJobs.CrestID == "" || string.IsNullOrEmpty(fetchedJobs.CrestID))
                    {
                        _logger.Log(" Job are not found: " + userId);
                        var jdata = new { message = "There are no open jobs in queue.", ManuscriptID = 0, returnValue = "false", jobType = "" };
                        return this.Json(jdata, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        _logger.Log(" Job fetched: " + userId);
                        
                        //open MS screeing form 
                        associateDasboardVM.specificAssociatedetails = _associateDashBoardReposistory.GetAssociatedFetchedJobs(fetchedJobs.CrestID, serviceTypeId, 0);
                        var jdata = new { message = "Job is fetched successfully.", ManuscriptID = "", returnValue = "true", jobType = fetchedJobs.JobType };
                        return this.Json(jdata, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    _logger.Log(" Job is already fetched: " + userId);
                    var jdata = new { message = "Job is already fetched.", ManuscriptID = 0, returnValue = "false", jobType = "" };
                    return this.Json(jdata, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(" MSID Get : " + ex + " " + userId);
                throw;
            }
        }

    }
}