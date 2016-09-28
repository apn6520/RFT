using System.ComponentModel.DataAnnotations;
using TransferDesk.Contracts.Manuscript.Entities;

namespace TransferDesk.Contracts.ReviewerIndex.Entities
{
    public class TitleReviewerlink
    {
        [Key]
        public int ID { get; set; }
        public int TitleMasterID { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime? ModifiedDate { get; set; }
        public int ReviewerMasterID { get; set; }
    }
}
