using System.ComponentModel.DataAnnotations;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class ManuscriptBookErrorCategory
    {
        [Key]
        public int ID { get; set; }
        public int ManuscriptBookScreeningID { get; set; }
        public int ErrorCategoryID { get; set; }
        public int? Status { get; set; }
        public System.DateTime? ModifiedDateTime { get; set; }
        public bool? IsUncheckedByUser { get; set; }
    }
}
