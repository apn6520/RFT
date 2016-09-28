using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.ReviewerIndex.Entities;

namespace TransferDesk.Contracts.ReviewerIndex.DataContext
{
    public  interface IReviewerIndexDataContext : IDisposable
    {
         DbSet<AffillationMaster> AffillationMaster { get; set; }

         DbSet<AffillationReviewerlink> AffillationReviewerlink { get; set; }

         DbSet<AreaOfExpertiseMaster> AreaOfExpertiseMaster { get; set; }

         DbSet<AreaOfExpReviewerlink> AreaOfExpReviewerlink { get; set; }

         DbSet<DepartmentMaster> DepartmentMaster { get; set; }

         DbSet<InstituteMaster> InstituteMaster { get; set; }

         DbSet<JournalReviewerLink> JournalReviewerLink { get; set; }

         DbSet<Location> Location { get; set; }

         DbSet<ReferenceReviewerlink> ReferenceReviewerlink { get; set; }

         DbSet<ReviewerMailLink> ReviewerMailLink { get; set; }

         DbSet<ReviewerMaster> ReviewerMaster { get; set; }

         DbSet<TitleMaster> TitleMaster { get; set; }

         DbSet<TitleReviewerlink> TitleReviewerlink { get; set; }


        string spGetReviewersList(string keySearchOne, string keySearchTwo, string searchCondition, string dDLOneValue,
            string dDLTwoValue, string reviewersList);     
        
    }
}
