using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Data Access Layer
using Repos = TransferDesk.DAL.Manuscript.Repositories;
using DContext = TransferDesk.DAL.Manuscript.DataContext;

//Contracts
using Entities = TransferDesk.Contracts.Manuscript.Entities;
using DTOs = TransferDesk.Contracts.Manuscript.DTO;
using TransferDesk.Contracts.Manuscript.DTO;

namespace TransferDesk.DAL.Manuscript.UnitOfWork
{
    public class UserRolesUnitOfWork : IDisposable
    {
        private Repos.BookUserReposistory _bookUserReposistory;
        private Repos.JournalUserReposistory _journalUserReposistory;
        private Repos.UserRoleRepository _userRoleRepository;
        public DTOs.UserRoleDTO userRoleDto { get; set; }

        public UserRolesUnitOfWork(string conString)
        {
            _bookUserReposistory = new Repos.BookUserReposistory(conString);
            _journalUserReposistory = new Repos.JournalUserReposistory(conString);
            _userRoleRepository = new Repos.UserRoleRepository(conString);
        }

        public void SaveUserRolesDetails(UserRoleDTO userRoleDto)
        {
            var servicetypeID = userRoleDto.userroles.ServiceTypeId;
            var useridforcheck = userRoleDto.userroles.UserID;
            var checkuserID = _userRoleRepository.CheckIfUserIsPresent(useridforcheck, servicetypeID, userRoleDto.userroles.RollID);

            if (checkuserID != true)
            {
                userRoleDto.userroles.ModifiedDateTime = System.DateTime.Now;
                userRoleDto.userroles.Status = 1;
                _userRoleRepository.AddUserRole(userRoleDto.userroles);
            }
            else
            {
                var id = _userRoleRepository.CheckUserIdIfPresent(useridforcheck, servicetypeID, userRoleDto.userroles.RollID);
                userRoleDto.userroles.ID = id;
                userRoleDto.userroles.ModifiedDateTime = System.DateTime.Now;
                userRoleDto.userroles.Status = 1;
                _userRoleRepository.UpdateUserRole(userRoleDto.userroles);

            }
            _userRoleRepository.SaveUserRole();

        }

        public int GetUserID(string userid, int servicetype, int roleId)
        {
            try
            {
                var id = _userRoleRepository.GetUserID(userid, servicetype, roleId);
                return id;
            }
            catch (Exception e)
            {
                return 0;
            }


        }

        public bool CheckIfJournalForUserIsPresentInJournalUserRoles(int userid, int journalid)
        {
            var checkuserforjournal = _journalUserReposistory.CheckJournalUser(userid, journalid);
            if (checkuserforjournal == true)
                return true;
            else
            {
                return false;
            }

        }

        public bool CheckIfBookForUserIsPresentInBookUserRoles(int userid, int bookid)
        {
            var checkuserforbook = _bookUserReposistory.CheckBookUser(userid, bookid);
            if (checkuserforbook == true)
                return true;
            else
            {
                return false;
            }

        }

        public Entities.JournalUserRoles GetJournalDetails(int id)
        {
            var details = _journalUserReposistory.GetDetails(id);
            return details;
        }
        public Entities.BookUserRoles GetBookDetails(int id)
        {
            var details = _bookUserReposistory.GetBookDetails(id);
            return details;
        }

        public int GetIDfromJournalUserRoles(int userid, int jid)
        {
            var id = _journalUserReposistory.GetId(userid, jid);
            return id;
        }

        public int GetIDfromBookUserRoles(int userid, int bid)
        {
            var id = _bookUserReposistory.GetId(userid, bid);
            return id;
        }


        public void SaveJournalUserRolesDetails(UserRoleDTO userRoleDto)
        {

            foreach (var item in userRoleDto.journaluser)
            {
                _journalUserReposistory.AddJournalUserDetails(item);

            }
            _journalUserReposistory.SaveChanges();
        }

        public void DeleteJournalUserRolesDetails(UserRoleDTO userRoleDto)
        {

            foreach (var item in userRoleDto.deleteJournalUser)
            {
                    _journalUserReposistory.RemoveJournalUserDetails(item);
            }
            _journalUserReposistory.SaveChanges();
        }


        public void DeleteBookUserRolesDetails(UserRoleDTO userRoleDto)
        {

            foreach (var item in userRoleDto.deleteBookUser)
            {
                _bookUserReposistory.RemoveBookUserDetails(item);
            }
            _bookUserReposistory.SaveChanges();
        }

        public void SaveBookUserRolesDetails(UserRoleDTO userRoleDto)
        {
            foreach (var item in userRoleDto.bookuser)
            {
                    _bookUserReposistory.AddBookUserDetails(item);
            
            }
            _bookUserReposistory.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
