using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.ComplexTypes.UserRole;
using TransferDesk.Contracts.Manuscript.DTO;
using TransferDesk.Contracts.Manuscript.Entities;
namespace TransferDesk.Services.Manuscript.ViewModel
{
    public class UserRoleVM
    {
        public UserRoleDTO _uDto;

        internal UserRoleDTO FetchDTO
        {
            get
            {
                return _uDto;
            }
        }

        public UserRoleVM()
        {
            _uDto = new UserRoleDTO();
        }
        public int ID
        {
            get { return _uDto.userroles.ID; }
            set { _uDto.userroles.ID = value; }
        }

        [Required(ErrorMessage = "User id")]
        public string UserID
        {
            get { return _uDto.userroles.UserID; }
            set { _uDto.userroles.UserID = value; }
        }

        [Required(ErrorMessage = "Role")]
        public int RollID
        {
            get { return _uDto.userroles.RollID; }
            set { _uDto.userroles.RollID = value; }
        }

        [Required(ErrorMessage = "Service Type")]
        public int ServiceTypeID
        {
            get { return _uDto.userroles.ServiceTypeId; }
            set { _uDto.userroles.ServiceTypeId = value; }
        }

        [Required(ErrorMessage = "Sliding Scale")]
        public int? SlidingScaleId
        {
            get { return _uDto.userroles.SlidingScaleId; }
            set { _uDto.userroles.SlidingScaleId = value; }
        }

        public int? TeamId
        {
            get { return _uDto.userroles.StatusTeamId; }
            set { _uDto.userroles.StatusTeamId = value; }
        }

        public bool IsActive
        {
            get { return _uDto.userroles.IsActive; }
            set { _uDto.userroles.IsActive = value; }
        }


        public List<int> SelectedJournalIDs
        {
            get { return _uDto.SelectedJournalIDs; }
            set { _uDto.SelectedJournalIDs = value; }
        }
        public List<int> SelectedBookIDs
        {
            get { return _uDto.SelectedBookIDs; }
            set { _uDto.SelectedBookIDs = value; }
        }


        public List<int> SelectedJournalID
        {
            get { return _uDto.SelectedJournalID; }
            set { _uDto.SelectedJournalID = value; }
        }


        public List<int> SelectedBookID
        {
            get { return _uDto.SelectedBookID; }
            set { _uDto.SelectedBookID = value; }
        }
        public string loginuser
        {
            get { return _uDto.loginuser; }
            set { _uDto.loginuser = value; }
        }
        [Required(ErrorMessage = "Name")]
        public string Name { get; set; }
        public bool IsJournal { get; set; }
        public bool IsBook { get; set; }
        public string EmployeeName { get; set; }
        public IEnumerable<Role> Role { get; set; }
        public IEnumerable<pr_GetUserRoleDetails_Result> userRoles { get; set; }
        public List<SlidingScale> SlidingScalelist { get; set; }
        public IEnumerable<Journal> JournalList { get; set; }
        public IEnumerable<BookMaster> BookList { get; set; }
        public List<StatusMaster> TeamList { get; set; }
        public List<pr_GetUserMasterDetails_Result> usermasterdetailslist { get; set; }
        public List<StatusMaster> ServiceType { get; set; }
    }
}
