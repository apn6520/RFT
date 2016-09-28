using System.ComponentModel.DataAnnotations;

namespace TransferDesk.Contracts.ReviewerIndex.Entities
{
    public class StatusMaster
    {
        [Key]
        public int ID { get; set; }
        public string StatusCode { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public System.DateTime? ModifiedDateTime { get; set; }
    }
}
