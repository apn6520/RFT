using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Contracts
using Entities = TransferDesk.Contracts.Manuscript.Entities;
using DTOs = TransferDesk.Contracts.Manuscript.DTO;
//Validations
using Validations = TransferDesk.BAL.Manuscript.Validations;
//UnitOfWork Manuscript Screening
using TransferDesk.DAL.Manuscript.UnitOfWork;
using TransferDesk.DAL.Manuscript.Repositories;
using TransferDesk.Contracts.Manuscript.Entities;

//todo:keep a seperate read side/done needs refactor later

//Developer Hint: All additional init functions are for performance optimizations when needed

namespace TransferDesk.BAL.Manuscript
{
    public class ManuscriptScreeningBL : IDisposable
    {

        public ManuscriptDBRepositoryReadSide _manuscriptDBRepositoryReadSide { get; set; }
        public ManuscriptLoginDBRepositoryReadSide ManuscriptLoginDbRepositoryReadSide { get; set; }
        public Validations.ManuscriptValidations _manuscriptValidations { get; set; }

        public String _ConStringRead { get; set; }

        public String _ConStringWrite { get; set; }

        public ManuscriptScreeningBL()
        {
            //empty constructor
        }

        public ManuscriptScreeningBL(String ConStringRead, String ConStringWrite)
        {
            _ConStringRead = ConStringRead;
            _ConStringWrite = ConStringWrite;
            InitManuscriptScreeningBL();
        }

        public void InitManuscriptScreeningBL()
        {
            InitManuscriptDBRepositoryReadSide();

            InitManuscriptValidations();
        }

        public void InitManuscriptDBRepositoryReadSide()
        {
            _manuscriptDBRepositoryReadSide = new ManuscriptDBRepositoryReadSide(_ConStringWrite);
            ManuscriptLoginDbRepositoryReadSide = new ManuscriptLoginDBRepositoryReadSide(_ConStringWrite);
        }

        public void InitManuscriptValidations()
        {
            _manuscriptValidations = new Validations.ManuscriptValidations();

            if (_manuscriptDBRepositoryReadSide == null)
            {
                InitManuscriptDBRepositoryReadSide();
            }
        }



        public DTOs.ManuscriptScreeningDTO GetManuscriptScreeningDefaultDTO()
        {
            DTOs.ManuscriptScreeningDTO manuscriptDTO = new DTOs.ManuscriptScreeningDTO();

            manuscriptDTO.ErrorCategoriesList = _manuscriptDBRepositoryReadSide.GetErrorCategoryList("j");

            return manuscriptDTO;

        }

        public DTOs.ManuscriptBookScreeningDTO GetManuscriptBookScreeningDefaultDTO()
        {
            DTOs.ManuscriptBookScreeningDTO manuscriptDTO = new DTOs.ManuscriptBookScreeningDTO();

            manuscriptDTO.ErrorCategoriesList = _manuscriptDBRepositoryReadSide.GetErrorCategoryList("b");

            return manuscriptDTO;

        }

        public DTOs.ManuscriptScreeningDTO GetManuscriptScreeningDTO(int manuscriptID)
        {

            DTOs.ManuscriptScreeningDTO manuscriptDTO = GetManuscriptScreeningDefaultDTO();
            manuscriptDTO.Manuscript = _manuscriptDBRepositoryReadSide.GetManuscriptByID(manuscriptID);
            manuscriptDTO.OtherAuthors = _manuscriptDBRepositoryReadSide.GetOtherAuthors(manuscriptID);
            manuscriptDTO.manuscriptErrorCategoryList = _manuscriptDBRepositoryReadSide.GetManuscriptErrorCategoryList(manuscriptID);

            return manuscriptDTO;
        }

        public DTOs.ManuscriptBookScreeningDTO GetManuscriptBoookScreeningDTO(int bookid)
        {

            DTOs.ManuscriptBookScreeningDTO manuscriptDTO = GetManuscriptBookScreeningDefaultDTO();
            var manuscriptBookScreeningList = _manuscriptDBRepositoryReadSide.IsBookLoginIDAvailable(bookid);
            if (manuscriptBookScreeningList.Count > 0)
            {
                manuscriptDTO.ManuscriptBookScreening = manuscriptBookScreeningList.First();
                manuscriptDTO.ManuscriptBookLogin =
                    ManuscriptLoginDbRepositoryReadSide.GetManuscriptBookLoginByCrestID(
                        manuscriptDTO.ManuscriptBookScreening.BookLoginID);
                manuscriptDTO.manuscriptBookErrorCategory = _manuscriptDBRepositoryReadSide.GetManuscriptBookErrorCategoryList(manuscriptDTO.ManuscriptBookScreening.ID);
            }
            else
            {
                manuscriptDTO.ManuscriptBookLogin = ManuscriptLoginDbRepositoryReadSide.GetManuscriptBookLoginByCrestID(bookid);
            }
            return manuscriptDTO;
        }

        public bool SaveManuscriptScreening(DTOs.ManuscriptScreeningDTO manuscriptScreeningDTO, IDictionary<string, string> dataErrors)
        {
            if (_manuscriptValidations == null)
            {
                InitManuscriptValidations();

                _manuscriptValidations.Validate_MSID(manuscriptScreeningDTO.Manuscript, dataErrors);
                if (dataErrors.Count > 0)
                {
                    return false;
                }

            }
            //set starttime as system time for add of Manuscript
            if (manuscriptScreeningDTO.Manuscript.ID == 0)
            {
                manuscriptScreeningDTO.Manuscript.StartDate = System.DateTime.Now;
            }
            //Set Quality user id if role is quality
            if (manuscriptScreeningDTO.Manuscript.RoleID == 2)//todo: set constants for roles
            {
                manuscriptScreeningDTO.Manuscript.QualityUserID = manuscriptScreeningDTO.CurrentUserID;
            }
            else
            {
                manuscriptScreeningDTO.Manuscript.UserID = manuscriptScreeningDTO.CurrentUserID;
            }

            //if a revision occurs, add the same manuscript as new manuscript with a parent

            if (manuscriptScreeningDTO.AddedNewRevision == true)
            {
                manuscriptScreeningDTO.Manuscript.ParentManuscriptID = manuscriptScreeningDTO.Manuscript.ID;
                manuscriptScreeningDTO.Manuscript.ID = 0;

                //also each related details will be added, for new revised manuscript
                for (int counter = 0; counter < manuscriptScreeningDTO.OtherAuthors.Count; counter++)
                {
                    manuscriptScreeningDTO.OtherAuthors[counter].ID = 0;
                }
                for (int counter = 0; counter < manuscriptScreeningDTO.manuscriptErrorCategoryList.Count; counter++)
                {
                    manuscriptScreeningDTO.manuscriptErrorCategoryList[counter].ID = 0;
                }
            }

            //set what to save for manuscript screening
            manuscriptScreeningDTO.HasToSaveManuscript = true;
            manuscriptScreeningDTO.HasToSaveOtherAuthors = true;
            manuscriptScreeningDTO.HasToSaveErrorCategoriesList = true;

            //on MSID get crest id
            int MLID = GetCrestIdOnMSID(manuscriptScreeningDTO.Manuscript.MSID);
            int userId = GetUserId(manuscriptScreeningDTO.CurrentUserID);
            List<ManuscriptLoginDetails> manuscriptLoginDetailsList = new List<ManuscriptLoginDetails>();
            if (manuscriptScreeningDTO.Manuscript.IsAssociateFinalSubmit == true && manuscriptScreeningDTO.Manuscript.RoleID == 1 && MLID !=0)
            {
                var addManuscriptLoginDetails = new ManuscriptLoginDetails
                {
                    CrestId = MLID,
                    UserRoleId = userId,
                    JobStatusId = 7,
                    JobProcessStatusId = 21,
                    ServiceTypeStatusId = 5,
                    RoleId = 1,
                    CreatedDate = DateTime.Now,
                    AssignedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };
                manuscriptLoginDetailsList = GetManuscriptLoginDetailsData(MLID);
                if (manuscriptLoginDetailsList.Count > 0)
                {
                    manuscriptLoginDetailsList[0].SubmitedDate = DateTime.Now;
                    manuscriptLoginDetailsList[0].JobStatusId = 8;
                    manuscriptLoginDetailsList[0].JobProcessStatusId = 12;
                    manuscriptLoginDetailsList[0].ModifiedDate = DateTime.Now;
                }
                manuscriptLoginDetailsList.Add(addManuscriptLoginDetails);
            }
            var manuscriptlogindata = new ManuscriptLogin();
            if (manuscriptScreeningDTO.Manuscript.IsQualityFinalSubmit == true && MLID != 0)
            {
                manuscriptLoginDetailsList = GetManuscriptLoginDetailsDataForQA(MLID);
                if (manuscriptLoginDetailsList.Count > 0)
                {
                    manuscriptLoginDetailsList[0].SubmitedDate = DateTime.Now;
                    manuscriptLoginDetailsList[0].JobStatusId = 8;
                    manuscriptLoginDetailsList[0].JobProcessStatusId = 12;
                    manuscriptLoginDetailsList[0].ModifiedDate = DateTime.Now;
                }

                manuscriptlogindata = GetManuscriptLoginData(manuscriptScreeningDTO.Manuscript.MSID);
                manuscriptlogindata.ManuscriptStatusId = 8;
                manuscriptlogindata.ModifiedDate = DateTime.Now;
                manuscriptlogindata.ModifiedBy = manuscriptScreeningDTO.CurrentUserID;

            }

            ManuscriptScreeningUnitOfWork _manuscriptScreeningUnitOfWork = null;
            try
            {
                _manuscriptScreeningUnitOfWork = new ManuscriptScreeningUnitOfWork(_ConStringWrite);
                _manuscriptScreeningUnitOfWork.manuscriptScreeningDTO = manuscriptScreeningDTO;
                _manuscriptScreeningUnitOfWork.SaveManuscriptScreening();
                _manuscriptScreeningUnitOfWork.SaveChanges();//todo:change this function to update ids and save as seperate commit
                if (manuscriptScreeningDTO.Manuscript.IsAssociateFinalSubmit == true && manuscriptScreeningDTO.Manuscript.RoleID == 1 && MLID != 0)
                {
                    _manuscriptScreeningUnitOfWork._manuscriptLoginUnitOfWork.manuscriptLoginDTO = new DTOs.ManuscriptLoginDTO();
                    _manuscriptScreeningUnitOfWork._manuscriptLoginUnitOfWork.manuscriptLoginDTO.manuscriptLoginDetails = manuscriptLoginDetailsList;
                    _manuscriptScreeningUnitOfWork._manuscriptLoginUnitOfWork.SaveManuscriptLoginDetails();
                }
                if (manuscriptScreeningDTO.Manuscript.IsQualityFinalSubmit == true && MLID != 0)
                {
                    _manuscriptScreeningUnitOfWork._manuscriptLoginUnitOfWork.manuscriptLoginDTO = new DTOs.ManuscriptLoginDTO();
                    _manuscriptScreeningUnitOfWork._manuscriptLoginUnitOfWork.manuscriptLoginDTO.manuscriptLoginDetails = manuscriptLoginDetailsList;
                    _manuscriptScreeningUnitOfWork._manuscriptLoginUnitOfWork.manuscriptLoginDTO.manuscriptLogin = manuscriptlogindata;
                    _manuscriptScreeningUnitOfWork._manuscriptLoginUnitOfWork.manuscriptLoginDTO.userID = manuscriptScreeningDTO.CurrentUserID;

                    _manuscriptScreeningUnitOfWork._manuscriptLoginUnitOfWork.SaveManuscriptLoginDetails();
                    _manuscriptScreeningUnitOfWork._manuscriptLoginUnitOfWork.SaveManuscriptLogin();
                    _manuscriptScreeningUnitOfWork.manuscriptScreeningDTO.Manuscript.RoleID = 2;
                    _manuscriptScreeningUnitOfWork.SaveManuscriptScreening();
                    _manuscriptScreeningUnitOfWork.SaveChanges();
                }
                return true;
            }
            //exception will be raised up in the call stack
            finally
            {
                if (_manuscriptScreeningUnitOfWork != null)
                {
                    _manuscriptScreeningUnitOfWork.Dispose();
                }
            }
        }

        public int GetCrestIdOnMSID(string MSID)
        {
            var MLID = (from ML in ManuscriptLoginDbRepositoryReadSide.manuscriptDataContextRead.ManuscriptLogin
                        where ML.MSID == MSID && ML.ServiceTypeStatusId == 5 && ML.ManuscriptStatusId == 7
                        orderby ML.Id descending
                        select ML.Id).FirstOrDefault();

            return Convert.ToInt32(MLID);
        }


        public int GetBookLoginIDOnChapterNumber(string ChpNumber)
        {
            var MLID = (from ML in ManuscriptLoginDbRepositoryReadSide.manuscriptDataContextRead.ManuscriptBookLogin
                        where ML.ChapterNumber==ChpNumber && ML.ServiceTypeID==5 && ML.ManuscriptStatusID == 7                      
                       orderby ML.ID descending
                        select ML.ID).FirstOrDefault();

            return Convert.ToInt32(MLID);
        }

        public int GetUserId(string userID)
        {
            var serivceType = (from UR in ManuscriptLoginDbRepositoryReadSide.manuscriptDataContextRead.UserRoles
                               where UR.UserID == userID && UR.RollID == 1 && UR.IsActive == true && UR.ServiceTypeId == 5
                               select UR.ID).FirstOrDefault();

            return Convert.ToInt32(serivceType);
        }
        public List<Entities.ManuscriptLoginDetails> GetManuscriptLoginDetailsData(int crestID)
        {
            var fetchedMLD = (from MLD in ManuscriptLoginDbRepositoryReadSide.manuscriptDataContextRead.ManuscriptLoginDetails
                              where MLD.CrestId == crestID && MLD.ServiceTypeStatusId == 5 && MLD.JobProcessStatusId == 11 && MLD.JobStatusId == 7
                              select MLD).ToList();

            return fetchedMLD;
        }
        
        public List<Entities.ManuscriptBookLoginDetails> GetManuscriptBookLoginDetailsData(int bookLoginId)
        {
            var fetchedMLD = (from MBLD in ManuscriptLoginDbRepositoryReadSide.manuscriptDataContextRead.ManuscriptBookLoginDetails
                              where MBLD.ManuscriptBookLoginId == bookLoginId && MBLD.ServiceTypeStatusId == 5 && MBLD.JobProcessStatusId == 11 && MBLD.JobStatusId == 7
                              select MBLD).ToList();

            return fetchedMLD;
        }

        public List<Entities.ManuscriptLoginDetails> GetManuscriptLoginDetailsDataForQA(int crestID)
        {
            var fetchedMLD = (from MLD in ManuscriptLoginDbRepositoryReadSide.manuscriptDataContextRead.ManuscriptLoginDetails
                              where MLD.CrestId == crestID && MLD.ServiceTypeStatusId == 5 && MLD.JobProcessStatusId == 22 && MLD.JobStatusId == 7
                              select MLD).ToList();

            return fetchedMLD;
        }
        public Entities.ManuscriptLogin GetManuscriptLoginData(string MSID)
        {
            var MLID = (from ML in ManuscriptLoginDbRepositoryReadSide.manuscriptDataContextRead.ManuscriptLogin
                        where ML.MSID == MSID && ML.ServiceTypeStatusId == 5 && ML.ManuscriptStatusId == 7
                        orderby ML.Id descending
                        select ML).FirstOrDefault();

            return MLID;
        }

        public bool SaveManuscriptBookScreening(DTOs.ManuscriptBookScreeningDTO manuscriptBookScreeningDto, IDictionary<string, string> dataErrors)
        {
            if (_manuscriptValidations == null)
            {
                InitManuscriptValidations();

                if (dataErrors.Count > 0)
                {
                    return false;
                }

            }

            //set starttime as system time for add of Manuscript
            if (manuscriptBookScreeningDto.ManuscriptBookScreening.ID == 0)
            {
                manuscriptBookScreeningDto.ManuscriptBookScreening.CreatedDate = System.DateTime.Now;
            }

            //Set Quality user id if role is quality
            if (manuscriptBookScreeningDto.ManuscriptBookScreening.RollID == 2)//todo: set constants for roles
            {
                manuscriptBookScreeningDto.ManuscriptBookScreening.QualityUserID = manuscriptBookScreeningDto.CurrentUserID;
            }
            else
            {
                manuscriptBookScreeningDto.ManuscriptBookScreening.AssociateUserID = manuscriptBookScreeningDto.CurrentUserID;
            }
            //set what to save for manuscript screening
            manuscriptBookScreeningDto.HasToSaveManuscriptBookScreening = true;
            manuscriptBookScreeningDto.HasToSaveErrorCategoriesList = true;

            int userId = GetUserId(manuscriptBookScreeningDto.CurrentUserID);
            List<ManuscriptBookLoginDetails> manuscriptBookLoginDetailsList = new List<ManuscriptBookLoginDetails>();
            if (manuscriptBookScreeningDto.ManuscriptBookScreening.IsAssociateFinalSubmit == true && manuscriptBookScreeningDto.ManuscriptBookScreening.RollID == 1 && manuscriptBookScreeningDto.ManuscriptBookLogin.ID != 0)
            {
                var addManuscriptLoginDetails = new ManuscriptBookLoginDetails
                {
                    ManuscriptBookLoginId = manuscriptBookScreeningDto.ManuscriptBookLogin.ID,
                    UserRoleId = userId,
                    JobStatusId = 7,
                    JobProcessStatusId = 21,
                    ServiceTypeStatusId = 5,
                    RoleId = 1,
                    CreatedDate = DateTime.Now,
                    AssignedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };
                manuscriptBookLoginDetailsList = GetManuscriptBookLoginDetailsData(manuscriptBookScreeningDto.ManuscriptBookLogin.ID);
                if (manuscriptBookLoginDetailsList.Count > 0)
                {
                    manuscriptBookLoginDetailsList[0].SubmitedDate = DateTime.Now;
                    manuscriptBookLoginDetailsList[0].JobStatusId = 8;
                    manuscriptBookLoginDetailsList[0].JobProcessStatusId = 12;
                    manuscriptBookLoginDetailsList[0].ModifiedDate = DateTime.Now;
                }
                manuscriptBookLoginDetailsList.Add(addManuscriptLoginDetails);
            }

            ManuscriptScreeningUnitOfWork _manuscriptScreeningUnitOfWork = null;

            try
            {

                _manuscriptScreeningUnitOfWork = new ManuscriptScreeningUnitOfWork(_ConStringWrite);

                _manuscriptScreeningUnitOfWork.manuscriptBookScreeningDTO = manuscriptBookScreeningDto;
                _manuscriptScreeningUnitOfWork.SaveManuscriptBookScreening();
                _manuscriptScreeningUnitOfWork.UpdateManuscriptBookLoginInfo();
                _manuscriptScreeningUnitOfWork.SaveBookChanges();//todo:change this function to update ids and save as seperate commit

                if (manuscriptBookScreeningDto.ManuscriptBookScreening.IsAssociateFinalSubmit == true && manuscriptBookScreeningDto.ManuscriptBookScreening.RollID == 1 && manuscriptBookScreeningDto.ManuscriptBookLogin.ID != 0)
                {
                    _manuscriptScreeningUnitOfWork._manuscriptLoginUnitOfWork.manuscriptBookLoginDTO = new DTOs.ManuscriptBookLoginDTO();
                    _manuscriptScreeningUnitOfWork._manuscriptLoginUnitOfWork.manuscriptBookLoginDTO.manuscriptBookLoginDetails = manuscriptBookLoginDetailsList;
                    _manuscriptScreeningUnitOfWork._manuscriptLoginUnitOfWork.SaveManuscriptBookLoginDetails();
                }
                return true;
            }
            //exception will be raised up in the call stack
            finally
            {
                if (_manuscriptScreeningUnitOfWork != null)
                {
                    _manuscriptScreeningUnitOfWork.Dispose();
                }
            }
        }


        public void GetMailDetailsMsScreening(Dictionary<String, String> dicReplace, int reviewerMasterID, int? msReviewersSuggestionID, string userID)
        {
            Entities.Manuscript manuscriptScreening = _manuscriptDBRepositoryReadSide.GetManuscriptByID(msReviewersSuggestionID);
            var journalName = _manuscriptDBRepositoryReadSide.GetJounralName(manuscriptScreening.JournalID);
            Entities.Employee associateUserInfo = null;
            Entities.Employee qualityUserInfo = null;
            if (!string.IsNullOrEmpty(manuscriptScreening.UserID)) //Analyst User ID
            {
                associateUserInfo = _manuscriptDBRepositoryReadSide.GetAssociateInfo(manuscriptScreening.UserID);
            }
            if (!string.IsNullOrEmpty(userID)) //Quality User ID
            {
                qualityUserInfo = _manuscriptDBRepositoryReadSide.GetAssociateInfo(userID);
            }
            dicReplace.Add("[manuscriptNumber]", manuscriptScreening.MSID);
            dicReplace.Add("[journalname]", journalName.ToString());
            dicReplace.Add("[QAname]", qualityUserInfo.EmpName);
            dicReplace.Add("[Analystname]", associateUserInfo.EmpName);
            dicReplace.Add("[QAEmail]", qualityUserInfo.alternateEmail);
            dicReplace.Add("[AnalystEmail]", associateUserInfo.alternateEmail);

        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
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
