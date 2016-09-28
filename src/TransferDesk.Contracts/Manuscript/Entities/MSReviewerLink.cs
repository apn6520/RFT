using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class MSReviewerLink
    {
        [Key]
        public int ID { get; set; }
        public int MSReviewersSuggestionInfoID { get; set; }
        public String Link { get; set; }

    }
}
