using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.BAL.Manuscript;
using TransferDesk.Contracts.Manuscript.DTO;
using TransferDesk.Services.Manuscript.ViewModel;

namespace TransferDesk.Services.Manuscript
{
    public class AdminDashBoardService
    {
        AdminDashBoardDTO adminDashBoardDTO { get; set; }
        AdminDashBoardBL adminDashBoardBL { get; set; }

        public AdminDashBoardService(String conString)
        {
            adminDashBoardDTO = new AdminDashBoardDTO();
            adminDashBoardBL = new AdminDashBoardBL(conString);
        }

        public bool AllocateManuscriptToUser(AdminDasboardVM adminDasboardVM)
        {
            GetManuscriptValues(adminDasboardVM);
            adminDashBoardDTO.AssociateName = adminDasboardVM.AssociateNameVM;
            return adminDashBoardBL.AllocateManuscriptToUser(adminDashBoardDTO);
        }

        public bool UnallocateManuscriptFromUser(AdminDasboardVM adminDasboardVM)
        {
            GetManuscriptValues(adminDasboardVM);
            return adminDashBoardBL.updateManuscriptLoginDeatils(adminDashBoardDTO);

        }

        private void GetManuscriptValues(AdminDasboardVM adminDasboardVM)
        {
            adminDashBoardDTO.CrestId = adminDasboardVM.CrestIdVM;
            adminDashBoardDTO.ServiceType = adminDasboardVM.ServiceTypeVM;
            adminDashBoardDTO.JobProcessingStatus = adminDasboardVM.JobProcessingStatusVM;
            adminDashBoardDTO.Role = adminDasboardVM.RoleVM;
            adminDashBoardDTO.JobType = adminDasboardVM.JobType;
        }
        public bool OnHoldManuscript(AdminDasboardVM adminDasboardVM)
        {
            GetManuscriptValues(adminDasboardVM);
            return adminDashBoardBL.updateManuscriptLoginDeatilsForHold(adminDashBoardDTO);

        }

        public AdminDasboardVM CreateAdminDasboardVm(string AssociateNameVM, int CrestIdVM, string ServiceTypeVM,
            string JobProcessingStatusVM, string RoleVM, string jobType)
        {
            var adminDash = new AdminDasboardVM();
            adminDash.CrestIdVM = CrestIdVM;
            adminDash.AssociateNameVM = AssociateNameVM;
            adminDash.ServiceTypeVM = ServiceTypeVM;
            adminDash.JobProcessingStatusVM = JobProcessingStatusVM;
            adminDash.RoleVM = RoleVM;
            adminDash.JobType = jobType;
            return adminDash;
        }

        public AdminDasboardVM CreateAdminDasboardVm(AdminFunctionalityEnum adminFunctionalityEnum)
        {
            var adminDash = new AdminDasboardVM();
            adminDash.CrestIdVM = adminFunctionalityEnum.CrestIdVM;
            adminDash.AssociateNameVM = adminFunctionalityEnum.AssociateNameVM;
            adminDash.ServiceTypeVM = adminFunctionalityEnum.ServiceTypeVM;
            adminDash.JobProcessingStatusVM =adminFunctionalityEnum.JobProcessingStatusVM ;
            adminDash.RoleVM = adminFunctionalityEnum.RoleVM;
            adminDash.JobType = adminFunctionalityEnum.JobType;
            
            return adminDash;
        }

        public void GetMailDetails(Dictionary<String, String> dicReplace, string adminUserId, string associateUserName)
        {
            adminDashBoardBL.GetMailDetails(dicReplace, adminUserId, associateUserName);
        }
    }

}
