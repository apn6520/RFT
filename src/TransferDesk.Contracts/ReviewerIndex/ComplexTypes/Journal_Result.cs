using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.ReviewerIndex.ComplexTypes
{
   public class Journal_Result
   {
        [Key]
       public int JournalID { get; set; }
       public string JournalTitle { get; set; }
       public int ReviewerMasterID { get; set; }
       public bool IsActive { get; set; }
       public DateTime? ModifiedDate { get; set; }
    }
}
