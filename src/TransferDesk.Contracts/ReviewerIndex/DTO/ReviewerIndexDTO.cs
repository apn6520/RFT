using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities = TransferDesk.Contracts.ReviewerIndex.Entities;
namespace TransferDesk.Contracts.ReviewerIndex.DTO
{
    public class ReviewerIndexDTO
    {
        public Entities.ReviewerMaster ReviewerMaster;

        public List<Entities.AffillationMaster> AffillationMasters { get; set; }
        public List<Entities.AffillationReviewerlink> AffillationReviewerlinks { get; set; }
        public List<Entities.AreaOfExpertiseMaster> AreaOfExpertiseMasters { get; set; }
        public List<Entities.AreaOfExpReviewerlink> AreaOfExpReviewerlinks { get; set; }
        public List<Entities.DepartmentMaster> DepartmentMasters { get; set; }
        public List<Entities.InstituteMaster> InstituteMasters { get; set; }
        public List<Entities.JournalReviewerLink> JournalReviewerLinks { get; set; }
        public List<Entities.Location> Location { get; set; }
        public List<Entities.ReferenceReviewerlink> ReferenceReviewerlinks { get; set; }
        public List<Entities.ReviewerMailLink> ReviewerMailLinks { get; set; }
        public List<Entities.TitleMaster> TitleMasters { get; set; }
        public List<Entities.TitleReviewerlink> TitleReviewerlinks { get; set; }

        public ReviewerIndexDTO()
        {
            AffillationMasters= new List<Entities.AffillationMaster>();
            AffillationReviewerlinks =new List<Entities.AffillationReviewerlink>();
            AreaOfExpertiseMasters= new List<Entities.AreaOfExpertiseMaster>();
            AreaOfExpReviewerlinks= new List<Entities.AreaOfExpReviewerlink>();
            DepartmentMasters = new List<Entities.DepartmentMaster>();
            InstituteMasters=new List<Entities.InstituteMaster>();
            JournalReviewerLinks=new List<Entities.JournalReviewerLink>();
            Location=new List<Entities.Location>();
            ReferenceReviewerlinks=new List<Entities.ReferenceReviewerlink>();
            TitleMasters=new List<Entities.TitleMaster>();
            TitleReviewerlinks=new List<Entities.TitleReviewerlink>();
        }

    }
}
