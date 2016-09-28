using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.ComplexTypes.AdminDashBoard;
using TransferDesk.Contracts.Manuscript.Entities;

namespace TransferDesk.Services.Manuscript.ViewModel
{
    public class AdminDasboardVM
    {

        public int CrestIdVM { get; set; }
        public string ServiceTypeVM { get; set; }
        public string RoleVM { get; set; }
        public string JobProcessingStatusVM { get; set; }
        public string AssociateNameVM { get; set; }
        public string StatusVm { get; set; }
        public string JobType { get; set; }
        public IEnumerable<pr_AdminDashBoardGetAllOpenJobs_Result> jobsdetails { get; set; }

    }
}
