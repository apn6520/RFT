using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.ReviewerIndex.ComplexTypes
{
    public class spmsdisplaymanuscriptdetails_Result
    {
        [Key]
        public int ID { get; set; }  
        public string msid { get; set; }
        public int ReviewerMasterID { get; set; }
        public string ReviewerName { get; set; }
        public string CreatedDate { get; set; } 
        public string email { get; set; }
        public string Referencelink { get; set; }
        public string IsAssociateFinalSubmit { get; set; } 
        public string JobType { get; set; }  
        public int JournalID { get; set; }  
        public string ArticleTitle { get; set; }
        public int SMTaskID { get; set; } 
        public string ToolTip { get; set; } 
        public int flag { get; set; }  
        public string Affiliation { get; set; }
        public DateTime? AnalystSubmissionDate { get; set; }
    }
}
