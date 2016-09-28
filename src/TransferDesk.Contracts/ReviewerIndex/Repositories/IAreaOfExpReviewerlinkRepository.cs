using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.ReviewerIndex.Repositories
{
    public interface IAreaOfExpReviewerlinkRepository:IDisposable
    {
        int? AddAreaOfExpReviewerlink(Entities.AreaOfExpReviewerlink areaOfExpReviewerlink);
        void UpdateAreaOfExpReviewerlink(Entities.AreaOfExpReviewerlink areaOfExpReviewerlink);
        void DeleteAreaOfExpReviewerlink(Entities.AreaOfExpReviewerlink areaOfExpReviewerlink);

        void SaveChanges();
    }   
}
