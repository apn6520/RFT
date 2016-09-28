using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.ReviewerIndex.Repositories
{
    public interface ITitleReviewerlinkRepository:IDisposable
    {
        int? AddTitleReviewerlink(Entities.TitleReviewerlink titleReviewerlink);
        void UpdateTitleReviewerlink(Entities.TitleReviewerlink titleReviewerlink);
        void DeleteTitleReviewerlink(Entities.TitleReviewerlink titleReviewerlink);

        void SaveChanges();
    }
}
