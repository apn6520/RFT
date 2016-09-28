using System; 
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions; 
using DataContexts= TransferDesk.Contracts.ReviewerIndex.DataContext;
using Entities = TransferDesk.Contracts.ReviewerIndex.Entities;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using TransferDesk.Contracts.ReviewerIndex.ComplexTypes;
using System.Text;

namespace TransferDesk.DAL.ReviewerIndex.DataContext
{
    public class ReviewerIndexDBContext :DbContext , DataContexts.IReviewerIndexDataContext
    {
        private ReviewerIndexDBContext context;

        public ReviewerIndexDBContext(string ConString)
            : base(ConString)
        {
        }
        

        public ReviewerIndexDBContext(ReviewerIndexDBContext context)
        {
            this.context = context;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //disable initializer this is mainly for production version if we dont want to lose existing data
            Database.SetInitializer<ReviewerIndexDBContext>(null);

            modelBuilder.Properties<DateTime>()
                .Configure(c => c.HasColumnType("datetime2"));

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Entities.AffillationMaster>();
            modelBuilder.Entity<Entities.AffillationReviewerlink>();
            modelBuilder.Entity<Entities.AreaOfExpertiseMaster>();
            modelBuilder.Entity<Entities.AreaOfExpReviewerlink>();
            modelBuilder.Entity<Entities.DepartmentMaster>();
            modelBuilder.Entity<Entities.InstituteMaster>();
            modelBuilder.Entity<Entities.JournalReviewerLink>();
            modelBuilder.Entity<Entities.Location>();
            modelBuilder.Entity<Entities.Journal>();
            modelBuilder.Entity<Entities.JournalReviewerLink>();
            modelBuilder.Entity<Entities.ReferenceReviewerlink>();
            modelBuilder.Entity<Entities.ReviewerMailLink>();
            modelBuilder.Entity<Entities.ReviewerMaster>();
            modelBuilder.Entity<Entities.TitleMaster>();
            modelBuilder.Entity<Entities.TitleReviewerlink>();
            modelBuilder.Entity<Entities.StatusMaster>();
            //modelBuilder.Entity<TransferDesk.Contracts.ReviewerIndex.ComplexTypes.spGetReviewersList>();

        }

        public virtual DbSet<Entities.AffillationMaster> AffillationMaster { get; set; }
        public virtual DbSet<Entities.AffillationReviewerlink> AffillationReviewerlink { get; set; }
        public virtual DbSet<Entities.AreaOfExpertiseMaster> AreaOfExpertiseMaster { get; set; }
        public virtual DbSet<Entities.AreaOfExpReviewerlink> AreaOfExpReviewerlink { get; set; }
        public virtual DbSet<Entities.DepartmentMaster> DepartmentMaster { get; set; }
        public virtual DbSet<Entities.InstituteMaster> InstituteMaster { get; set; }
        public virtual DbSet<Entities.JournalReviewerLink> JournalReviewerLink { get; set; }
        public virtual DbSet<Entities.Location> Location { get; set; }
        public virtual DbSet<Entities.Journal> Journal { get; set; }
        public virtual DbSet<Entities.ReferenceReviewerlink> ReferenceReviewerlink { get; set; }
        public virtual DbSet<Entities.ReviewerMailLink> ReviewerMailLink { get; set; }
        public virtual DbSet<Entities.ReviewerMaster> ReviewerMaster { get; set; }
        public virtual DbSet<Entities.TitleMaster> TitleMaster { get; set; }
        public virtual DbSet<Entities.TitleReviewerlink> TitleReviewerlink { get; set; }
        public virtual DbSet<Entities.StatusMaster> StatusMaster { get; set; }

        public virtual DbSet<spGetReviewerDetails_Result> spGetReviewerDetails_Result { get; set; }
        public virtual DbSet<ReviewerEmails_Result> ReviewerEmails_Result { get; set; }
        public virtual DbSet<AreaOfExpertise_Result> AreaOfExpertise_Result { get; set; }
        public virtual DbSet<ReferenceLink_Result> ReferenceLink_Result { get; set; }
        public virtual DbSet<Journal_Result> Journal_Result { get; set; }
        public virtual DbSet<TitleAndAffillationMaster_Result> TitleAndAffillationMaster_Result { get; set; }      
        public virtual DbSet<AreaOfExpReviewerlink_Result> AreaOfExpReviewerlink_Result { get; set; }
        public virtual DbSet<TitleReviewerlinkMaster_Result> TitleReviewerlinkMaster_Result { get; set; }    
     



        public virtual ObjectResult<pr_LocationInfoForCleanData_Result> pr_LocationInfoForCleanData(Nullable<int> reviewerMasterID)
        {
            var reviewerMasterIDParameter = reviewerMasterID.HasValue ?
                new ObjectParameter("ReviewerMasterID", reviewerMasterID) :
                new ObjectParameter("ReviewerMasterID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<pr_LocationInfoForCleanData_Result>("pr_LocationInfoForCleanData", reviewerMasterIDParameter);
        }

        public virtual ObjectResult<pr_ReviewerDetails_Result> pr_ReviewerDetails(Nullable<int> selectedValue, string searchBy)
        {
            var selectedValueParameter = selectedValue.HasValue ?
                new ObjectParameter("SelectedValue", selectedValue) :
                new ObjectParameter("SelectedValue", typeof(int));

            var searchByParameter = searchBy != null ?
                new ObjectParameter("SearchBy", searchBy) :
                new ObjectParameter("SearchBy", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<pr_ReviewerDetails_Result>("pr_ReviewerDetails", selectedValueParameter, searchByParameter);
        }

        public virtual int spAddAffiliation(string keySearch, string tableName, string userID, string keySearchState, string keySearchCity, ObjectParameter iD)
        {
            var keySearchParameter = keySearch != null ?
                new ObjectParameter("keySearch", keySearch) :
                new ObjectParameter("keySearch", typeof(string));

            var tableNameParameter = tableName != null ?
                new ObjectParameter("tableName", tableName) :
                new ObjectParameter("tableName", typeof(string));

            var userIDParameter = userID != null ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(string));

            var keySearchStateParameter = keySearchState != null ?
                new ObjectParameter("keySearchState", keySearchState) :
                new ObjectParameter("keySearchState", typeof(string));

            var keySearchCityParameter = keySearchCity != null ?
                new ObjectParameter("keySearchCity", keySearchCity) :
                new ObjectParameter("keySearchCity", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spAddAffiliation", keySearchParameter, tableNameParameter, userIDParameter, keySearchStateParameter, keySearchCityParameter, iD);
        }

        public virtual ObjectResult<Nullable<int>> spAddLinkDetails(Nullable<int> execFlag, string name, string userID, string mScriptID)
        {
            var execFlagParameter = execFlag.HasValue ?
                new ObjectParameter("execFlag", execFlag) :
                new ObjectParameter("execFlag", typeof(int));

            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));

            var userIDParameter = userID != null ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(string));

            var mScriptIDParameter = mScriptID != null ?
                new ObjectParameter("MScriptID", mScriptID) :
                new ObjectParameter("MScriptID", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("spAddLinkDetails", execFlagParameter, nameParameter, userIDParameter, mScriptIDParameter);
        }

        public virtual string spGetReviewersList(string keySearchOne, string keySearchTwo, string searchCondition, string dDLOneValue, string dDLTwoValue, string reviewersList)
        {
            StringBuilder stringBuilder = new StringBuilder();

            try
            {
                stringBuilder.AppendLine("INFO DAL :ReviewerIndexDBContext > spGetReviewersList- Execution start");
                stringBuilder.AppendLine("INFO DAL :spGetReviewersList > keySearchOne:" + keySearchOne + " keySearchTwo:" + keySearchTwo
                  + " searchCondition:" + searchCondition + " dDLOneValue:" + dDLOneValue + " dDLTwoValue:" + dDLTwoValue + " reviewersList:" + reviewersList);

                var keySearchOneParameter = keySearchOne != null ?
                new ObjectParameter("KeySearchOne", keySearchOne) :
                new ObjectParameter("KeySearchOne", typeof(string));

                var keySearchTwoParameter = keySearchTwo != null ?
                    new ObjectParameter("KeySearchTwo", keySearchTwo) :
                    new ObjectParameter("KeySearchTwo", typeof(string));

                var searchConditionParameter = searchCondition != null ?
                    new ObjectParameter("SearchCondition", searchCondition) :
                    new ObjectParameter("SearchCondition", typeof(string));

                var dDLOneValueParameter = dDLOneValue != null ?
                    new ObjectParameter("DDLOneValue", dDLOneValue) :
                    new ObjectParameter("DDLOneValue", typeof(string));

                var dDLTwoValueParameter = dDLTwoValue != null ?
                    new ObjectParameter("DDLTwoValue", dDLTwoValue) :
                    new ObjectParameter("DDLTwoValue", typeof(string));

                var reviewersListParameter = reviewersList != null ?
                    new ObjectParameter("ReviewersList", reviewersList) :
                    new ObjectParameter("ReviewersList", typeof(string));


                var result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spGetReviewersList", keySearchOneParameter, keySearchTwoParameter, searchConditionParameter, dDLOneValueParameter, dDLTwoValueParameter, reviewersListParameter);
                return "";
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual ObjectResult<string> SpGetUser(string userId)
        {
            var userIdParameter = userId != null ?
                new ObjectParameter("userId", userId) :
                new ObjectParameter("userId", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("SpGetUser", userIdParameter);
        }

        public virtual ObjectResult<spmsdisplaymanuscriptdetails_Result> spmsdisplaymanuscriptdetails(string key, string msid, string reviewerId, Nullable<int> journalID)
        {
            var keyParameter = key != null ?
                new ObjectParameter("key", key) :
                new ObjectParameter("key", typeof(string));

            var msidParameter = msid != null ?
                new ObjectParameter("msid", msid) :
                new ObjectParameter("msid", typeof(string));

            var reviewerIdParameter = reviewerId != null ?
                new ObjectParameter("ReviewerId", reviewerId) :
                new ObjectParameter("ReviewerId", typeof(string));

            var journalIDParameter = journalID.HasValue ?
                new ObjectParameter("JournalID", journalID) :
                new ObjectParameter("JournalID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spmsdisplaymanuscriptdetails_Result>("spmsdisplaymanuscriptdetails", keyParameter, msidParameter, reviewerIdParameter, journalIDParameter);
        }

        public virtual int SPMSReviewersSuggestion(string key, string mSID, Nullable<int> sMTaskID, Nullable<int> roleID, string jobType, string articleTitle, Nullable<int> journalID, string analystUserID, Nullable<int> reviewerMasterID, Nullable<bool> isAssociateFinalSubmit, Nullable<int> mSReviewersSuggestionID, ObjectParameter iD)
        {
            var keyParameter = key != null ?
                new ObjectParameter("Key", key) :
                new ObjectParameter("Key", typeof(string));

            var mSIDParameter = mSID != null ?
                new ObjectParameter("MSID", mSID) :
                new ObjectParameter("MSID", typeof(string));

            var sMTaskIDParameter = sMTaskID.HasValue ?
                new ObjectParameter("SMTaskID", sMTaskID) :
                new ObjectParameter("SMTaskID", typeof(int));

            var roleIDParameter = roleID.HasValue ?
                new ObjectParameter("RoleID", roleID) :
                new ObjectParameter("RoleID", typeof(int));

            var jobTypeParameter = jobType != null ?
                new ObjectParameter("JobType", jobType) :
                new ObjectParameter("JobType", typeof(string));

            var articleTitleParameter = articleTitle != null ?
                new ObjectParameter("ArticleTitle", articleTitle) :
                new ObjectParameter("ArticleTitle", typeof(string));

            var journalIDParameter = journalID.HasValue ?
                new ObjectParameter("JournalID", journalID) :
                new ObjectParameter("JournalID", typeof(int));

            var analystUserIDParameter = analystUserID != null ?
                new ObjectParameter("AnalystUserID", analystUserID) :
                new ObjectParameter("AnalystUserID", typeof(string));

            var reviewerMasterIDParameter = reviewerMasterID.HasValue ?
                new ObjectParameter("ReviewerMasterID", reviewerMasterID) :
                new ObjectParameter("ReviewerMasterID", typeof(int));

            var isAssociateFinalSubmitParameter = isAssociateFinalSubmit.HasValue ?
                new ObjectParameter("IsAssociateFinalSubmit", isAssociateFinalSubmit) :
                new ObjectParameter("IsAssociateFinalSubmit", typeof(bool));

            var mSReviewersSuggestionIDParameter = mSReviewersSuggestionID.HasValue ?
                new ObjectParameter("MSReviewersSuggestionID", mSReviewersSuggestionID) :
                new ObjectParameter("MSReviewersSuggestionID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SPMSReviewersSuggestion", keyParameter, mSIDParameter, sMTaskIDParameter, roleIDParameter, jobTypeParameter, articleTitleParameter, journalIDParameter, analystUserIDParameter, reviewerMasterIDParameter, isAssociateFinalSubmitParameter, mSReviewersSuggestionIDParameter, iD);
        }

        public virtual ObjectResult<SPMSReviewersSuggestionInfo_Result> SPMSReviewersSuggestionInfo(string key, Nullable<int> mSReviewersSuggestionID, Nullable<int> reviewerMasterID, string createdBy, Nullable<bool> isChecked, Nullable<bool> isAssociateFinalSubmit, string articleTitle, string mSID, string modifiedBy, string modifiedDate, ObjectParameter iD)
        {
            var keyParameter = key != null ?
                new ObjectParameter("Key", key) :
                new ObjectParameter("Key", typeof(string));

            var mSReviewersSuggestionIDParameter = mSReviewersSuggestionID.HasValue ?
                new ObjectParameter("MSReviewersSuggestionID", mSReviewersSuggestionID) :
                new ObjectParameter("MSReviewersSuggestionID", typeof(int));

            var reviewerMasterIDParameter = reviewerMasterID.HasValue ?
                new ObjectParameter("ReviewerMasterID", reviewerMasterID) :
                new ObjectParameter("ReviewerMasterID", typeof(int));

            var createdByParameter = createdBy != null ?
                new ObjectParameter("CreatedBy", createdBy) :
                new ObjectParameter("CreatedBy", typeof(string));

            var isCheckedParameter = isChecked.HasValue ?
                new ObjectParameter("IsChecked", isChecked) :
                new ObjectParameter("IsChecked", typeof(bool));

            var isAssociateFinalSubmitParameter = isAssociateFinalSubmit.HasValue ?
                new ObjectParameter("IsAssociateFinalSubmit", isAssociateFinalSubmit) :
                new ObjectParameter("IsAssociateFinalSubmit", typeof(bool));

            var articleTitleParameter = articleTitle != null ?
                new ObjectParameter("ArticleTitle", articleTitle) :
                new ObjectParameter("ArticleTitle", typeof(string));

            var mSIDParameter = mSID != null ?
                new ObjectParameter("MSID", mSID) :
                new ObjectParameter("MSID", typeof(string));

            var modifiedByParameter = modifiedBy != null ?
                new ObjectParameter("ModifiedBy", modifiedBy) :
                new ObjectParameter("ModifiedBy", typeof(string));

            var modifiedDateParameter = modifiedDate != null ?
                new ObjectParameter("ModifiedDate", modifiedDate) :
                new ObjectParameter("ModifiedDate", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SPMSReviewersSuggestionInfo_Result>("SPMSReviewersSuggestionInfo", keyParameter, mSReviewersSuggestionIDParameter, reviewerMasterIDParameter, createdByParameter, isCheckedParameter, isAssociateFinalSubmitParameter, articleTitleParameter, mSIDParameter, modifiedByParameter, modifiedDateParameter, iD);
        }

        public virtual int spSaveUpdateReviewerDetails(string initials, string lastName, string firstName, string middleName, string name, Nullable<int> reviewerId, Nullable<int> instituteID, Nullable<int> deptId, string streetName, Nullable<int> cityID, Nullable<int> noOfPublications, string userID, ObjectParameter iD)
        {
            try
            {
                var initialsParameter = initials != null ?
               new ObjectParameter("Initials", initials) :
               new ObjectParameter("Initials", typeof(string));

                var lastNameParameter = lastName != null ?
                    new ObjectParameter("LastName", lastName) :
                    new ObjectParameter("LastName", typeof(string));

                var firstNameParameter = firstName != null ?
                    new ObjectParameter("FirstName", firstName) :
                    new ObjectParameter("FirstName", typeof(string));

                var middleNameParameter = middleName != null ?
                    new ObjectParameter("MiddleName", middleName) :
                    new ObjectParameter("MiddleName", typeof(string));

                var nameParameter = name != null ?
                    new ObjectParameter("Name", name) :
                    new ObjectParameter("Name", typeof(string));

                var reviewerIdParameter = reviewerId.HasValue ?
                    new ObjectParameter("ReviewerId", reviewerId) :
                    new ObjectParameter("ReviewerId", typeof(int));

                var instituteIDParameter = instituteID.HasValue ?
                    new ObjectParameter("InstituteID", instituteID) :
                    new ObjectParameter("InstituteID", typeof(int));

                var deptIdParameter = deptId.HasValue ?
                    new ObjectParameter("DeptId", deptId) :
                    new ObjectParameter("DeptId", typeof(int));

                var streetNameParameter = streetName != null ?
                    new ObjectParameter("StreetName", streetName) :
                    new ObjectParameter("StreetName", typeof(string));

                var cityIDParameter = cityID.HasValue ?
                    new ObjectParameter("CityID", cityID) :
                    new ObjectParameter("CityID", typeof(int));

                var noOfPublicationsParameter = noOfPublications.HasValue ?
                    new ObjectParameter("NoOfPublications", noOfPublications) :
                    new ObjectParameter("NoOfPublications", typeof(int));

                var userIDParameter = userID != null ?
                    new ObjectParameter("UserID", userID) :
                    new ObjectParameter("UserID", typeof(string));

                return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spSaveUpdateReviewerDetails", initialsParameter, lastNameParameter, firstNameParameter, middleNameParameter, nameParameter, reviewerIdParameter, instituteIDParameter, deptIdParameter, streetNameParameter, cityIDParameter, noOfPublicationsParameter, userIDParameter, iD);


            }
            catch (Exception)
            {

                throw;
            }
        }

        public virtual ObjectResult<spViewReviewerDetails_Result> spViewReviewerDetails(Nullable<int> titleId, Nullable<int> reviewerID)
        {
            var titleIdParameter = titleId.HasValue ?
                new ObjectParameter("TitleId", titleId) :
                new ObjectParameter("TitleId", typeof(int));

            var reviewerIDParameter = reviewerID.HasValue ?
                new ObjectParameter("ReviewerID", reviewerID) :
                new ObjectParameter("ReviewerID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spViewReviewerDetails_Result>("spViewReviewerDetails", titleIdParameter, reviewerIDParameter);
        }

        public virtual int UpdateReviewerTitle(Nullable<int> reviewerMasterID, string articleTitle, string mSID, Nullable<bool> isAssociateFinalSubmit, string createdBy)
        {
            var reviewerMasterIDParameter = reviewerMasterID.HasValue ?
                new ObjectParameter("ReviewerMasterID", reviewerMasterID) :
                new ObjectParameter("ReviewerMasterID", typeof(int));

            var articleTitleParameter = articleTitle != null ?
                new ObjectParameter("ArticleTitle", articleTitle) :
                new ObjectParameter("ArticleTitle", typeof(string));

            var mSIDParameter = mSID != null ?
                new ObjectParameter("MSID", mSID) :
                new ObjectParameter("MSID", typeof(string));

            var isAssociateFinalSubmitParameter = isAssociateFinalSubmit.HasValue ?
                new ObjectParameter("IsAssociateFinalSubmit", isAssociateFinalSubmit) :
                new ObjectParameter("IsAssociateFinalSubmit", typeof(bool));

            var createdByParameter = createdBy != null ?
                new ObjectParameter("CreatedBy", createdBy) :
                new ObjectParameter("CreatedBy", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateReviewerTitle", reviewerMasterIDParameter, articleTitleParameter, mSIDParameter, isAssociateFinalSubmitParameter, createdByParameter);
        }
    
    

    }
}
