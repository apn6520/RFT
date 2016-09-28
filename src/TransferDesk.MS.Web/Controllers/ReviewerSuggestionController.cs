using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TransferDesk.Services.Manuscript;
using TransferDesk.Services.Manuscript.ViewModel;
using TransferDesk.Contracts.Manuscript.Repositories;
using TransferDesk.DAL.Manuscript.Repositories;
using TransferDesk.Contracts.Manuscript.Entities;
using Newtonsoft.Json;
using System.Configuration;
using System.Text;

namespace TransferDesk.MS.Web.Controllers
{
    public class ReviewerSuggestionController : Controller
    {
        private ManuscriptDBRepositoryReadSide _manuscriptDBRepositoryReadSide;
        private ReviewerSuggestionDBRepositoryReadSide _reviewerDBRepositoryReadSide;
        private ReviewerService _reviewerService;
        public string userID;
        public ReviewerSuggestionController()
        {
            if (@System.Web.HttpContext.Current != null && @System.Web.HttpContext.Current.User != null)
            {
                userID = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            }
            string conString = string.Empty;
            conString = Convert.ToString(ConfigurationManager.AppSettings["dbTransferDeskService"]);
            _manuscriptDBRepositoryReadSide = new ManuscriptDBRepositoryReadSide(conString);
            _reviewerDBRepositoryReadSide = new ReviewerSuggestionDBRepositoryReadSide(conString);
            _reviewerService = new ReviewerService(conString, conString);
        }

        // GET: ReviewerSuggetion
        public ActionResult ReviewersSuggestions(int? id)
        {
            int[] roleIds = _manuscriptDBRepositoryReadSide.GetUserRoles(userID,6);
            if (roleIds.Count() > 0)
            {
                ViewBag.SearchList = _manuscriptDBRepositoryReadSide.GetSearchList("j");
                ViewBag.JournalList = _manuscriptDBRepositoryReadSide.GetJournalList();
                ViewBag.RoleList = _manuscriptDBRepositoryReadSide.GetUserRoleList(roleIds);
                ViewBag.TaskIDList = _reviewerDBRepositoryReadSide.GetTaskIDList();
                ReviewerSuggestionVM msReviewerSuggestionVM;
                int reviewerID = id ?? default(int);

                if (_reviewerService._reviewerSuggetionBL == null)
                {
                    _reviewerService._reviewerSuggetionBL = new BAL.Manuscript.ReviewerSuggetionBL();
                }

                if (_reviewerService._reviewerSuggetionBL.msReviewerSuggestionDBRepositoryReadSide == null)
                {
                    _reviewerService._reviewerSuggetionBL.msReviewerSuggestionDBRepositoryReadSide = _reviewerDBRepositoryReadSide;
                }
                if (reviewerID == 0)
                {
                    msReviewerSuggestionVM = _reviewerService.GetManuscriptScreeningDefaultVM();
                }
                else
                {
                    msReviewerSuggestionVM = _reviewerService.GetManuscriptScreeningVM(reviewerID);
                }
                msReviewerSuggestionVM.EmployeeName = _manuscriptDBRepositoryReadSide.EmployeeName(userID);
                return View(msReviewerSuggestionVM);
            }
            else
            {
                return File("~/Views/Shared/Unauthorised.htm", "text/html");
            }
        }

        public ActionResult UnAssignReviewer(int reviewerInfoID, int? msReviewersSuggestionID)
        {
            bool result = _reviewerService.UnAssignReviewer(reviewerInfoID, msReviewersSuggestionID);
            if (result)
            {
                Dictionary<String, String> dicReplace = new Dictionary<String, String>();
                _reviewerService.GetMailDetails(dicReplace, reviewerInfoID, msReviewersSuggestionID, userID);
                SendMail(dicReplace, "~/EmailTemplate/Unassign_Reviewer_mail_template.html", "Reviewer(s) unassigned for :" + dicReplace["[manuscriptNumber]"], Convert.ToString(dicReplace["[QAEmail]"]), Convert.ToString(dicReplace["[AnalystEmail]"]), Convert.ToString(dicReplace["[QAEmail]"]), "");
                TempData["MSIDError"] = "<script>alert('Reviewer unassigned successfully');</script>";

            }
            return RedirectToAction("ReviewersSuggestions", new { id = msReviewersSuggestionID });
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult ReviewersSuggestions(ReviewerSuggestionVM msReviewerSuggestionVM, string AssociateCommand, string QualityCommand, string[] UnAssignedReviewer, string RevertToAssociate)
        {
            IDictionary<string, string> dataErrors = new Dictionary<string, string>();
            if (ModelState.IsValid)
            {
                if (msReviewerSuggestionVM.ID == 0)
                {
                    TempData["MSIDError"] = "<script>alert('Please, contact administrators to add record.');</script>";
                }
                else
                {
                    if (_reviewerDBRepositoryReadSide.IsMSIDAvailable(msReviewerSuggestionVM.MSID, msReviewerSuggestionVM.ID))
                    {
                        TempData["MSIDError"] = "<script>alert('Manuscript Number is already present.');</script>";
                    }
                    else
                    {
                        msReviewerSuggestionVM = IsSaveOrSubmit(msReviewerSuggestionVM, AssociateCommand, QualityCommand);
                        _reviewerService.SaveMSReviewerSuggestionVM(dataErrors, msReviewerSuggestionVM);
                        TempData["MSIDError"] = "<script>alert('Record updated succesfully');</script>";
                    }
                }
            }
            string reviewerNames = String.Empty;
            //un-assign reviewer if any
            if (UnAssignedReviewer != null && RevertToAssociate != null)
            {
                foreach (var reviewerID in UnAssignedReviewer)
                {
                    _reviewerService.UnAssignReviewer(Convert.ToInt32(reviewerID), msReviewerSuggestionVM.ID);
                    var result = _reviewerDBRepositoryReadSide.GetMSReviewerInfoIDs(Convert.ToInt32(reviewerID));
                    List<TransferDesk.Contracts.Manuscript.Entities.ReviewerMaster> reviewerMasters =
                        _reviewerDBRepositoryReadSide.GetReviewerDetails(result.ReviewerMasterID);
                    reviewerNames += reviewerMasters[0].ReviewerName + ", ";

                    _reviewerService.RemoveReviewerTile(Convert.ToInt32(result.ReviewerMasterID), msReviewerSuggestionVM.ArticleTitle.Trim(), userID);

                }
                if (UnAssignedReviewer.Length > 0)
                {
                    reviewerNames = reviewerNames.Remove(reviewerNames.Length - 1);
                    Dictionary<String, String> dicReplace = new Dictionary<String, String>();
                    _reviewerService.GetMailDetails(dicReplace, Convert.ToInt32(UnAssignedReviewer[0]),
                        msReviewerSuggestionVM.ID, userID);
                    dicReplace.Add("[reviewername]", reviewerNames);
                    dicReplace.Add("[ReviewerCount]", Convert.ToString(UnAssignedReviewer.Length));
                    dicReplace.Add("[ErrorDescription]", msReviewerSuggestionVM.ErrorDescription);
                    if (Convert.ToString(dicReplace["[QAEmail]"]) != "" &&
                        Convert.ToString(dicReplace["[AnalystEmail]"]) != "")
                    {
                        SendMail(dicReplace, "~/EmailTemplate/Unassign_Reviewer_mail_template.html",
                            "Reviewer(s) unassigned for :" + dicReplace["[manuscriptNumber]"],
                            Convert.ToString(dicReplace["[QAEmail]"]), Convert.ToString(dicReplace["[AnalystEmail]"]),
                            Convert.ToString(dicReplace["[QAEmail]"]), "");
                    }
                    TempData["MSIDError"] = "<script>alert('Reviewer unassigned successfully');</script>";
                }
            }
            msReviewerSuggestionVM.EmployeeName = _manuscriptDBRepositoryReadSide.EmployeeName(userID);
            return RedirectToAction("ReviewersSuggestions", new { id = msReviewerSuggestionVM.ID });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetReviewerSearchResult(string SelectedValue, string SearchBy)
        {
            return this.Json(_reviewerDBRepositoryReadSide.GetReviewerSearchResult(SelectedValue, SearchBy), JsonRequestBehavior.AllowGet);
        }

        public ReviewerSuggestionVM IsSaveOrSubmit(ReviewerSuggestionVM msReviewerSuggestionVM, string AssociateCommand, string QualityCommand)
        {
            if (AssociateCommand != null)
            {
                if (AssociateCommand.ToLower() == "save")
                    msReviewerSuggestionVM.IsAssociateFinalSubmit = false;
                else if (AssociateCommand.ToLower() == "submit")
                {
                    msReviewerSuggestionVM.IsAssociateFinalSubmit = true;
                    msReviewerSuggestionVM.QualityEndDate = System.DateTime.Now;
                }
            }
            if (QualityCommand != null)
            {
                if (QualityCommand.ToLower() == "save")
                {
                    msReviewerSuggestionVM.IsQualityFinalSubmit = false;
                    if (msReviewerSuggestionVM.QualityStartCheckDate == null)
                    {
                        msReviewerSuggestionVM.QualityStartCheckDate = System.DateTime.Now;
                    }
                }
                else if (QualityCommand.ToLower() == "submit")
                {
                    msReviewerSuggestionVM.IsQualityFinalSubmit = true;
                    var QAStartDate = _reviewerDBRepositoryReadSide.GetStartDateOfQA(msReviewerSuggestionVM.MSID);
                    if (QAStartDate != null)
                        msReviewerSuggestionVM.QualityStartCheckDate = Convert.ToDateTime(QAStartDate);
                    else
                    {
                        msReviewerSuggestionVM.QualityStartCheckDate = null;
                    }
                    if (msReviewerSuggestionVM.QualityStartCheckDate == null)
                    {
                        msReviewerSuggestionVM.QualityStartCheckDate = System.DateTime.Now;
                    }
                    msReviewerSuggestionVM.QualityEndDate = System.DateTime.Now;
                }
            }
            return msReviewerSuggestionVM;
        }

        public void SendMail(Dictionary<String, String> dicReplace, String emailTemplatePath, String emailSubject, String emailFrom, String emailTo, String emailCC, String emailBCC)
        {
            emailTemplatePath = Server.MapPath(emailTemplatePath);
            StringBuilder emailBody = new StringBuilder(System.IO.File.ReadAllText(emailTemplatePath));
            foreach (KeyValuePair<String, String> kvp in dicReplace)
            {
                emailBody.Replace(kvp.Key, kvp.Value);
            }
            Email objEmail = new Email();
            objEmail.SendEmail(emailTo, emailFrom, emailCC, emailBCC, emailSubject, Convert.ToString(emailBody));

            //save mail details
            _reviewerService.SaveMailDetails(dicReplace, emailTo, emailFrom, emailCC, emailBCC, emailSubject, Convert.ToString(emailBody));
        }

    
    }
}