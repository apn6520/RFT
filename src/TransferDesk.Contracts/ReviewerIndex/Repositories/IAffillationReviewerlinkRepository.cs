using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.ReviewerIndex.Repositories
{
    public interface IAffillationReviewerlinkRepository:IDisposable
    {
        int? AddAffillationReviewerlink(Entities.AffillationReviewerlink affillationReviewerlink);
        void UpdateAffillationReviewerlink(Entities.AffillationReviewerlink affillationReviewerlink);
        void DeleteAffillationReviewerlink(Entities.AffillationReviewerlink affillationReviewerlink);

        void SaveChanges();
    }
}
