using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.ComplexTypes.AssociateDashBoard;

namespace TransferDesk.Services.Manuscript.ViewModel
{
    public class AssociateDashboardVM
    {
        public IEnumerable<pr_GetSpecificAssociateDetails_Result> specificAssociatedetails { get; set; }
        public int? manuscriptsIDVM { get; set; }
        public int TotalAssociateJournalMSPendingJobs { get; set; }
        public int TotalAssociateJournalRSPendingJobs { get; set; }
        public int TotalAssociateBookMSPendingJobs { get; set; }
        public int TotalAssociateBookRSPendingJobs { get; set; }
    }
}
