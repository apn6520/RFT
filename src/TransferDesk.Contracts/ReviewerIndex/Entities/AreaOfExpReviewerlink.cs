 
using System.ComponentModel.DataAnnotations;

namespace TransferDesk.Contracts.ReviewerIndex.Entities
{
    public class AreaOfExpReviewerlink
    {
        [Key]
        public int ID { get; set; }
        public int AreaOfExpertiseMasterID { get; set; }
        public int ReviewerMasterID { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public  System.DateTime? ModifiedDate { get; set; }

    }
}
