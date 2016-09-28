using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.ReviewerIndex.Repositories
{
    public interface IJournalReviewerLinkRepository:IDisposable
    {
        int? AddJournalReviewerLink(Entities.JournalReviewerLink journalReviewerLink);
        //void UpdateJournalReviewerLink(Entities.JournalReviewerLink journalReviewerLink);
        void DeleteJournalReviewerLink(Entities.JournalReviewerLink journalReviewerLink);

        void SaveChanges();
    }
}
