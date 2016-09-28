using System.ComponentModel.DataAnnotations;

namespace TransferDesk.Contracts.ReviewerIndex.Entities
{
    public class TitleMaster
    {
        [Key]
        public int TitleID { get; set; }
        public string MScriptID { get; set; }
        public string Name { get; set; }
        public string TitleName { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime? ModifiedDate { get; set; }
    }
}
