using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TransferDesk.Contracts.ReviewerIndex.Entities
{
    public class AreaOfExpertiseMaster
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int? ExpertiseLevel { get; set; }
        public  int? ParentID { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public  System.DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public  System.DateTime? ModifiedDate { get; set; }
    }
}
