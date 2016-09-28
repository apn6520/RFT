using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class SlidingScale
    {
        [Key]
        public int ID { get; set; }
        public string Quality { get; set; }
        public string QualityCheckedPercentage { get; set; }
        public int ServiceTypeID { get; set; }
        public int? Status { get; set; }
        public string CreatedBy { get; set; }
        public string ModifidedBy { get; set; }
        public System.DateTime? ModifidedDate { get; set; }
    }
}
