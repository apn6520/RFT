using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.ComplexTypes.QualityAnalystDashBoard;
using TransferDesk.DAL.Manuscript.DataContext;

namespace TransferDesk.DAL.Manuscript.Repositories
{
    public class QualityAnalystDashBoardReposistory
    {

        private ManuscriptDBContext context;
        private bool disposed = false;


        public QualityAnalystDashBoardReposistory(string conString)
        {
            this.context = new ManuscriptDBContext(conString);
        }

        public IEnumerable<pr_GetSpecificQualityAnalystJobs_Result> pr_GetAllQualityAnalystAssignedJobs(string userid, int serviceTypeId)
        {
            try
            {

                var qualityassociateid = userid != null ?
                   new SqlParameter("userid", userid) :
                   new SqlParameter("userid", typeof(global::System.String));

                var serviceType = serviceTypeId != null ?
                   new SqlParameter("serviceType", serviceTypeId) :
                   new SqlParameter("serviceType", typeof(global::System.Int32));


                IEnumerable<pr_GetSpecificQualityAnalystJobs_Result> alljobsdetails = this.context.Database.SqlQuery
                                                                                  <pr_GetSpecificQualityAnalystJobs_Result>("exec pr_GetQualityAnalystAssignedJobs @userid,@serviceType", qualityassociateid, serviceType).ToList();
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

        public int TotalQualityMSPendingJobs()
        {
            var result =
                context.ManuscriptLoginDetails.Where(x => x.JobProcessStatusId == 7 && x.ServiceTypeStatusId == 5 && x.JobStatusId != 8 && x.RoleId == 2)
                    .ToList()
                    .Count();
            return result;
        }

        public int TotalQualityRSPendingJobs()
        {
            var result =
                context.ManuscriptLoginDetails.Where(x => x.JobProcessStatusId == 7 && x.ServiceTypeStatusId == 6 && x.JobStatusId !=8 && x.RoleId==2)
                    .ToList()
                    .Count();
            return result;
        }

        public IEnumerable<pr_GetSpecificQualityAnalystJobs_Result> GetQualityFetchedJobs(string crestId, int serviceTypeId, int role)
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

                IEnumerable<pr_GetSpecificQualityAnalystJobs_Result> fetchedJobs = this.context.Database.SqlQuery
                                                                                  <pr_GetSpecificQualityAnalystJobs_Result>("exec pr_GetQualityFetchedJobs @crestID,@serviceType,@roleID", crestID, serviceType, roleId).ToList();
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

        public pr_IsJobFetchedByQuality_Result IsJobFetchedByQuality(string userid, int serviceTypeId)
        {
            try
            {

                var userId = userid != null ?
                   new SqlParameter("userId", userid) :
                   new SqlParameter("userId", typeof(global::System.String));

                var serviceType = serviceTypeId != null ?
                   new SqlParameter("serviceType", serviceTypeId) :
                   new SqlParameter("serviceType", typeof(global::System.Int32));

                pr_IsJobFetchedByQuality_Result fetchedJobs = this.context.Database.SqlQuery<pr_IsJobFetchedByQuality_Result>("exec pr_IsJobFetchedByQuality @userId,@serviceType", userId, serviceType).FirstOrDefault();
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
            else
            {
                MSID = "";
            }
            return MSID;
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
            else
                ID = 0;
            return ID;
        }

        public int GetServiceTypeOnUserId(string userID)
        {
            var serivceType = (from UR in context.UserRoles
                               where UR.UserID == userID && UR.RollID == 2 && UR.IsActive == true && UR.ServiceTypeId == 5
                               select UR.ServiceTypeId).FirstOrDefault();

            return Convert.ToInt32(serivceType);
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

                pr_JobTobeFetched_Result fetchedJobs = this.context.Database.SqlQuery<pr_JobTobeFetched_Result>("exec pr_JobTobeFetchedInQuality @userId,@serviceType,@RoleId", userId, serviceType, role).FirstOrDefault();
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
