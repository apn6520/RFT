using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using RepositoryInterfaces = TransferDesk.Contracts.ReviewerIndex.Repositories;
using Entities = TransferDesk.Contracts.Manuscript.Entities;
using DataContexts = TransferDesk.DAL.ReviewerIndex.DataContext;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using TransferDesk.Contracts.ReviewerIndex.ComplexTypes;
using TransferDesk.DAL.ReviewerIndex.DataContext;
using System.Data.Entity.Infrastructure;
using TransferDesk.Contracts.ReviewerIndex.Entities;
using System.Data;
using System.Linq.Expressions;
using TransferDesk.Contracts.Logging;


namespace TransferDesk.DAL.ReviewerIndex.Repositories
{
    public class ReviewerIndexDBRepositoriesReadSite : IDisposable
    {

        public DataContexts.ReviewerIndexDBContext reviewerIndexDBContextRead;
        private ILogger _logger;

        public ReviewerIndexDBRepositoriesReadSite(string conString, ILogger Logger) 
        {
            this.reviewerIndexDBContextRead = new DataContexts.ReviewerIndexDBContext(conString);
            _logger = Logger;
        }
        public string GetReviewerIds(string searchOne, string searchTwo, string condition, string ddlSerarchOne, string ddlSearchTwo)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                stringBuilder.AppendLine("GetReviewerIds execution start.");
                var srchOne = searchOne != null ? new SqlParameter("KeySearchOne", searchOne) : new SqlParameter("KeySearchOne", typeof(global::System.String));
                var srechTwo = searchTwo != null ? new SqlParameter("KeySearchTwo", searchTwo) : new SqlParameter("KeySearchTwo", typeof(global::System.String));
                var srchCondition = condition != null ? new SqlParameter("SearchCondition", condition) : new SqlParameter("SearchCondition", typeof(global::System.String));
                var srchddlOne = ddlSerarchOne != null ? new SqlParameter("DDLOneValue", ddlSerarchOne) : new SqlParameter("DDLOneValue", "tm.Name");
                var srchddlTwo = ddlSearchTwo != null ? new SqlParameter("DDLTwoValue", ddlSearchTwo) : new SqlParameter("DDLTwoValue", "vwe.AreaOfExpertise");
                stringBuilder.AppendLine(string.Format("KeySearchOne : {0} KeySearchTwo {1} SearchCondition : {2} DDLOneValue {3} DDLTwoValue : {4}", srchOne, srechTwo, srchCondition, srchddlOne, srchddlTwo));

                var result = this.reviewerIndexDBContextRead.Database.SqlQuery<string>("exec spGetReviewersList  @KeySearchOne, @KeySearchTwo, @SearchCondition, @DDLOneValue, @DDLTwoValue", srchOne, srechTwo, srchCondition, srchddlOne, srchddlTwo).FirstOrDefault();
                stringBuilder.AppendLine("GetReviewerIds execution end.");
                return Convert.ToString(result);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, stringBuilder);
                throw;
            }
            finally
            {
                _logger.WriteStringBuilderToUserLogAndClear(stringBuilder);
            }
        }

        public IEnumerable<pr_GetReviewersList> GetReviewersLists(string strReviewersIDs , int? minValue, int? maxValue)
        {
           StringBuilder stringBuilder = new StringBuilder();
            try
            {

                stringBuilder.AppendLine("adding parameters to SqlParameter");
                var srchReviewerIds = strReviewersIDs != null ? new SqlParameter("ReviewerIDs", strReviewersIDs) : new SqlParameter("ReviewerIDs", "0");

                var srchminValue = minValue != null ? new SqlParameter("MinValue", minValue) : new SqlParameter("MinValue", null);
                var srchmaxValue = minValue != null ? new SqlParameter("MaxValue", maxValue) : new SqlParameter("MaxValue", null);

                var reviewerDetailsLists = this.reviewerIndexDBContextRead.Database.SqlQuery
                                         <pr_GetReviewersList>("exec spGetReviewersListDetails @ReviewerIDs, @MinValue, @MaxValue", srchReviewerIds, srchminValue, srchmaxValue).ToList();
                stringBuilder.AppendLine("store procedure executed successfully.");
                
                stringBuilder.AppendLine("INFO DAL : Method Name - GetReviewersLists : reviewer detail result count - " + reviewerDetailsLists.Count);
                return reviewerDetailsLists;

            }
            catch(Exception ex)
            {
                _logger.LogException(ex, stringBuilder);
                return null;//todo:check and remove this trycatchhandler
            }
            finally
            {
                _logger.WriteStringBuilderToUserLogAndClear(stringBuilder);
            }

        }

        public virtual int spSaveUpdateReviewerDetails(string initials, string lastName, string firstName, string middleName, string name, Nullable<int> reviewerId, Nullable<int> instituteID, Nullable<int> deptId, string streetName, Nullable<int> cityID, Nullable<int> noOfPublications, string userID)
        {
            string fullName = initials + " " + firstName + " " + middleName + " " + lastName;
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                stringBuilder.AppendLine("spSaveUpdateReviewerDetails execution start");
                var initialsParameter = initials != null ?
               new SqlParameter("Initials", initials) :
               new SqlParameter("Initials", typeof(string));

                var lastNameParameter = lastName != null ?
                    new SqlParameter("LastName", lastName) :
                    new SqlParameter("LastName", typeof(string));

                var firstNameParameter = firstName != null ?
                    new SqlParameter("FirstName", firstName) :
                    new SqlParameter("FirstName", typeof(string));

                var middleNameParameter = middleName != null ?
                    new SqlParameter("MiddleName", middleName) :
                    new SqlParameter("MiddleName", typeof(string));

                var nameParameter = fullName != null ?
                    new SqlParameter("Name", fullName) :
                    new SqlParameter("Name", typeof(string));

                var reviewerIdParameter = reviewerId.HasValue ?
                    new SqlParameter("ReviewerId", reviewerId) :
                    new SqlParameter("ReviewerId", typeof(int));

                var instituteIDParameter = instituteID.HasValue ?
                    new SqlParameter("InstituteID", instituteID) :
                    new SqlParameter("InstituteID", typeof(int));

                var deptIdParameter = deptId.HasValue ?
                    new SqlParameter("DeptId", deptId) :
                    new SqlParameter("DeptId", typeof(int));

                var streetNameParameter = streetName != null ?
                    new SqlParameter("StreetName", streetName) :
                    new SqlParameter("StreetName", typeof(string));

                var cityIDParameter = cityID.HasValue ?
                    new SqlParameter("CityID", cityID) :
                    new SqlParameter("CityID", typeof(int));

                var noOfPublicationsParameter = noOfPublications.HasValue ?
                    new SqlParameter("NoOfPublications", noOfPublications) :
                    new SqlParameter("NoOfPublications", typeof(int));

                var userIDParameter = userID != null ?
                    new SqlParameter("UserID", userID) :
                    new SqlParameter("UserID", typeof(string));

                stringBuilder.AppendLine("this are sqlParameters we are passing to store procedures -> " + initialsParameter + lastNameParameter + firstNameParameter + middleNameParameter + nameParameter + reviewerIdParameter + instituteIDParameter + deptIdParameter + streetNameParameter + cityIDParameter + noOfPublicationsParameter + userIDParameter);
                var result = this.reviewerIndexDBContextRead.Database.SqlQuery<int>("exec spSaveUpdateReviewerDetailsNew  @Initials, @LastName, @FirstName, @MiddleName, @Name, @ReviewerId, @InstituteID, @DeptId, @StreetName, @CityID,@NoOfPublications,@UserID",
                    initialsParameter, lastNameParameter, firstNameParameter, middleNameParameter, nameParameter, reviewerIdParameter, instituteIDParameter, deptIdParameter, streetNameParameter, cityIDParameter, noOfPublicationsParameter, userIDParameter).FirstOrDefault();
                stringBuilder.AppendLine("spSaveUpdateReviewerDetails execution end");
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogException(ex, stringBuilder);
                throw;
            }
            finally
            {
                _logger.WriteStringBuilderToUserLogAndClear(stringBuilder);
            }
        }

        public ReviewerProfile_Result GetReviewerDetails(int reviewerID)
        {
               StringBuilder stringBuilder = new StringBuilder();
               ReviewerProfile_Result reviewerProfile = new ReviewerProfile_Result();
               stringBuilder.AppendLine("GetReviewerDetails > method execution start for reviewerID :" + reviewerID +"StartTime :" + DateTime.Now.ToString());
                // Make sure Code First has built the model before we open the connection
                reviewerIndexDBContextRead.Database.Initialize(force: false);

                // Create a SQL command to execute the sproc
                var cmd = reviewerIndexDBContextRead.Database.Connection.CreateCommand();
                stringBuilder.AppendLine("GetReviewerDetails > Create a SQL command to execute the StoredProcedure");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[spGetReviewerDetails]";
                cmd.Parameters.Add(new SqlParameter("@ReviewerID", reviewerID));
               
                try
                {
                   
                    // Run the sproc
                    reviewerIndexDBContextRead.Database.Connection.Open();
                    stringBuilder.AppendLine("GetReviewerDetails > reviewerIndexDBContextRead Database connection open.");
                    var reader = cmd.ExecuteReader();
                  

                    // Read Blogs from the first result set
                    var reviewerData = ((IObjectContextAdapter)reviewerIndexDBContextRead)
                        .ObjectContext
                        .Translate<spGetReviewerDetails_Result>(reader, "spGetReviewerDetails_Result", MergeOption.AppendOnly);
                    stringBuilder.AppendLine("GetReviewerDetails > start to read 'spGetReviewerDetails_Result' result set.");

                    foreach (var item in reviewerData)
                    {
                        reviewerProfile.ReviewerDetails_Result = new spGetReviewerDetails_Result { 
                            ReviewerID = item.ReviewerID, Initials = item.Initials, FirstName = item.FirstName, 
                             City = item.City, CityId = item.CityId, Country = item.Country, CountryID = item.CountryID,
                             DepartmentName = item.DepartmentName, DeptID=item.DeptID, InstituteID = item.InstituteID,
                             InstituteName= item.InstituteName, LastName=item.LastName, StateId= item.StateId, MiddleName=item.MiddleName,
                             NoOfPublication = item.NoOfPublication, ReviewerName = item.ReviewerName, State= item.State, StreetName= item.StreetName,
                             TitleMasterID = item.TitleMasterID
                        };
                    }

                    // Move to second result set and read Posts
                    reader.NextResult();
                    var Emails = ((IObjectContextAdapter)reviewerIndexDBContextRead)
                        .ObjectContext
                        .Translate<ReviewerEmails_Result>(reader, "ReviewerEmails_Result", MergeOption.AppendOnly);
                    stringBuilder.AppendLine("GetReviewerDetails > start to read 'ReviewerEmails_Result' result set.");

                    foreach (var item in Emails)
                    {
                        if (item.IsActive == true)
                        {
                            reviewerProfile.ReviewerEmails.Add(new ReviewerEmails_Result { ID = item.ID, Email = item.Email, IsActive = item.IsActive, ModifiedDate = item.ModifiedDate, ReviewerMasterID = item.ReviewerMasterID });
                        }
                    }

                    reader.NextResult();
                    var areaOfExpertise = ((IObjectContextAdapter)reviewerIndexDBContextRead)
                        .ObjectContext
                        .Translate<AreaOfExpertise_Result>(reader, "AreaOfExpertise_Result", MergeOption.AppendOnly);
                    stringBuilder.AppendLine("GetReviewerDetails > start to read 'AreaOfExpertise_Result' result set.");

                    foreach (var item in areaOfExpertise)
                    {
                        if (item.IsActive == true)
                        {
                            reviewerProfile.AreaOfExpertise.Add(new AreaOfExpertise_Result
                            {
                                ID = item.ID,
                                AreaOfExpertiseMasterID = item.AreaOfExpertiseMasterID,
                                IsActive = item.IsActive,
                                ModifiedDate = item.ModifiedDate,
                                Name = item.Name,
                                ReviewerMasterID = item.ReviewerMasterID
                            });
                        }

                    }

                    reader.NextResult();
                    var referenceLink = ((IObjectContextAdapter)reviewerIndexDBContextRead)
                        .ObjectContext
                        .Translate<ReferenceLink_Result>(reader, "ReferenceLink_Result", MergeOption.AppendOnly);
                    stringBuilder.AppendLine("GetReviewerDetails > start to read 'ReferenceLink_Result' result set.");

                    foreach (var item in referenceLink)
                    {
                        if (item.IsActive == true)
                        {
                            reviewerProfile.ReferenceLink.Add(new ReferenceLink_Result { ID = item.ID, ReferenceLink = item.ReferenceLink, IsActive = item.IsActive, ModifiedDate = item.ModifiedDate, ReviewerMasterID = item.ReviewerMasterID });

                        }
                    }

                    reader.NextResult();
                    var Journals = ((IObjectContextAdapter)reviewerIndexDBContextRead)
                        .ObjectContext
                        .Translate<Journal_Result>(reader, "Journal_Result", MergeOption.AppendOnly);
                    stringBuilder.AppendLine("GetReviewerDetails > start to read 'Journal_Result' result set.");

                    foreach (var item in Journals)
                    {
                        if (item.IsActive == true)
                        {
                            reviewerProfile.Journal.Add(new Journal_Result { JournalID = item.JournalID, JournalTitle = item.JournalTitle, IsActive = item.IsActive, ModifiedDate = item.ModifiedDate, ReviewerMasterID = item.ReviewerMasterID });
                        }
                    }      

                    reader.NextResult();
                    var titleMaster = ((IObjectContextAdapter)reviewerIndexDBContextRead)
                        .ObjectContext
                        .Translate<TitleReviewerlinkMaster_Result>(reader, "TitleReviewerlinkMaster_Result", MergeOption.AppendOnly);
                    stringBuilder.AppendLine("GetReviewerDetails > start to read 'TitleReviewerlinkMaster_Result' result set.");

                    foreach (var item in titleMaster)
                    {
                        if (item.IsActive == true)
                        {
                            reviewerProfile.TitleMaster.Add(new TitleReviewerlinkMaster_Result { ID = item.ID, Name = item.Name, IsActive = item.IsActive, ModifiedDate = item.ModifiedDate, CreatedBy = item.CreatedBy, MScriptID = item.MScriptID });
                        }
                    }

                    reader.NextResult();
                    var affillationMaster = ((IObjectContextAdapter)reviewerIndexDBContextRead)
                        .ObjectContext
                        .Translate<TitleAndAffillationMaster_Result>(reader, "TitleAndAffillationMaster_Result", MergeOption.AppendOnly);
                    stringBuilder.AppendLine("GetReviewerDetails > start to read 'TitleAndAffillationMaster_Result' result set.");

                    foreach (var item in affillationMaster)
                    {
                        if (item.IsActive == true)
                        {
                            reviewerProfile.AffillationMastert.Add(new TitleAndAffillationMaster_Result { ID = item.ID, Name = item.Name, ModifiedDate = item.ModifiedDate, IsActive = item.IsActive });
                        }
                    }

                    reader.NextResult();
                    var areaOfExpReviewerlink = ((IObjectContextAdapter)reviewerIndexDBContextRead)
                        .ObjectContext
                        .Translate<AreaOfExpReviewerlink_Result>(reader, "AreaOfExpReviewerlink_Result", MergeOption.AppendOnly);

                    stringBuilder.AppendLine("GetReviewerDetails > start to read 'AreaOfExpReviewerlink_Result' result set.");

                    foreach (var item in areaOfExpReviewerlink)
                    {
                        if (item.IsActive == true)
                        {
                            reviewerProfile.AreaOfExpReviewerlink.Add(new AreaOfExpReviewerlink_Result { FExpertiseLevel = item.FExpertiseLevel, PID = item.PID, PrimaryExp = item.PrimaryExp, SecondaryExp = item.SecondaryExp, SExpertiseLevel = item.SExpertiseLevel, SID = item.SID, TertiaryExp = item.TertiaryExp, TExpertiseLevel = item.TExpertiseLevel, TID = item.TID });
                        }
                    }
                    stringBuilder.AppendLine("GetReviewerDetails > end execution process of GetReviewerDetails. " + "EndTime :" + DateTime.Now.ToString());
                    return reviewerProfile;
                }
                catch (Exception ex)
                {
                    _logger.LogException(ex, stringBuilder);
                    throw;
                }
                finally
                {
                    _logger.WriteStringBuilderToUserLogAndClear(stringBuilder);
                }
            
        }

        public void UpdateAreaOfExpReviewerlink(int reviewerID)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                stringBuilder.AppendLine("UpdateAreaOfExpReviewerlink execution start for ReviewerMasterID :" + reviewerID);
                var records = this.reviewerIndexDBContextRead.AreaOfExpReviewerlink.Where(o => o.ReviewerMasterID == reviewerID).ToList();
                foreach (var item in records)
                {
                    reviewerIndexDBContextRead.AreaOfExpReviewerlink.Remove(item);
                }
                reviewerIndexDBContextRead.SaveChanges();
                stringBuilder.AppendLine("UpdateAreaOfExpReviewerlink execution end for ReviewerMasterID :" + reviewerID);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, stringBuilder);
                throw;
            }
            finally
            {
                _logger.WriteStringBuilderToUserLogAndClear(stringBuilder);
            }
        }

        public void UpdateEmailAddress(ReviewerMailLink reviewerMailLink)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                stringBuilder.AppendLine("UpdateEmailAddress execution start for Email :" + reviewerMailLink.Email + "ReviewerMasterID :" + reviewerMailLink.ReviewerMasterID);
                var result = this.reviewerIndexDBContextRead.ReviewerMailLink.Where(o => o.Email == reviewerMailLink.Email && o.ReviewerMasterID == reviewerMailLink.ReviewerMasterID).FirstOrDefault();
                result.IsActive = reviewerMailLink.IsActive;
                result.ModifiedBy = reviewerMailLink.CreatedBy;
                result.ModifiedDate = DateTime.Now;
                reviewerIndexDBContextRead.SaveChanges();
                stringBuilder.AppendLine("UpdateEmailAddress execution end for Email :" + reviewerMailLink.Email + "ReviewerMasterID :" + reviewerMailLink.ReviewerMasterID);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, stringBuilder);
                throw;
            }
            finally
            {
                _logger.WriteStringBuilderToUserLogAndClear(stringBuilder);
            }       
        }

        public void UpdateReferenceReviewerlink(ReferenceReviewerlink referenceReviewerlink)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                stringBuilder.AppendLine("UpdateReferenceReviewerlink execution start for ReferenceLink :" + referenceReviewerlink.ReferenceLink + "ReviewerMasterID :" + referenceReviewerlink.ReviewerMasterID);
                var result = this.reviewerIndexDBContextRead.ReferenceReviewerlink.Where(o => o.ReferenceLink == referenceReviewerlink.ReferenceLink && o.ReviewerMasterID == referenceReviewerlink.ReviewerMasterID).FirstOrDefault();
                result.IsActive = referenceReviewerlink.IsActive;
                result.ModifiedBy = referenceReviewerlink.CreatedBy;
                result.ModifiedDate = DateTime.Now;
                reviewerIndexDBContextRead.SaveChanges();
                stringBuilder.AppendLine("UpdateReferenceReviewerlink execution end for ReferenceLink :" + referenceReviewerlink.ReferenceLink + "ReviewerMasterID :" + referenceReviewerlink.ReviewerMasterID);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, stringBuilder);
                throw;
            }
            finally
            {
                _logger.WriteStringBuilderToUserLogAndClear(stringBuilder);
            }
        }

        public void UpdateManuscriptTitle(int titleId, string mScriptID, string name)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                stringBuilder.AppendLine("UpdateManuscriptTitle execution start for titleId :" + titleId + "mScriptID :" + mScriptID + "name :" + name);
                var result = this.reviewerIndexDBContextRead.TitleMaster.Where(o => o.TitleID == titleId).FirstOrDefault();
                result.TitleName = name;
                result.Name = name;
                result.MScriptID = mScriptID;
                reviewerIndexDBContextRead.SaveChanges();
                stringBuilder.AppendLine("UpdateManuscriptTitle execution end for titleId :" + titleId + "mScriptID :" + mScriptID + "name :" + name);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, stringBuilder);
                throw;
            }
            finally
            {
                _logger.WriteStringBuilderToUserLogAndClear(stringBuilder);
            }
        }

        public void DeleteTitleReviewerByReviewerId(int reviewerID)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                stringBuilder.AppendLine("DeleteTitleReviewerByReviewerId execution start for reviewerId :" + reviewerID);
                var result = this.reviewerIndexDBContextRead.TitleReviewerlink.Where(o => o.ReviewerMasterID == reviewerID).ToList();
                foreach (var item in result)
                {
                    reviewerIndexDBContextRead.TitleReviewerlink.Remove(item);
                }
                reviewerIndexDBContextRead.SaveChanges();
                stringBuilder.AppendLine("DeleteTitleReviewerByReviewerId execution end for reviewerId :" + reviewerID);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, stringBuilder);
                throw;
            }
            finally
            {
                _logger.WriteStringBuilderToUserLogAndClear(stringBuilder);
            }
        }      

        public void DeleteJournalByReviewerId(int reviewerId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {                
                stringBuilder.AppendLine("DeleteJournalByReviewerId execution start for reviewerId :" + reviewerId);
                var result = this.reviewerIndexDBContextRead.JournalReviewerLink.Where(o => o.ReviewerMasterID == reviewerId).ToList();
                foreach (var item in result)
                {
                    reviewerIndexDBContextRead.JournalReviewerLink.Remove(item);
                }
                reviewerIndexDBContextRead.SaveChanges();
                stringBuilder.AppendLine("DeleteJournalByReviewerId execution end for reviewerId :" + reviewerId);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, stringBuilder);
                throw;
            }
            finally
            {
                _logger.WriteStringBuilderToUserLogAndClear(stringBuilder);
            }
        }

       
        /// <summary>
        /// Loads manuscript/reviewers details on click of Assign Manuscript button, txtManuscriptId textbox and after save/submit.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="msid"></param>
        /// <param name="reviewersId"></param>
        /// <returns></return>


        public void DeleteReviewerMailByReviewerId(int reviewerId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                stringBuilder.AppendLine("DeleteReviewerMailByReviewerId execution start for reviewerId :" + reviewerId);
                var result = this.reviewerIndexDBContextRead.ReviewerMailLink.Where(o => o.ReviewerMasterID == reviewerId).ToList();
                foreach (var item in result)
                {
                    reviewerIndexDBContextRead.ReviewerMailLink.Remove(item);
                }
                reviewerIndexDBContextRead.SaveChanges();
                stringBuilder.AppendLine("DeleteReviewerMailByReviewerId execution end for reviewerId :" + reviewerId);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, stringBuilder);
                throw;
            }
            finally
            {
                _logger.WriteStringBuilderToUserLogAndClear(stringBuilder);
            }
        }

        public void DeleteReferenceLinkByReviewerId(int reviewerId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                stringBuilder.AppendLine("DeleteReferenceLinkByReviewerId execution start for reviewerId :" + reviewerId);
                var result = this.reviewerIndexDBContextRead.ReferenceReviewerlink.Where(o => o.ReviewerMasterID == reviewerId).ToList();
                foreach (var item in result)
                {
                    reviewerIndexDBContextRead.ReferenceReviewerlink.Remove(item);
                }
                reviewerIndexDBContextRead.SaveChanges();
                stringBuilder.AppendLine("DeleteReferenceLinkByReviewerId execution end for reviewerId :" + reviewerId);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, stringBuilder);
                throw;
            }
            finally
            {
                _logger.WriteStringBuilderToUserLogAndClear(stringBuilder);
            }
        }

        

        public IEnumerable<spmsdisplaymanuscriptdetails_Result> DisplayManuscriptDetails(string key = null, string msid = null, string reviewersId = null)
        {
            try
            {
                var strkey = key != null
                    ? new SqlParameter("key", key)
                    : new SqlParameter("key", "");
                var strMsid = msid != null
                    ? new SqlParameter("msid", msid)
                    : new SqlParameter("msid", null);
                var strReviewerIds = reviewersId != null
                    ? new SqlParameter("ReviewerId", reviewersId)
                    : new SqlParameter("ReviewerId", null);

                var reviewerMsDetailsLists = this.reviewerIndexDBContextRead.Database.SqlQuery
                                         <spmsdisplaymanuscriptdetails_Result>("exec spmsdisplaymanuscriptdetailsNew @key, @msid, @ReviewerId", strkey, strMsid, strReviewerIds).ToList();
                return reviewerMsDetailsLists;
            }
            catch (Exception ex)
            {
                StringBuilder str = new StringBuilder();
                str.Append("Exception occured while loading records on " + key + ". MSID : " + msid + " , " + reviewersId + ".");
                _logger.LogException(ex, str);
                throw;
            } 
        }
        /// <summary>
        /// Check selected reviewer is duplicate or not for selected manuscript and journal. 
        /// </summary>
        /// <param name="journalid"></param>
        /// <param name="key"></param>
        /// <param name="msid"></param>
        /// <param name="reviewersId"></param>
        /// <returns></returns>
        public virtual int CheckDuplicates(int journalid, string key = null, string msid = null,
            string reviewersId = null)
        {
            try
            {
                var strkey = key != null
                    ? new SqlParameter("key", key)
                    : new SqlParameter("key", "");
                var strMsid = msid != null
                    ? new SqlParameter("msid", msid)
                    : new SqlParameter("msid", null);
                var strReviewerIds = reviewersId != null
                    ? new SqlParameter("ReviewerId", reviewersId)
                    : new SqlParameter("ReviewerId", null);
                var strJournalId = journalid != null
                    ? new SqlParameter("JournalID", journalid)
                    : new SqlParameter("JournalID", null);

                var chkCount = this.reviewerIndexDBContextRead.Database.SqlQuery
                    <int>("exec spmsdisplaymanuscriptdetailsNew @key, @msid, @ReviewerId, @JournalID", strkey, strMsid,
                        strReviewerIds, strJournalId).ToList();

                return chkCount.Count;
            }
            catch(Exception ex)
            {
                StringBuilder str = new StringBuilder();
                str.Append("Exception occured while checking duplicate reviewer for given Manuscript : " + key + ", MSID : " + msid + " , journalid : " + reviewersId + ".");
                _logger.LogException(ex, str);
                throw;
            }
        }
        /// <summary>
        /// It save the below Master detials of manuscript in table [MSReviewersSuggestion].
        /// </summary>
        /// <param name="key"></param>
        /// <param name="msid"></param>
        /// <param name="ddlTask"></param>
        /// <param name="rollId"></param>
        /// <param name="jobType"></param>
        /// <param name="articleTitle"></param>
        /// <param name="ddlJournalId"></param>
        /// <param name="user"></param>
        /// <param name="reviewerMasterId"></param>
        /// <param name="isAssociateFinalSubmit"></param>
        /// <param name="msReviewerSuggestionId"></param>
        /// <returns></returns>
        public virtual int SaveReviewersSuggestion(string key, string msid, int ddlTask, int rollId, char jobType,
            string articleTitle, int ddlJournalId, string user, int reviewerMasterId, int isAssociateFinalSubmit,
            int msReviewerSuggestionId)
        {
            try
            {
                var strkey = key != null
                    ? new SqlParameter("key", key)
                    : new SqlParameter("key", "INSERT");

                var strMsid = msid != null
                    ? new SqlParameter("msid", msid)
                    : new SqlParameter("msid", null);

                var strddlTask = ddlTask != null
                    ? new SqlParameter("SMTaskID", ddlTask)
                    : new SqlParameter("SMTaskID", null);

                var strrollId = rollId != null
                    ? new SqlParameter("RoleID", rollId)
                    : new SqlParameter("RoleID", null);

                var strjobType = jobType != null
                    ? new SqlParameter("JobType", jobType)
                    : new SqlParameter("JobType", null);

                var strarticleTitle = articleTitle != null
                    ? new SqlParameter("ArticleTitle", articleTitle)
                    : new SqlParameter("ArticleTitle", null);

                var strddlJournalId = ddlJournalId != null
                    ? new SqlParameter("JournalID", ddlJournalId)
                    : new SqlParameter("JournalID", null);

                var struser = user != null
                    ? new SqlParameter("AnalystUserID", user)
                    : new SqlParameter("AnalystUserID", null);

                var strreviewerMasterId = reviewerMasterId != null
                    ? new SqlParameter("ReviewerMasterID", reviewerMasterId)
                    : new SqlParameter("ReviewerMasterID", null);

                var strisAssociateFinalSubmit = isAssociateFinalSubmit != null
                    ? new SqlParameter("IsAssociateFinalSubmit", isAssociateFinalSubmit)
                    : new SqlParameter("IsAssociateFinalSubmit", null);

                var strmsReviewerSuggestionId = msReviewerSuggestionId != null
                    ? new SqlParameter("MSReviewersSuggestionID", msReviewerSuggestionId)
                    : new SqlParameter("MSReviewersSuggestionID", null);

                var iD = new SqlParameter()
                {
                    ParameterName = "ID",
                    SqlDbType = SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };

                var rowNum = this.reviewerIndexDBContextRead.Database.ExecuteSqlCommand(
                    "exec SPMSReviewersSuggestion @Key, @MSID, @SMTaskID, @RoleID,@JobType, @ArticleTitle, @JournalID, @AnalystUserID, @ReviewerMasterID, @IsAssociateFinalSubmit, @MSReviewersSuggestionID, @ID OUT",
                    strkey, strMsid, strddlTask, strrollId, strjobType, strarticleTitle, strddlJournalId, struser,
                    strreviewerMasterId,
                    strisAssociateFinalSubmit, strmsReviewerSuggestionId, iD);
                return Convert.ToInt32(iD.Value);
            }
            catch (Exception ex)
            {
                StringBuilder str = new StringBuilder();
                str.Append("Exception occured while saving/submitting the reviewer details : reviewer ID - " + reviewerMasterId + ", jobType : " + jobType + " , articleTitle : " + articleTitle + " isAssociateFinalSubmit " + isAssociateFinalSubmit + ".");
                _logger.LogException(ex, str);
                throw;
            }
        }

        public int IsTitleExist(string name , string msID)
        {
           int titleId = 0; 
           var IsExist =  reviewerIndexDBContextRead.TitleMaster.Where(o => o.Name.ToLower() == name.ToLower() && o.MScriptID.ToLower() == msID.ToLower()).FirstOrDefault();
           if (IsExist != null)
           {
               titleId = IsExist.TitleID;
           }
           return titleId;
        }

        /// <summary>
        /// It save the below Master detials of manuscript in table [MSReviewersSuggestionInfo].
        /// </summary>
        /// <param name="key"></param>
        /// <param name="rowNum"></param>
        /// <param name="reviewerMasterId"></param>
        /// <param name="user"></param>
        /// <param name="chk"></param>
        /// <param name="isAssociateFinalSubmit"></param>
        /// <param name="articleTitle"></param>
        /// <param name="msid"></param>
        public virtual void SaveReviewersSuggestionInfo(string key, int rowNum, int reviewerMasterId, string user,
            bool chk, int isAssociateFinalSubmit, string articleTitle, string msid)
        {
            try
            {
                var strkey = key != null
                    ? new SqlParameter("key", key)
                    : new SqlParameter("key", "INSERT");

                var strrowNum = rowNum != 0
                    ? new SqlParameter("MSReviewersSuggestionID", rowNum)
                    : new SqlParameter("MSReviewersSuggestionID", null);

                var strreviewerMasterId = reviewerMasterId != null
                    ? new SqlParameter("ReviewerMasterID", reviewerMasterId)
                    : new SqlParameter("ReviewerMasterID", null);

                var struser = user != null
                    ? new SqlParameter("CreatedBy", user)
                    : new SqlParameter("CreatedBy", null);

                var strchk = chk != null
                    ? new SqlParameter("IsChecked", chk)
                    : new SqlParameter("IsChecked", null);

                var strisAssociateFinalSubmit = isAssociateFinalSubmit != null
                    ? new SqlParameter("IsAssociateFinalSubmit", isAssociateFinalSubmit)
                    : new SqlParameter("IsAssociateFinalSubmit", null);

                var strarticleTitle = articleTitle != null
                    ? new SqlParameter("ArticleTitle", articleTitle)
                    : new SqlParameter("ArticleTitle", null);

                var strMsid = msid != null
                    ? new SqlParameter("msid", msid)
                    : new SqlParameter("msid", null);

                this.reviewerIndexDBContextRead.Database.ExecuteSqlCommand(
                    "exec SPMSReviewersSuggestionInfo @Key, @MSReviewersSuggestionID, @ReviewerMasterID, @CreatedBy, @IsChecked, @IsAssociateFinalSubmit, @ArticleTitle, @MSID",
                    strkey, strrowNum, strreviewerMasterId, struser, strchk, strisAssociateFinalSubmit, strarticleTitle,
                    strMsid);
            }
            catch (Exception ex)
            {
                StringBuilder str = new StringBuilder();
                str.Append("Exception occured while saving/submitting the reviewer details : reviewer ID - " + reviewerMasterId + ", user : " + user + " , articleTitle : " + articleTitle + " isAssociateFinalSubmit " + isAssociateFinalSubmit + ".");
                _logger.LogException(ex, str);
                throw;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    reviewerIndexDBContextRead.Dispose();
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


