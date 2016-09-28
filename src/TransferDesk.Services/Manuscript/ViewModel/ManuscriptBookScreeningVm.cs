using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.DTO;
using TransferDesk.Contracts.Manuscript.Entities;

namespace TransferDesk.Services.Manuscript.ViewModel
{
    public class ManuscriptBookScreeningVm
    {
        private ManuscriptBookScreeningDTO _msDTO;

        internal ManuscriptBookScreeningDTO FetchDTO
        {
            get
            {
                ListErrorCategoryVMToDTO();
                return _msDTO;
            }
        }

        private List<ManuscriptErrorCategoryVM> _ErrorCategoryVMList;

        public bool AddedNewRevision
        {
            get { return _msDTO.AddedNewRevision; }
            set { _msDTO.AddedNewRevision = value; }
        }

        public string EmployeeName { get; set; }

        public ManuscriptBookScreeningVm(ManuscriptBookScreeningDTO manuscriptDTO)
        {
            _msDTO = manuscriptDTO;
            ListErrorCategoryVMFromDTO();
        }

        public ManuscriptBookScreeningVm()
        {
            _msDTO = new ManuscriptBookScreeningDTO();
        }

        public List<ManuscriptErrorCategoryVM> ListManuscriptErrorCategoriesVM
        {
            get
            {
                return _ErrorCategoryVMList;
            }
            //todo:during set fill dto using value that comes in as list
            set
            {
                _ErrorCategoryVMList = value;
                //ListErrorCategoryVMToDTO();
            }
        }

        private void ListErrorCategoryVMToDTO()
        {
            _msDTO.manuscriptBookErrorCategory = new List<ManuscriptBookErrorCategory>();
            //locate errorcategory in viewmodel and remove unselected with id 0 
            foreach (ManuscriptErrorCategoryVM manuscriptErrorCategoryVM in _ErrorCategoryVMList)
            {
                if (manuscriptErrorCategoryVM.ID > 0 || manuscriptErrorCategoryVM.IsSelected == true)
                {
                    ManuscriptBookErrorCategory manuscriptErrorCategory = new ManuscriptBookErrorCategory();
                    manuscriptErrorCategory.ID = manuscriptErrorCategoryVM.ID;
                    manuscriptErrorCategory.ErrorCategoryID = manuscriptErrorCategoryVM.ErrorCategoryID;
                    if (manuscriptErrorCategoryVM.ID > 0 && manuscriptErrorCategoryVM.IsSelected == false)
                    {
                        //todo: remove unchecked by user on progressive updates, instead of deletion
                        manuscriptErrorCategory.IsUncheckedByUser = true;
                    }
                    if (manuscriptErrorCategoryVM.ID > 0 && manuscriptErrorCategoryVM.IsSelected == true)
                    {
                        //todo: remove unchecked by user on progressive updates, instead of deletion
                        manuscriptErrorCategory.IsUncheckedByUser = false;
                    }
                    if (manuscriptErrorCategoryVM.ID == 0 && manuscriptErrorCategoryVM.IsSelected == true)
                    {
                        manuscriptErrorCategory.IsUncheckedByUser = false;
                    }
                    _msDTO.manuscriptBookErrorCategory.Add(manuscriptErrorCategory);
                }
            }
        }

        private List<ManuscriptErrorCategoryVM> ListErrorCategoryVMFromDTO()
        {
            _ErrorCategoryVMList = new List<ManuscriptErrorCategoryVM>();

            //First fetch error categories master from DTO into each VM
            foreach (ErrorCategory errorCategory in _msDTO.ErrorCategoriesList)
            {
                ManuscriptErrorCategoryVM manuscriptErrorCategoryVM = new ManuscriptErrorCategoryVM();
                manuscriptErrorCategoryVM.ErrorCategoryID = errorCategory.ID;
                manuscriptErrorCategoryVM.ErrorCategoryName = errorCategory.ErrorCategoryName;
                _ErrorCategoryVMList.Add(manuscriptErrorCategoryVM);
            }

            //todo:Now update the already selected in dto into list  if any
            foreach (ManuscriptBookErrorCategory manuscriptErrorCategory in _msDTO.manuscriptBookErrorCategory)
            {
                if (manuscriptErrorCategory.IsUncheckedByUser == true) continue;//continue with next iteration
                //ManuscriptErrorCategory tempManuscriptErrorCategory = null;
                //locate errorcategory in viewmodel and fill details 
                foreach (ManuscriptErrorCategoryVM manuscriptErrorCategoryVM in _ErrorCategoryVMList)
                {
                    if (manuscriptErrorCategoryVM.ErrorCategoryID == manuscriptErrorCategory.ErrorCategoryID)
                    {
                        //fill all details from manuscript error categories
                        manuscriptErrorCategoryVM.IsSelected = true;
                        manuscriptErrorCategoryVM.ID = manuscriptErrorCategory.ID;
                    }
                }
            }
            return _ErrorCategoryVMList;
        }

        [Key]
        public int BookScreeningID
        { get { return _msDTO.ManuscriptBookScreening.ID; } set { _msDTO.ManuscriptBookScreening.ID = value; } }

        public int? RollID
        {
            get { return _msDTO.ManuscriptBookScreening.RollID; }
            set { _msDTO.ManuscriptBookScreening.RollID = value; }
        }
        [Required(ErrorMessage = "Book Title")]
        public int BookTitleId
        {
            get { return _msDTO.ManuscriptBookLogin.BookMasterID; }
            set { _msDTO.ManuscriptBookLogin.BookMasterID = value; }
        }
        
        public int BookLoginID
        {
            get { return _msDTO.ManuscriptBookLogin.ID; }
            set { _msDTO.ManuscriptBookLogin.ID = value; }
        }

        [Required(ErrorMessage = "Chapter Number")]
        public string ChapterNumber
        {
            get { return _msDTO.ManuscriptBookLogin.ChapterNumber; }
            set { _msDTO.ManuscriptBookLogin.ChapterNumber = value; }
        }

        [Required(ErrorMessage = "Chapter Title")]
        public string ChapterTitle
        {
            get { return _msDTO.ManuscriptBookLogin.ChapterTitle; }
            set { _msDTO.ManuscriptBookLogin.ChapterTitle = value; }
        }
        [Required(ErrorMessage = "Received Date")]
        public DateTime ReceivedDate
        {
            get { return _msDTO.ManuscriptBookLogin.ReceivedDate; }
            set { _msDTO.ManuscriptBookLogin.ReceivedDate = value; }
        }
        [Required(ErrorMessage = "Page Count")]
        public int? PageCount
        {
            get { return _msDTO.ManuscriptBookLogin.PageCount; }
            set { _msDTO.ManuscriptBookLogin.PageCount = value; }
        }

        public string ShareDrivePath
        {
            get { return _msDTO.ManuscriptBookLogin.ShareDrivePath; }
            set { _msDTO.ManuscriptBookLogin.ShareDrivePath = value; }
        }

        [Required(ErrorMessage = "Cross check/iThenticate result %")]
        public int? Crosscheck_iThenticateResultID
        { get { return _msDTO.ManuscriptBookScreening.Crosscheck_iThenticateResultID; } set { _msDTO.ManuscriptBookScreening.Crosscheck_iThenticateResultID = value; } }

        [Required(ErrorMessage = "Highest iThenticate % from single source")]
        public int? Highest_iThenticateFromSingleSrc
        { get { return _msDTO.ManuscriptBookScreening.Highest_iThenticateFromSingleSrc; } set { _msDTO.ManuscriptBookScreening.Highest_iThenticateFromSingleSrc = value; } }

        [Required(ErrorMessage = "English language Quality % ")]
        public int? English_Lang_QualityID
        { get { return _msDTO.ManuscriptBookScreening.English_Lang_QualityID; } set { _msDTO.ManuscriptBookScreening.English_Lang_QualityID = value; } }

        [Required(ErrorMessage = "Ethics Complience %")]
        public int? Ethics_ComplianceID
        { get { return _msDTO.ManuscriptBookScreening.Ethics_ComplianceID; } set { _msDTO.ManuscriptBookScreening.Ethics_ComplianceID = value; } }

        public bool? QualityCheck
        {
            get
            {
                return _msDTO.ManuscriptBookScreening.QualityCheck.HasValue ? _msDTO.ManuscriptBookScreening.QualityCheck.Value : false;
            }
            set
            {
                _msDTO.ManuscriptBookScreening.QualityCheck = value;
            }
        }

        [Required(ErrorMessage = "Corresponding Author")]
        public string CorrespondingAuthor
        { get { return _msDTO.ManuscriptBookScreening.CorrespondingAuthor; } set { _msDTO.ManuscriptBookScreening.CorrespondingAuthor = value; } }
        [Required(ErrorMessage = "Corresponding Author Email")]
        public string CorrespondingAuthorEmail
        { get { return _msDTO.ManuscriptBookScreening.CorrespondingAuthorEmail; } set { _msDTO.ManuscriptBookScreening.CorrespondingAuthorEmail = value; } }

        public string CorrespondingAuthorAff
        { get { return _msDTO.ManuscriptBookScreening.CorrespondingAuthorAff; } set { _msDTO.ManuscriptBookScreening.CorrespondingAuthorAff = value; } }

        [Required(ErrorMessage = "iThenticate %")]
        public int? iThenticatePercentage
        { get { return _msDTO.ManuscriptBookScreening.iThenticatePercentage; } set { _msDTO.ManuscriptBookScreening.iThenticatePercentage = value; } }

        public bool? Accurate
        { get { return _msDTO.ManuscriptBookScreening.Accurate; } set { _msDTO.ManuscriptBookScreening.Accurate = value; } }

        public string ErrorDescription
        { get { return _msDTO.ManuscriptBookScreening.ErrorDescription; } set { _msDTO.ManuscriptBookScreening.ErrorDescription = value; } }

        public int? OverallAnalysisID
        { get { return _msDTO.ManuscriptBookScreening.OverallAnalysisID; } set { _msDTO.ManuscriptBookScreening.OverallAnalysisID = value; } }

        public bool? IsAccurate
        { get { return _msDTO.ManuscriptBookScreening.Accurate; } set { _msDTO.ManuscriptBookScreening.Accurate = value; } }

        public string Comments_English_Lang_Quality
        { get { return _msDTO.ManuscriptBookScreening.Comments_English_Lang_Quality; } set { _msDTO.ManuscriptBookScreening.Comments_English_Lang_Quality = value; } }

        public string Comments_Ethics_Compliance
        { get { return _msDTO.ManuscriptBookScreening.Comments_Ethics_Compliance; } set { _msDTO.ManuscriptBookScreening.Comments_Ethics_Compliance = value; } }

        public string Comments_Crosscheck_iThenticateResult
        { get { return _msDTO.ManuscriptBookScreening.Comments_Crosscheck_iThenticateResult; } set { _msDTO.ManuscriptBookScreening.Comments_Crosscheck_iThenticateResult = value; } }

        public string Comments_OverallAnalysis
        { get { return _msDTO.ManuscriptBookScreening.Comments_OverallAnalysis; } set { _msDTO.ManuscriptBookScreening.Comments_OverallAnalysis = value; } }

        public bool? IsAssociateFinalSubmit
        { get { return _msDTO.ManuscriptBookScreening.IsAssociateFinalSubmit; } set { _msDTO.ManuscriptBookScreening.IsAssociateFinalSubmit = value; } }

        public bool? IsQualityFinalSubmit
        { get { return _msDTO.ManuscriptBookScreening.IsQualityFinalSubmit; } set { _msDTO.ManuscriptBookScreening.IsQualityFinalSubmit = value; } }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public System.DateTime? QualityStartCheckDate
        { get { return _msDTO.ManuscriptBookScreening.QualityStartCheckDate; } set { _msDTO.ManuscriptBookScreening.QualityStartCheckDate = value; } }

        public System.DateTime? StartDate
        { get { return _msDTO.ManuscriptBookScreening.CreatedDate; } set { _msDTO.ManuscriptBookScreening.CreatedDate = value; } }

        public System.DateTime? AssociateFinalSubmitDate
        { get { return _msDTO.ManuscriptBookScreening.AssociateFinalSubmitDate; } set { _msDTO.ManuscriptBookScreening.AssociateFinalSubmitDate = value; } }

        public System.DateTime? QualityFinalSubmitDate
        { get { return _msDTO.ManuscriptBookScreening.QualityFinalSubmitDate; } set { _msDTO.ManuscriptBookScreening.QualityFinalSubmitDate = value; } }

        public string AssociateUserID
        { get { return _msDTO.ManuscriptBookScreening.AssociateUserID; } set { _msDTO.ManuscriptBookScreening.AssociateUserID = value; } }

    }
}
