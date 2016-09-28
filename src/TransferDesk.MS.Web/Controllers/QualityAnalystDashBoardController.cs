using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TransferDesk.Contracts.Logging;
using TransferDesk.DAL.Manuscript.Repositories;
using TransferDesk.Services.Manuscript.ViewModel;

namespace TransferDesk.MS.Web.Controllers
{
    public class QualityAnalystDashBoardController : Controller
    {
       
        // GET: /QualityAnalystDashBoard/

        private readonly ManuscriptDBRepositoryReadSide _manuscriptDbRepositoryReadSide;
        private QualityAnalystDashBoardVM qualityAnalystDashBoardVm;
        private QualityAnalystDashBoardReposistory _qualityanalystreposistory;
        public ILogger _logger;

        public QualityAnalystDashBoardController(ILogger logger)
        {
            _logger = logger;
            var conString = Convert.ToString(ConfigurationManager.AppSettings["dbTransferDeskService"]);
            _manuscriptDbRepositoryReadSide = new ManuscriptDBRepositoryReadSide(conString);
            qualityAnalystDashBoardVm = new QualityAnalystDashBoardVM();
            _qualityanalystreposistory = new QualityAnalystDashBoardReposistory(conString);    
        }
        [HttpGet]
        public ActionResult QualityAnalystDashboard()
        {
            
            var userId = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            qualityAnalystDashBoardVm.specificQualityAnalystdetails = _qualityanalystreposistory.pr_GetAllQualityAnalystAssignedJobs(userId, 5);
            qualityAnalystDashBoardVm.MsPendingJobs =Convert.ToString( _qualityanalystreposistory.TotalQualityMSPendingJobs());
            qualityAnalystDashBoardVm.RsPendingJobs = Convert.ToString(_qualityanalystreposistory.TotalQualityRSPendingJobs());         
            int serviceTypeId = _qualityanalystreposistory.GetServiceTypeOnUserId(userId);
            qualityAnalystDashBoardVm.specificQualityAnalystdetails = _qualityanalystreposistory.pr_GetAllQualityAnalystAssignedJobs(userId, serviceTypeId);
            
            return View(qualityAnalystDashBoardVm);
        }

        public bool IsJobFetched(string userId, int serviceTypeId)
        {
            var fetchedJobCount = _qualityanalystreposistory.IsJobFetchedByQuality(userId, serviceTypeId);
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
                string MSID = _qualityanalystreposistory.GetMSIDOnCrestId(crestID);
                int ManuscriptID = _qualityanalystreposistory.GetManuscriptIDOnMSID(MSID, crestID);
                qualityAnalystDashBoardVm.specificQualityAnalystdetails = _qualityanalystreposistory.pr_GetAllQualityAnalystAssignedJobs(userId,5);
                _logger.Log(" MSID Get : " + MSID + " " + userId);
                var jdata = new { ManuscriptID = ManuscriptID, returnValue = "true", jobType = "" };
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
                int serviceTypeId = _qualityanalystreposistory.GetServiceTypeOnUserId(userId);
                _logger.Log(" Find service type of user: " + userId);
                if (IsJobFetched(userId, serviceTypeId))
                {
                    _logger.Log(" Job fetching started: " + userId);
                    var fetchedJobs = _qualityanalystreposistory.JobTobeFetched(userId, serviceTypeId, 0);
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
                        qualityAnalystDashBoardVm.specificQualityAnalystdetails= _qualityanalystreposistory.GetQualityFetchedJobs(fetchedJobs.CrestID, serviceTypeId, 0);
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