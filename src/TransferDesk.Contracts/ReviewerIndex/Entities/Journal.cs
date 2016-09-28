using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.ReviewerIndex.Entities
{
   public class Journal
    {
        [Key]
        public int ID { get; set; }
        public string JournalTitle { get; set; }
        public int Status { get; set; }
        public System.DateTime? ModifiedDateTime { get; set; }
        public bool IsActive { get; set; }
        public string Link { get; set; }
    }
}
