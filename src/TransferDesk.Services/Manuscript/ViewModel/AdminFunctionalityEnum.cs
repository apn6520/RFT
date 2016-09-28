using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Services.Manuscript.ViewModel
{
    public class AdminFunctionalityEnum
    {
        public int CrestIdVM { get; set; }
        public string AssociateNameVM { get; set; }
        public string ServiceTypeVM { get; set; }
        public string JobProcessingStatusVM { get; set; }
        public string RoleVM { get; set; }
        public string JobType { get; set; }

        public DateTime? ReceivedDate { get; set; }
        public string Msid { get; set; }
        public string JournalTitle { get; set; }
    }
}
