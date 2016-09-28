using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.ReviewerIndex.Repositories
{
    public interface IReviewerMailLinkRepository:IDisposable
    {
        int? AddReviewerMailLink(Entities.ReviewerMailLink reviewerMailLink);
        void UpdateReviewerMailLink(Entities.ReviewerMailLink reviewerMailLink);
        void DeleteReviewerMailLink(Entities.ReviewerMailLink reviewerMailLink);

        void SaveChanges();
    }
}
