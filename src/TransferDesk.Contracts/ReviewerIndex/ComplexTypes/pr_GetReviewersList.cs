using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.ReviewerIndex.ComplexTypes
{
    public class pr_GetReviewersList
    {
        public long RowNo { get; set; }
        public string ReviewerName { get; set; }
        public string Affiliation { get; set; }
        public int ReviewerID { get; set; }
        public int Numberofrelevantpublications { get; set; }
        public int TitleID { get; set; }
        public string ManuscriptID { get; set; }
        public string Title { get; set; }
        public int AreaofexpertiseID { get; set; }
        public int AreaofexpertiseID1 { get; set; }
        public long RowNum { get; set; }
        public string emailaddress { get; set; }
        public string Referencelink { get; set; }
        public string AreaOfExpertise { get; set; }
         
        
    }
}
