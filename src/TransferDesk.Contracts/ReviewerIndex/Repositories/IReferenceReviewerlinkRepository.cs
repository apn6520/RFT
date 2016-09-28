using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.ReviewerIndex.Repositories
{
    public interface IReferenceReviewerlinkRepository:IDisposable
    {
        int? AddReferenceReviewerlink(Entities.ReferenceReviewerlink referenceReviewerlink);
        void UpdateReferenceReviewerlink(Entities.ReferenceReviewerlink referenceReviewerlink);
        void DeleteReferenceReviewerlink(Entities.ReferenceReviewerlink referenceReviewerlink);

        void SaveChanges();

    }
}
