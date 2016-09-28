using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.ComplexTypes.AssociateDashBoard
{
   public class pr_GetSpecificAssociateDetails_Result
    {
        public long? SrNo { get; set; }
        public string CrestId { get; set; }
        public string JobType { get; set; }
        public string ServiceType { get; set; }
        public string MSID { get; set; }
        public string JournalBookName { get; set; }
        public int PageCount { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Task { get; set; }
        public string GroupNo { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public System.DateTime? FetchedDate { get; set; }
        public int? Age { get; set; }
        public string HandlingTime { get; set; }
    }
   public class pr_IsJobFetchedOrAssign_Result
   {
       public int FetchedJobCount { get; set; }

   }

   public class pr_IsJobFetched_Result
   {
       public int FetchedJobCount { get; set; }

   }
   public class pr_JobTobeFetched_Result
   {
       public string CrestID { get; set; }
       public int ManuscriptLoginDetailsId { get; set; }
       public string JobType { get; set; }
   }
   
}
