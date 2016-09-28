using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.ReviewerIndex.ComplexTypes
{
    public class ReviewerProfile_Result
    {
        public spGetReviewerDetails_Result ReviewerDetails_Result;

        public List<ReviewerEmails_Result> ReviewerEmails { get; set; }
        public List<AreaOfExpertise_Result> AreaOfExpertise{ get; set; }
        public List<ReferenceLink_Result> ReferenceLink { get; set; }
        public List<Journal_Result> Journal{ get; set; }
        public List<TitleReviewerlinkMaster_Result> TitleMaster { get; set; }
        public List<TitleAndAffillationMaster_Result> AffillationMastert { get; set; }
        public List<AreaOfExpReviewerlink_Result> AreaOfExpReviewerlink { get; set; }

        public ReviewerProfile_Result()
        {
            ReviewerEmails = new List<ReviewerEmails_Result>();
            AreaOfExpertise = new List<AreaOfExpertise_Result>();
            ReferenceLink = new List<ReferenceLink_Result>();
            Journal = new List<Journal_Result>();
            TitleMaster = new List<TitleReviewerlinkMaster_Result>();
            AffillationMastert = new List<TitleAndAffillationMaster_Result>();
            AreaOfExpReviewerlink = new List<AreaOfExpReviewerlink_Result>();
        }
    }
}
