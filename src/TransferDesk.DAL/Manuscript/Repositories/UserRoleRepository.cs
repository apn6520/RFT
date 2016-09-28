
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;

using TransferDesk.Contracts.Manuscript.Repositories;
using Entities = TransferDesk.Contracts.Manuscript.Entities;

using TransferDesk.DAL.Manuscript.DataContext;
using System.Data.Entity.Infrastructure;
using TransferDesk.Contracts.Manuscript.ComplexTypes.AssociateDashBoard;
using TransferDesk.Contracts.Manuscript.ComplexTypes.ManuscriptLogin;
using TransferDesk.Contracts.Manuscript.Entities;
using TransferDesk.Contracts.Manuscript.ComplexTypes.UserRole;


namespace TransferDesk.DAL.Manuscript.Repositories
{
    public class UserRoleRepository : IDisposable
    {
        private ManuscriptDBContext context;
        private AssociateDashBoardReposistory _associateDashBoardReposistory;
        public UserRoleRepository(ManuscriptDBContext context)
        {
            this.context = context;
            _associateDashBoardReposistory=new AssociateDashBoardReposistory(context);
        }

		public UserRoleRepository(string constring)
        {
            this.context = new ManuscriptDBContext(constring);
            _associateDashBoardReposistory = new AssociateDashBoardReposistory(context);
        }
        public IEnumerable<Entities.UserRoles> GetUserRoles()
        {
            return context.UserRoles.ToList<Entities.UserRoles>();
        }

        public Entities.UserRoles GetUserRoleByID(int id)
        {
            return context.UserRoles.Find(id);
        }

        public void AddUserRole(Entities.UserRoles userRoles)
        {
            context.UserRoles.Add(userRoles);
        }

		public Entities.UserRoles GetUserDetailsById(int? id)
        {
            var userdetails = context.UserRoles.Find(id);
            return userdetails;
        }

        public void UpdateUserRole(Entities.UserRoles userRoles)
        {
            UserRoles existing = context.UserRoles.Find(userRoles.ID);
            ((IObjectContextAdapter)context).ObjectContext.Detach(existing);
            context.Entry(userRoles).State = EntityState.Modified;
        }

        public IEnumerable<Role> GetRoleByUserID(string userID)
        {
            var roles = from role in context.Roles
                        select role;
            return roles.ToList<Entities.Role>();
        }


        public IEnumerable<pr_GetUserRoleDetails_Result> GetUserRoleDetails()
        {
            try
            {
                IEnumerable<pr_GetUserRoleDetails_Result> userRoleDetails =
                    this.context.Database.SqlQuery<pr_GetUserRoleDetails_Result>("exec pr_GetUserRoleDetails").ToList();
                return userRoleDetails;

            }
            catch
            {
                return null;//todo:check and remove this trycatchhandler
            }
            finally
            {

            }

        }

        public bool CheckIfUserIsPresent(string userid, int servicetypeID, int roleId)
        {

            var check = (from q in context.UserRoles
                         where q.UserID == userid && q.ServiceTypeId == servicetypeID && q.RollID == roleId
                         select q);
            if (check.Count() > 0)
                return true;
            else
            {
                return false;
            }

        }
        public int CheckUserIdIfPresent(string userid, int servicetypeID, int roleId)
        {

            var check = (from q in context.UserRoles
                         where q.UserID == userid && q.ServiceTypeId == servicetypeID && q.RollID == roleId
                         select q);
            foreach (var id in check)
            {
                return id.ID;
            }
            return 0;
        }

        public int GetUserID(string userid, int serviceType, int roleId)
        {
            var id = from q in context.UserRoles
                     where q.UserID.ToLower().Trim() == userid.ToLower().Trim() && q.ServiceTypeId == serviceType && q.RollID == roleId
                     select q;
            foreach (var uid in id)
            {
                return uid.ID;

            }

            return 0;




        }
        
        public bool IsUserRoleAvailable(string userID, int serviceType, int roleId)
        {
            var count = 0;

            if (roleId == 1)
            {
                var userRoles = (from UR in context.UserRoles
                                 where UR.UserID == userID.Trim() && UR.ServiceTypeId == serviceType && UR.RollID == roleId && UR.IsActive == true
                                 select UR).FirstOrDefault();
                if (userRoles == null || userRoles.ServiceTypeId != serviceType)
                    count = CheckAccessOfUser(userID, serviceType, 1);
                else
                    count = 0;
            }

            if (count > 0)
                return false;
            else
                return true;
        }

        public int CheckAccessOfUser(string userID, int serviceType, int roleId)
        {
            var count = 0;
            if (serviceType == 5)
                serviceType = 6;
            else
                serviceType = 5;
            var updateUserRoles = (from UR in context.UserRoles
                                   where UR.UserID == userID.Trim() && UR.ServiceTypeId == serviceType && UR.RollID == roleId && UR.IsActive == true
                                   select UR.UserID).Count();
            if (updateUserRoles == 0)
                count = 0;
            else
                count = updateUserRoles;
            return count;
        }


        public bool IsJobFetchedByUser(string userID, int serviceType, int roleId, bool IsActive)
        {
            if (IsActive == false)
            {
                pr_IsJobFetchedOrAssign_Result IsJobFetchedOrAssing;
                IsJobFetchedOrAssing = _associateDashBoardReposistory.IsJobFetchedOrAssign(userID, serviceType);
                if (IsJobFetchedOrAssing.FetchedJobCount == 0)
                    return true;
                else
                    return false;
            }
            else
                return true;
        }

        public void SaveUserRole()
        {
            context.SaveChanges();
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool IsUserRoleAvailable(int id, int rollId, string userId)
        {
            if (id == 0)
            {
                var count = (from q in context.UserRoles
                                where q.RollID == rollId && q.UserID == userId
                                select q).Count();
                if (count > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                var userRoles = from q in context.UserRoles
                                where q.RollID == rollId && q.UserID == userId
                                select q;
                if (userRoles.ToList().Count() == 1)
                {
                    var pkCheck = from userRole in userRoles
                                  where userRole.ID == id
                                  select userRole;
                    if (pkCheck.ToList().Count() == 1)
                        return false;
                    else
                        return true;
                }
                else if (userRoles.ToList().Count() > 1)
                    return true;
                else
                    return false;
            }
        }

        public bool IsUserIDAvailable(string userID)
        {

            var count = (from q in context.Users
                         where q.EmpUserID == userID.Trim()
                         select q).Count();
            if (count > 0)
                return true; 
            else
                return false;
            
        }

        public bool IsAdmin(string userId)
        {
            var count=(from userAdmin in context.UserAdmin
                        where userAdmin.UserID==userId
                            select userAdmin).Count();
            if (count > 0)
                return true;
            else
                return false;
        }

		
		
		public List<pr_GetUserMasterDetails_Result> pr_GetUserMasterDetails_Result()
        {
            try
            {
                List<pr_GetUserMasterDetails_Result> usermasterdetails =
             this.context.Database.SqlQuery<pr_GetUserMasterDetails_Result>("exec UserMasterDetails").ToList();
                return usermasterdetails;
            }
            catch
            {
                return null;//todo:check and remove this trycatchhandler
            }
            finally
            {

            }

        }
		public List<Entities.SlidingScale> GetSlidingScaleList(int servicetypeid)
        {
            var slidingscale = from s in context.SlidingScale
                               where s.ServiceTypeID == servicetypeid && s.Status==1
                               select s;
            //   var result = from ja in journalArticles join s in manuscriptDataContextRead.ArticleTypes on ja.ArticleTypeID equals s.ID select s;
            return slidingscale.ToList();
        }
    }
}
