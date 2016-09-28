using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.ComplexTypes.AdminDashBoard;
using TransferDesk.Contracts.Manuscript.ComplexTypes.AssociateDashBoard;
using TransferDesk.DAL.Manuscript.DataContext;

namespace TransferDesk.DAL.Manuscript.Repositories
{
    public class AssociateDashBoardReposistory
    {

        private ManuscriptDBContext context;
        private bool disposed = false;


        public AssociateDashBoardReposistory(string conString)
        {
            this.context = new ManuscriptDBContext(conString);
        }

        public AssociateDashBoardReposistory(ManuscriptDBContext manuscriptDbContext)
        {
            this.context = manuscriptDbContext;  
        }

        public IEnumerable<pr_GetSpecificAssociateDetails_Result> pr_GetAllAssociatesAssignedJobs(string userid, int serviceTypeId)
        {
            try
            {

                var associateuserid = userid != null ?
                   new SqlParameter("userid", userid) :
                   new SqlParameter("userid", typeof(global::System.String));

                var serviceType = serviceTypeId != null ?
                   new SqlParameter("serviceType", serviceTypeId) :
                   new SqlParameter("serviceType", typeof(global::System.Int32));

                IEnumerable<pr_GetSpecificAssociateDetails_Result> alljobsdetails = this.context.Database.SqlQuery
                                                                                  <pr_GetSpecificAssociateDetails_Result>("exec pr_GetAssociateAssignedJobs @userid,@serviceType", associateuserid, serviceType).ToList();
                return alljobsdetails;
            }
            catch
            {
                return null;//todo:check and remove this trycatchhandler
            }
            finally
            {

            }
        }

        public IEnumerable<pr_GetSpecificAssociateDetails_Result> GetAssociatedFetchedJobs(string crestId, int serviceTypeId, int role)
        {
            try
            {

                var crestID = crestId != null ?
                   new SqlParameter("crestID", crestId) :
                   new SqlParameter("crestID", typeof(global::System.String));

                var serviceType = serviceTypeId != null ?
                   new SqlParameter("serviceType", serviceTypeId) :
                   new SqlParameter("serviceType", typeof(global::System.Int32));

                var roleId = role != null ?
                   new SqlParameter("roleID", role) :
                   new SqlParameter("roleID", typeof(global::System.Int32));

                IEnumerable<pr_GetSpecificAssociateDetails_Result> fetchedJobs = this.context.Database.SqlQuery
                                                                                  <pr_GetSpecificAssociateDetails_Result>("exec pr_GetAssociatedFetchedJobs @crestID,@serviceType,@roleID", crestID, serviceType, roleId).ToList();
                return fetchedJobs;
            }
            catch
            {
                return null;//todo:check and remove this trycatchhandler
            }
            finally
            {

            }
        }


        public pr_IsJobFetched_Result IsJobFetched(string userid, int serviceTypeId)
        {
            try
            {

                var userId = userid != null ?
                   new SqlParameter("userId", userid) :
                   new SqlParameter("userId", typeof(global::System.String));

                var serviceType = serviceTypeId != null ?
                   new SqlParameter("serviceType", serviceTypeId) :
                   new SqlParameter("serviceType", typeof(global::System.Int32));

                pr_IsJobFetched_Result fetchedJobs = this.context.Database.SqlQuery<pr_IsJobFetched_Result>("exec pr_IsJobFetched @userId,@serviceType", userId, serviceType).FirstOrDefault();
                return fetchedJobs;
            }
            catch
            {
                return null;//todo:check and remove this trycatchhandler
            }
            finally
            {

            }
        }


        public pr_IsJobFetchedOrAssign_Result IsJobFetchedOrAssign(string userid, int serviceTypeId)
        {
            try
            {

                var userId = userid != null ?
                   new SqlParameter("userId", userid) :
                   new SqlParameter("userId", typeof(global::System.String));

                var serviceType = serviceTypeId != null ?
                   new SqlParameter("serviceType", serviceTypeId) :
                   new SqlParameter("serviceType", typeof(global::System.Int32));

                pr_IsJobFetchedOrAssign_Result fetchedJobs = this.context.Database.SqlQuery<pr_IsJobFetchedOrAssign_Result>("exec pr_IsJobFetchedOrAssign @userId,@serviceType", userId, serviceType).FirstOrDefault();
                return fetchedJobs;
            }
            catch
            {
                return null;//todo:check and remove this trycatchhandler
            }
            finally
            {

            }
        }

        public pr_JobTobeFetched_Result JobTobeFetched(string userid, int serviceTypeId, int roleId)
        {
            try
            {

                var userId = userid != null ?
                   new SqlParameter("userId", userid) :
                   new SqlParameter("userId", typeof(global::System.Int32));

                var serviceType = serviceTypeId != null ?
                   new SqlParameter("serviceType", serviceTypeId) :
                   new SqlParameter("serviceType", typeof(global::System.Int32));

                var role = roleId != null ?
                   new SqlParameter("RoleId", roleId) :
                   new SqlParameter("RoleId", typeof(global::System.Int32));

                pr_JobTobeFetched_Result fetchedJobs = this.context.Database.SqlQuery<pr_JobTobeFetched_Result>("exec pr_JobTobeFetched @userId,@serviceType,@RoleId", userId, serviceType, role).FirstOrDefault();
                return fetchedJobs;
            }
            catch
            {
                return null;//todo:check and remove this trycatchhandler
            }
            finally
            {

            }
        }
        
        public string GetMSIDOnCrestId(string crestID)
        {
            string MSID = string.Empty;
            if (crestID.StartsWith("J"))
            {
                MSID = (from ML in context.ManuscriptLogin
                        where ML.CrestId == crestID
                        select ML.MSID).FirstOrDefault();
            }
            else if (crestID.StartsWith("B"))
            {
                MSID = (from ML in context.ManuscriptBookLogin
                        where ML.CrestID == crestID
                        select ML.ChapterNumber).FirstOrDefault();          
            }
            else
            {
                MSID = (from MBL in context.ManuscriptBookLogin
                        where MBL.CrestID == crestID
                        select MBL.ID).FirstOrDefault().ToString();               
            }
            return MSID;
        }

        public string GetChapterNumberOnCrestId(string crestID)
        {
            string ChapNumber = "";
          
             if (crestID.StartsWith("B"))
            {
                ChapNumber = (from ML in context.ManuscriptBookLogin
                        where ML.CrestID == crestID
                        select ML.ChapterNumber).FirstOrDefault();
            }
             return ChapNumber;
        }

        public int GetServiceTypeOnUserId(string userID)
        {
            var serivceType = (from UR in context.UserRoles
                               where UR.UserID == userID && UR.RollID == 1 && UR.IsActive==true
                               select UR.ServiceTypeId).FirstOrDefault();

            return Convert.ToInt32(serivceType);
        }

        public int GetManuscriptIDOnMSID(string MSID, string crestID)
        {
            int ID = 0;
            if (crestID.StartsWith("J"))
            {
                ID = (from M in context.Manuscripts
                      where M.MSID == MSID
                      select M.ID).FirstOrDefault();
            }
            else if (crestID.StartsWith("B"))
            {
                ID = (from MBL in context.ManuscriptBookLogin
                      where MBL.CrestID == crestID
                      select MBL.ID).FirstOrDefault();
            }            
            return ID;
        }

         public int GetIDOnChapterNumber(string ChpNumber,string Crestid)
        {
            int ID = 0;
           if (Crestid.StartsWith("B"))
            {
                  ID = (from BM in context.ManuscriptBookLogin
                        where BM.ChapterNumber == ChpNumber && BM.CrestID==Crestid
                      select BM.ID).FirstOrDefault();
            }            
            return ID;
        }

        public int TotalAssociateJournalMSPendingJobs()
        {
            var result =
                context.ManuscriptLoginDetails.Where(x => x.JobProcessStatusId == 7 && x.ServiceTypeStatusId == 5 && x.JobStatusId != 8 && x.RoleId == 1)
                    .ToList()
                    .Count();
            return result;
        }

        public int TotalAssociateJournalRSPendingJobs()
        {
            var result =
                context.ManuscriptLoginDetails.Where(x => x.JobProcessStatusId == 7 && x.ServiceTypeStatusId == 6 && x.JobStatusId != 8 && x.RoleId == 1)
                    .ToList()
                    .Count();
            return result;
        }

        public int TotalAssociateBookMSPendingJobs()
        {
            var result =
                context.ManuscriptBookLoginDetails.Where(x => x.JobProcessStatusId == 7 && x.ServiceTypeStatusId == 5 && x.JobStatusId != 8 && x.RoleId == 1)
                    .ToList()
                    .Count();
            return result;
        }

        public int TotalAssociateBookRSPendingJobs()
        {
            var result =
                context.ManuscriptBookLoginDetails.Where(x => x.JobProcessStatusId == 7 && x.ServiceTypeStatusId == 6 && x.JobStatusId != 8 && x.RoleId == 1)
                    .ToList()
                    .Count();
            return result;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

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

    }
}
