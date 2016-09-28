using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.ComplexTypes.Search
{
   public class pr_SearchMSBookDetails_Result
    {
        public int ID { get; set; }
        public string BookTitle { get; set; }
        public string ChapterNumber { get; set; }
        public string ChapterTitle { get; set; }
    }
}
