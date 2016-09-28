using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.ReviewerIndex.ComplexTypes
{
    public class AreaOfExpReviewerlink_Result
    {
         [Key]
        public string PrimaryExp { get; set; }
        public int? FExpertiseLevel { get; set; }
        public int PID { get; set; }
        public string SecondaryExp { get; set; }
        public int? SExpertiseLevel { get; set; }
        public int SID { get; set; }
        public string TertiaryExp { get; set; }
        public int? TExpertiseLevel { get; set; }
        public int TID { get; set; }
        public bool IsActive { get; set; }
        public int ReviewerMasterID { get; set; }
    }
}
