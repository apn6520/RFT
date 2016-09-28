using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Entities
{
  public  class JournalUserRoles
    {
        [Key]
        public int ID { get; set; }
        public int JournalMasterId { get; set; }
        public int UserRolesId { get; set; }
        public bool Status { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime? ModifiedDate { get; set; }
      
    }
}
