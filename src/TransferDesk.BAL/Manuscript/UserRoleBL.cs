using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.DTO;
using TransferDesk.Contracts.Manuscript.Entities;
using TransferDesk.DAL.Manuscript;
using TransferDesk.DAL.Manuscript.Repositories;
using TransferDesk.DAL.Manuscript.UnitOfWork;

namespace TransferDesk.BAL.Manuscript
{
    public class UserRoleBL
    {
        private TransferDesk.DAL.Manuscript.DataContext.ManuscriptDBContext context = null;
        public ManuscriptDBRepositoryReadSide _manuscriptDBRepositoryReadSide { get; set; }
        public UserRoleRepository _UserRoleRepository { get; set; }
        public Impersonation.Impersonate impersonate = null;
        private JournalUserReposistory _journalUserRoles;
        private BookUserReposistory _bookUserReposistory;
        string conString;

        public UserRoleBL(string conString)
        {
            this.conString = conString;
            context = new TransferDesk.DAL.Manuscript.DataContext.ManuscriptDBContext(conString);
            _manuscriptDBRepositoryReadSide = new ManuscriptDBRepositoryReadSide(conString);
            _UserRoleRepository = new UserRoleRepository(conString);
            _journalUserRoles = new JournalUserReposistory(conString);
            _bookUserReposistory=new BookUserReposistory(conString);
        }


        public bool SaveUserRoles(UserRoleDTO userRoleDto)
        {
            UserRolesUnitOfWork userRolesUnitOfWork = null;
            //   var userroleid=from q in context.Users          
            userRolesUnitOfWork = new UserRolesUnitOfWork(conString);
            userRolesUnitOfWork.userRoleDto = userRoleDto;
            userRolesUnitOfWork.SaveUserRolesDetails(userRoleDto);
            if (userRoleDto.userroles.ID != 0)
            {
                userRolesUnitOfWork.SaveUserRolesDetails(userRoleDto);
            }
            if (SaveJournalUserRolesDetails(userRoleDto, userRolesUnitOfWork) == false)
            {
                return false;
            }
            if (SaveBookUserRolesDetails(userRoleDto, userRolesUnitOfWork) == false)
            {
                return false;
            }
            return true;
        }

        private bool SaveJournalUserRolesDetails(UserRoleDTO userRoleDto, UserRolesUnitOfWork userRolesUnitOfWork)
        {           
            try
            {
                if (userRoleDto.SelectedJournalID != null || userRoleDto.SelectedJournalIDs != null)
                {
                    SaveManuscriptJournalUserRolesDetails(userRoleDto, userRolesUnitOfWork);
                }
                else
                {
                    if (DeleteFromJournalUser(userRoleDto, userRolesUnitOfWork))
                    {
                      
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception)
            {              
                return false;
            }
        }


        private bool SaveBookUserRolesDetails(UserRoleDTO userRoleDto, UserRolesUnitOfWork userRolesUnitOfWork)
        {

            try
            {
                if (userRoleDto.SelectedBookID != null || userRoleDto.SelectedBookIDs != null)
                {
                    SaveManuscriptBookUserRolesDetails(userRoleDto, userRolesUnitOfWork);
                }
                else
                {
                    if (DeleteFromBookUser(userRoleDto, userRolesUnitOfWork))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
               
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteFromJournalUser(UserRoleDTO userRoleDto, UserRolesUnitOfWork userRolesUnitOfWork)
        {
            try
            {
                List<JournalUserRoles> journalUserRolesList = new List<JournalUserRoles>();
                var usermasterid = userRolesUnitOfWork.GetUserID(userRoleDto.userroles.UserID, userRoleDto.userroles.ServiceTypeId, userRoleDto.userroles.RollID);
                journalUserRolesList = _journalUserRoles.GetJournalDetailsForUserID(usermasterid);
                userRoleDto.deleteJournalUser = journalUserRolesList;
                userRolesUnitOfWork.DeleteJournalUserRolesDetails(userRoleDto);
                return true;
            }
              
            catch (Exception)
            {
                return false;
            }
            
        }



        public bool DeleteFromBookUser(UserRoleDTO userRoleDto, UserRolesUnitOfWork userRolesUnitOfWork)
        {
            try
            {
                List<BookUserRoles> bookUserRolesList = new List<BookUserRoles>();
                var usermasterid = userRolesUnitOfWork.GetUserID(userRoleDto.userroles.UserID, userRoleDto.userroles.ServiceTypeId, userRoleDto.userroles.RollID);
                bookUserRolesList = _bookUserReposistory.GetBookDetailsForUserID(usermasterid);
                userRoleDto.deleteBookUser = bookUserRolesList;
                userRolesUnitOfWork.DeleteBookUserRolesDetails(userRoleDto);  
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        private void SaveManuscriptJournalUserRolesDetails(UserRoleDTO userRoleDto, UserRolesUnitOfWork userRolesUnitOfWork)
        {
            try
            {
                List<JournalUserRoles> journalUserRolesList = new List<JournalUserRoles>();
                var usermasterid = userRolesUnitOfWork.GetUserID(userRoleDto.userroles.UserID, userRoleDto.userroles.ServiceTypeId, userRoleDto.userroles.RollID);
                if (userRoleDto.SelectedJournalID != null || userRoleDto.SelectedJournalIDs != null)
                {              
                    journalUserRolesList = _journalUserRoles.GetJournalDetailsForUserID(usermasterid);
                    userRoleDto.deleteJournalUser=journalUserRolesList;
                    userRolesUnitOfWork.DeleteJournalUserRolesDetails(userRoleDto);                   
                    foreach (var id in userRoleDto.SelectedJournalID)
                    {                        
                        //var check = userRolesUnitOfWork.CheckIfJournalForUserIsPresentInJournalUserRoles(usermasterid, id);
                            userRoleDto.journaluser.Clear();
                            var JournalUserRolesList = new JournalUserRoles();
                            {
                                JournalUserRolesList.JournalMasterId = id;
                                JournalUserRolesList.UserRolesId = userRoleDto.userroles.ID;
                                JournalUserRolesList.CreatedBy = userRoleDto.loginuser;
                                JournalUserRolesList.ModifiedBy = userRoleDto.loginuser;
                                JournalUserRolesList.ModifiedDate = DateTime.Now;
                                JournalUserRolesList.CreatedDate = DateTime.Now;
                                JournalUserRolesList.Status = true;
                            }
                            userRoleDto.journaluser.Add(JournalUserRolesList);
                            userRolesUnitOfWork.SaveJournalUserRolesDetails(userRoleDto);
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SaveManuscriptBookUserRolesDetails(UserRoleDTO userRoleDto, UserRolesUnitOfWork userRolesUnitOfWork)
        {
            try
            {
                List<BookUserRoles> bookUserRolesList = new List<BookUserRoles>();
                var usermasterid = userRolesUnitOfWork.GetUserID(userRoleDto.userroles.UserID, userRoleDto.userroles.ServiceTypeId, userRoleDto.userroles.RollID);
                if (userRoleDto.SelectedBookID != null || userRoleDto.SelectedBookIDs != null)
                {
                    bookUserRolesList = _bookUserReposistory.GetBookDetailsForUserID(usermasterid);                    
                         userRoleDto.deleteBookUser=bookUserRolesList;
                         userRolesUnitOfWork.DeleteBookUserRolesDetails(userRoleDto);  
                    foreach (var id in userRoleDto.SelectedBookID)
                    {
                                            
                            userRoleDto.bookuser.Clear();
                            var BookUserRolesList = new BookUserRoles();
                            {
                                BookUserRolesList.BookMasterId = id;
                                BookUserRolesList.UserRolesId = userRoleDto.userroles.ID;
                                BookUserRolesList.CreatedBy = userRoleDto.loginuser;
                                BookUserRolesList.ModifiedBy = userRoleDto.loginuser;
                                BookUserRolesList.ModifiedDate = DateTime.Now;
                                BookUserRolesList.CreatedDate = DateTime.Now;
                                BookUserRolesList.Status = true;
                            }
                            ;
                            userRoleDto.bookuser.Add(BookUserRolesList);
                            userRolesUnitOfWork.SaveBookUserRolesDetails(userRoleDto);
                      

                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }



        public bool IsUserIdinJournalUserRoles(int Id)
        {

            var userjournalRoles = (from ju in context.JournalUserRoles
                                    where ju.UserRolesId == Id
                                    select ju.ID).Count();

            if (Convert.ToInt32(userjournalRoles) == 0)
                return false;
            else
                return true;

        }
        public bool IsUserIdinBooklUserRoles(int Id)
        {

            var userbooklRoles = (from ju in context.BookUserRoles
                                  where ju.UserRolesId == Id
                                  select ju.ID).Count();

            if (Convert.ToInt32(userbooklRoles) == 0)
                return false;
            else
                return true;

        }
    }
}
