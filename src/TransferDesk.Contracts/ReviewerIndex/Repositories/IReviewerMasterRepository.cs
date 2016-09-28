using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.ReviewerIndex.Repositories
{
    public interface IReviewerMasterRepository:IDisposable
    {
        int? AddReviewerMaster(Entities.ReviewerMaster reviewerMaster);
        void UpdateReviewerMaster(Entities.ReviewerMaster reviewerMaster);
        void SaveChanges();
    }
}
