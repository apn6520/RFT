using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TransferDesk.Contracts.Logging;
using TransferDesk.Contracts.Manuscript.Entities;
using TransferDesk.DAL.Manuscript;
using TransferDesk.DAL.Manuscript.DataContext;
using TransferDesk.DAL.Manuscript.Repositories;
using TransferDesk.Services.Manuscript;
using TransferDesk.Services.Manuscript.ViewModel;

namespace TransferDesk.MS.Web.Controllers
{
    public class UserRoleController : Controller
    {
        private UserRoleRepository _UserRoleRepository;
		private JournalUserReposistory _journalUserRoles;
        private BookUserReposistory _bookUserRoles;
        private UserRoleVM userRoleVM;
        private ManuscriptDBRepositoryReadSide _manuscriptDBRepositoryReadSide;
		private readonly UserRoleService _userRoleService;
        private readonly string _conString;
        private string _errormsg = String.Empty;
        private ILogger _logger;
        public UserRoleController(ILogger logger)
        {
			_logger = logger;
            string conString = Convert.ToString(ConfigurationManager.AppSettings["dbTransferDeskService"]);
            //ManuscriptContext  
            this._UserRoleRepository = new UserRoleRepository((new ManuscriptDBContext(conString)));
            _manuscriptDBRepositoryReadSide = new ManuscriptDBRepositoryReadSide(conString);
            userRoleVM = new UserRoleVM();
			_userRoleService = new UserRoleService(conString);
            _journalUserRoles = new JournalUserReposistory(conString);
            _bookUserRoles = new BookUserReposistory(conString);
        }
        // GET: UserRole
        public ActionResult UserRole()
        {
            int id = 0;
            return RedirectToAction("UserMaster", id = 0);
            var userID = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            if(_UserRoleRepository.IsAdmin(userID.Trim()))
            {
                userRoleVM.Role = _UserRoleRepository.GetRoleByUserID(userID);
                userRoleVM.userRoles = _UserRoleRepository.GetUserRoleDetails();
                userRoleVM.EmployeeName = _manuscriptDBRepositoryReadSide.EmployeeName(userID);
                return View(userRoleVM);
            }
            else
                return File("~/Views/Shared/Unauthorised.htm", "text/html");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserRole(UserRoleVM userRoleVM)
        {
            int id = 0;
            return RedirectToAction("UserMaster", id = 0);
            if (ModelState.IsValid)
            {
                userRoleVM.UserID = userRoleVM.UserID.Trim();
                if (userRoleVM.ID == 0)
                {
                    if (_UserRoleRepository.IsUserIDAvailable(userRoleVM.UserID))
                    {
                        if (!_UserRoleRepository.IsUserRoleAvailable(userRoleVM.ID, userRoleVM.RollID, userRoleVM.UserID))
                        {
                            var _UserRole = new UserRoles();
                            _UserRole.ID = userRoleVM.ID;
                            _UserRole.IsActive = true;
                            _UserRole.UserID = userRoleVM.UserID;
                            _UserRole.RollID = userRoleVM.RollID;
                            _UserRole.DefaultRollID = userRoleVM.RollID;
                            _UserRole.Status = 1;
                            _UserRole.ModifiedDateTime = System.DateTime.Now;
                            _UserRoleRepository.AddUserRole(_UserRole);
                            _UserRoleRepository.SaveUserRole();
                            TempData["msg"] = "<script>alert('Record added succesfully');</script>";
                            return RedirectToAction("UserRole", "UserRole", userRoleVM);
                        }
                        else
                        {
                            TempData["msg"] = "<script>alert('Record is already present.');</script>";
                        }
                    }
                    else
                    {
                        TempData["msg"] = "<script>alert('User ID is not present in active directory.');</script>";
                    }
                }
                else
                {
                    if (_UserRoleRepository.IsUserIDAvailable(userRoleVM.UserID))
                    {
                        if (!_UserRoleRepository.IsUserRoleAvailable(userRoleVM.ID, userRoleVM.RollID, userRoleVM.UserID))
                        {
                            var _UserRole = new UserRoles();
                            _UserRole.IsActive = userRoleVM.IsActive;
                            _UserRole.UserID = userRoleVM.UserID;
                            _UserRole.RollID = userRoleVM.RollID;
                            _UserRole.ID = userRoleVM.ID;
                            _UserRole.DefaultRollID = userRoleVM.RollID;
                            _UserRole.Status = 1;
                            _UserRole.ModifiedDateTime = System.DateTime.Now;
                            _UserRoleRepository.UpdateUserRole(_UserRole);
                            _UserRoleRepository.SaveUserRole();
                            TempData["msg"] = "<script>alert('Record updated succesfully');</script>";
                            return RedirectToAction("UserRole", "UserRole", userRoleVM);
                        }
                        else
                        {
                            TempData["msg"] = "<script>alert('Record is already present.');</script>";
                        }
                    }
                    else
                    {
                        TempData["msg"] = "<script>alert('User ID is not present in active directory.');</script>";
                    }
                }
            }

            var userID = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            userRoleVM.Role = _UserRoleRepository.GetRoleByUserID(userID);
            userRoleVM.userRoles = _UserRoleRepository.GetUserRoleDetails();
            userRoleVM.EmployeeName = _manuscriptDBRepositoryReadSide.EmployeeName(userID);
            return View(userRoleVM);
        }
		
		[HttpGet]
        public ActionResult UserMaster(int? id)
        {
            var userRoleVM = new UserRoleVM();
            if (id != 0 && id != null)
            {
                UserMasterDetailsOnEdit(userRoleVM, id);
            }
            var userId = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            ViewBag.RoleList = _manuscriptDBRepositoryReadSide.GetRoleList();
            userRoleVM.EmployeeName = _manuscriptDBRepositoryReadSide.EmployeeName(userId);
            userRoleVM.ServiceType = _manuscriptDBRepositoryReadSide.GetServiceType();           
            ViewBag.JournalList = _manuscriptDBRepositoryReadSide.GetJournalList();
            ViewBag.BookList = _manuscriptDBRepositoryReadSide.GetManuscriptBookTitleList();
            userRoleVM.TeamList = _manuscriptDBRepositoryReadSide.GetTeamList();
            var serviceId = Convert.ToInt32(userRoleVM.ServiceTypeID);
            ViewBag.SlidingSacleList = _UserRoleRepository.GetSlidingScaleList(serviceId);
            userRoleVM.usermasterdetailslist = _UserRoleRepository.pr_GetUserMasterDetails_Result();

            return View("UserMaster",userRoleVM);
        }


		private void UserMasterDetailsOnEdit(UserRoleVM userRoleVm, int? id)
        {
            UserRoles userrole;
            List<JournalUserRoles> journalUserRoles = new List<JournalUserRoles>();
            List<BookUserRoles> bookUserRoles = new List<BookUserRoles>();
            var userId = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            try
            {
                userrole = _UserRoleRepository.GetUserDetailsById(id);
                journalUserRoles = _journalUserRoles.GetJournalDetailsForUserID(userrole.ID);
               
                if (journalUserRoles.Count == 0)
                {
                    userRoleVm.IsJournal = false;
                    userRoleVm.SelectedJournalIDs = null;
                }
                else
                {
                    userRoleVm.IsJournal = true;
                    int[] juid = new int[journalUserRoles.Count];
                    int i = 0;
                    foreach (var jid in journalUserRoles)
                    {
                        juid[i] = jid.JournalMasterId;
                        i++;
                    }
                    userRoleVm.SelectedJournalIDs = juid.ToArray().ToList();
                }
                bookUserRoles = _bookUserRoles.GetBookDetailsForUserID(userrole.ID);
                if (bookUserRoles.Count == 0)
                {
                    userRoleVm.IsBook = false;
                    userRoleVm.SelectedBookIDs = null;
                }
                else
                {
                    userRoleVm.IsBook = true;
                    int[] buid = new int[bookUserRoles.Count];
                    int j = 0;
                    foreach (var bid in bookUserRoles)
                    {
                        buid[j] = bid.BookMasterId;
                        j++;
                    }
                    userRoleVm.SelectedBookIDs = buid.ToArray().ToList();
                }
                userRoleVm.Name = _manuscriptDBRepositoryReadSide.EmployeeName(userrole.UserID);
                userRoleVm.UserID = userrole.UserID;
                userRoleVm.IsActive = userrole.IsActive;
                userRoleVm.ServiceTypeID = userrole.ServiceTypeId;
                userRoleVm.SlidingScaleId = userrole.SlidingScaleId;
                userRoleVm.RollID = userrole.RollID;
                userRoleVm.TeamId = userrole.StatusTeamId;


            }
            catch (Exception ex)
            {
                _logger.Log("Error in UserMaster during update operation: \n" + ex.StackTrace);
            }
        }

        [HttpPost]
        public ActionResult UserMaster(UserRoleVM userrolevm, List<int> SelectedJournalID, List<int> SelectedBookIDs)
        {
            var userId = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            userrolevm.loginuser = userId;
            if (userrolevm.ID == 0)
            {
                if (AddUserMasterInfo(userrolevm))
                {
                    TempData["msg"] = "<script>alert('Record added succesfully');</script>";
                }
            }
            else
            {
                if (AddUserMasterInfo(userrolevm))
                {
                    TempData["msg"] = "<script>alert('Record updated succesfully');</script>";
                }
            }
            return RedirectToAction("UserMaster", new { id = 0 });
        }

        private bool AddUserMasterInfo(UserRoleVM userrolevm)
        {
            var userRoles = new UserRoles();
            if (_UserRoleRepository.IsUserRoleAvailable(userrolevm.UserID, userrolevm.ServiceTypeID, userrolevm.RollID))
            {
                if (_UserRoleRepository.IsJobFetchedByUser(userrolevm.UserID, userrolevm.ServiceTypeID, userrolevm.RollID, userrolevm.IsActive))
                {
                    _userRoleService.SaveUserRoleDetails(userrolevm, userRoles);
                    return true;
                }
                else
                {
                    TempData["msg"] = "<script>alert('Unallocate job from user.');</script>";
                    return false;
                }
            }
            else
            {
                TempData["msg"] = "<script>alert('Associate should have only one role.');</script>";
                return false;
            }
        }
		
		[AcceptVerbs(HttpVerbs.Get)]
        public string GetSlidingScaleList(int ServiceTypeId)
        {
            var slidingscaleList = string.Empty;
            if (ServiceTypeId > 0)
            {
                var slidingscalelist = _UserRoleRepository.GetSlidingScaleList(ServiceTypeId);
                foreach (var quality in slidingscalelist)
                {
                    slidingscaleList += Convert.ToString(quality.ID) + "---" +
                                       Convert.ToString(quality.QualityCheckedPercentage) + "~";
                }
            }
            return slidingscaleList;
        }
		public string EmployeeName(string userID)
        {
            var name = _manuscriptDBRepositoryReadSide.EmployeeName(userID);
            return name;
        }
	
	}
}