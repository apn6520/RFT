using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RepositoryInterfaces = TransferDesk.Contracts.ReviewerIndex.Repositories;
using Entities = TransferDesk.Contracts.ReviewerIndex.Entities;
using DataContexts = TransferDesk.DAL.ReviewerIndex.DataContext;
using System.Data.Entity;

namespace TransferDesk.DAL.ReviewerIndex.Repositories
{
 
    public class ReviewerIndexRepository :RepositoryInterfaces.IAffillationMasterRepository,
                                          RepositoryInterfaces.IAffillationReviewerlinkRepository ,
                                          RepositoryInterfaces.IAreaOfExpertiseMasterRepository ,
                                          RepositoryInterfaces.IAreaOfExpReviewerlinkRepository, 
                                          RepositoryInterfaces.IDepartmentMasterRepository ,
                                          RepositoryInterfaces.IInstituteMasterRepository, 
                                          RepositoryInterfaces.IJournalReviewerLinkRepository,
                                          RepositoryInterfaces.ILocationRepository,
                                          RepositoryInterfaces.IReferenceReviewerlinkRepository,
                                          RepositoryInterfaces.IReviewerMailLinkRepository ,
                                          RepositoryInterfaces.IReviewerMasterRepository,
                                          RepositoryInterfaces.ITitleMasterRepository,                       
                                          RepositoryInterfaces.ITitleReviewerlinkRepository, IDisposable
    {
        public DataContexts.ReviewerIndexDBContext reviewerIndexDBContext;
        //public string userID = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");

        public ReviewerIndexRepository(string conString)
        {
            this.reviewerIndexDBContext = new DataContexts.ReviewerIndexDBContext(conString);
            
        }
        //AffillationMaster
        public int? AddAffillationMaster(Entities.AffillationMaster affillation)
        {
            affillation.IsActive = true;
            affillation.CreatedBy = "";
            affillation.CreatedDate= System.DateTime.Now;
            reviewerIndexDBContext.AffillationMaster.Add(affillation);
            reviewerIndexDBContext.SaveChanges();
            return affillation.ID;
        }
        public void UpdateAffillationMaster(Entities.AffillationMaster affillation)
        {
            affillation.ModifiedDate = System.DateTime.Now;
            affillation.ModifiedBy = "";
            reviewerIndexDBContext.Entry(affillation).State = EntityState.Modified;
        }

        //AffillationReviewerlink
        public int? AddAffillationReviewerlink(Entities.AffillationReviewerlink affillationReviewerlink)
        {
            affillationReviewerlink.IsActive = true;
            affillationReviewerlink.CreatedBy = "";
            affillationReviewerlink.CreatedDate = System.DateTime.Now;
            reviewerIndexDBContext.AffillationReviewerlink.Add(affillationReviewerlink);
            reviewerIndexDBContext.SaveChanges();
            return affillationReviewerlink.ID;
        }
        public void UpdateAffillationReviewerlink(Entities.AffillationReviewerlink affillationReviewerlink)
        {
            affillationReviewerlink.ModifiedDate = System.DateTime.Now;
            affillationReviewerlink.ModifiedBy = "";
            reviewerIndexDBContext.Entry(affillationReviewerlink).State = EntityState.Modified; 
        }
        public void DeleteAffillationReviewerlink(Entities.AffillationReviewerlink affillationReviewerlink)
        {
            reviewerIndexDBContext.AffillationReviewerlink.Remove(affillationReviewerlink);
            reviewerIndexDBContext.Entry(affillationReviewerlink).State = EntityState.Deleted;

        }

        //AreaOfExpertiseMaster
        public int? AddAreaOfExpertiseMaster(Entities.AreaOfExpertiseMaster areaOfExpertiseMaster)
        {
            areaOfExpertiseMaster.IsActive = true;
            areaOfExpertiseMaster.CreatedBy = areaOfExpertiseMaster.CreatedBy;
            areaOfExpertiseMaster.CreatedDate = System.DateTime.Now;
            reviewerIndexDBContext.AreaOfExpertiseMaster.Add(areaOfExpertiseMaster);
            reviewerIndexDBContext.SaveChanges();
            return areaOfExpertiseMaster.ID;
        }
        public void UpdateAreaOfExpertiseMaster(Entities.AreaOfExpertiseMaster areaOfExpertiseMaster)
        {
            areaOfExpertiseMaster.ModifiedDate = System.DateTime.Now;
            areaOfExpertiseMaster.ModifiedBy = "";
            reviewerIndexDBContext.Entry(areaOfExpertiseMaster).State = EntityState.Modified;
        }
        public void DeleteAreaOfExpertiseMaster(Entities.AreaOfExpertiseMaster areaOfExpertiseMaster)
        {
            reviewerIndexDBContext.AreaOfExpertiseMaster.Remove(areaOfExpertiseMaster);
            reviewerIndexDBContext.Entry(areaOfExpertiseMaster).State = EntityState.Deleted;

        }

        //AreaOfExpReviewerlink
        public int? AddAreaOfExpReviewerlink(Entities.AreaOfExpReviewerlink areaOfExpReviewerlink)
        {
            areaOfExpReviewerlink.IsActive = true;
            areaOfExpReviewerlink.CreatedBy = areaOfExpReviewerlink.CreatedBy;
            areaOfExpReviewerlink.CreatedDate = System.DateTime.Now;
            reviewerIndexDBContext.AreaOfExpReviewerlink.Add(areaOfExpReviewerlink);
            reviewerIndexDBContext.SaveChanges();
            return areaOfExpReviewerlink.ID;
        }
        public void UpdateAreaOfExpReviewerlink(Entities.AreaOfExpReviewerlink areaOfExpReviewerlink)
        {
            areaOfExpReviewerlink.ModifiedDate = System.DateTime.Now;
            areaOfExpReviewerlink.IsActive = false;
            areaOfExpReviewerlink.ModifiedBy = areaOfExpReviewerlink.CreatedBy;
           
        }
        public void DeleteAreaOfExpReviewerlink(Entities.AreaOfExpReviewerlink areaOfExpertiseMaster)
        {
            reviewerIndexDBContext.AreaOfExpReviewerlink.Remove(areaOfExpertiseMaster);
            reviewerIndexDBContext.Entry(areaOfExpertiseMaster).State = EntityState.Deleted;

        }

        //DepartmentMaster
        public int? AddDepartmentMaster(Entities.DepartmentMaster departmentMaster)
        {
            departmentMaster.IsActive = true;
            departmentMaster.CreatedBy = departmentMaster.CreatedBy;
            departmentMaster.CreatedDate = System.DateTime.Now;
            reviewerIndexDBContext.DepartmentMaster.Add(departmentMaster);
            reviewerIndexDBContext.SaveChanges();
            return departmentMaster.ID;
        }
        public void UpdateDepartmentMaster(Entities.DepartmentMaster departmentMaster)
        {
            departmentMaster.ModifiedDate = System.DateTime.Now;
            departmentMaster.ModifiedBy = "";
            reviewerIndexDBContext.Entry(departmentMaster).State = EntityState.Modified;
        }

        //InstituteMaster
        public int? AddInstituteMaster(Entities.InstituteMaster instituteMaster)
        {
            instituteMaster.IsActive = true;
            instituteMaster.CreatedBy = instituteMaster.CreatedBy;
            instituteMaster.CreatedDate = System.DateTime.Now;
            reviewerIndexDBContext.InstituteMaster.Add(instituteMaster);
            reviewerIndexDBContext.SaveChanges();
            return instituteMaster.ID;
        }
        public void UpdateInstituteMaster(Entities.InstituteMaster instituteMaster)
        {
            instituteMaster.ModifiedDate = System.DateTime.Now;
            instituteMaster.ModifiedBy = "";
            reviewerIndexDBContext.Entry(instituteMaster).State = EntityState.Modified;
        }

        //JournalReviewerLink
        public int? AddJournalReviewerLink(Entities.JournalReviewerLink journalReviewerLink)
        {
            journalReviewerLink.IsActive = true;
            journalReviewerLink.CreatedBy = journalReviewerLink.CreatedBy;            
            journalReviewerLink.CreatedDate = System.DateTime.Now;
            reviewerIndexDBContext.JournalReviewerLink.Add(journalReviewerLink);
            reviewerIndexDBContext.SaveChanges();
            return journalReviewerLink.JournalID;
        }
        public void DeleteJournalReviewerLink(Entities.JournalReviewerLink journalReviewerLink)
        {
            reviewerIndexDBContext.JournalReviewerLink.Remove(journalReviewerLink);
            reviewerIndexDBContext.Entry(journalReviewerLink).State = EntityState.Deleted;

        }
        
        //Location
        public int? AddLocation(Entities.Location location)
        {
            reviewerIndexDBContext.Location.Add(location);
            reviewerIndexDBContext.SaveChanges();
            return location.ID;
        }
        public void UpdateLocation(Entities.Location location)
        { 
            reviewerIndexDBContext.Entry(location).State = EntityState.Modified;
        }

        //ReferenceReviewerlink
        public int? AddReferenceReviewerlink(Entities.ReferenceReviewerlink referenceReviewerlink)
        {
            referenceReviewerlink.IsActive = true;
            referenceReviewerlink.CreatedBy = referenceReviewerlink.CreatedBy;
            referenceReviewerlink.CreatedDate = System.DateTime.Now;
            reviewerIndexDBContext.ReferenceReviewerlink.Add(referenceReviewerlink);
            reviewerIndexDBContext.SaveChanges();
            return referenceReviewerlink.ID;
        }

        public void DeleteReferenceReviewerlink(Entities.ReferenceReviewerlink referenceReviewerlink)
        {
            reviewerIndexDBContext.ReferenceReviewerlink.Remove(referenceReviewerlink);
            reviewerIndexDBContext.Entry(referenceReviewerlink).State = EntityState.Deleted;

        }


        //ReviewerMailLink
        public int? AddReviewerMailLink(Entities.ReviewerMailLink reviewerMailLink)
        {
            try
            {
                reviewerMailLink.IsActive = true;
                reviewerMailLink.CreatedBy = reviewerMailLink.CreatedBy;
                reviewerMailLink.CreatedDate = System.DateTime.Now;
                reviewerIndexDBContext.ReviewerMailLink.Add(reviewerMailLink);
                reviewerIndexDBContext.SaveChanges();
                return reviewerMailLink.ID;
            }
            catch (Exception ex)
            {
                
                throw;
            }
         
        }
        public void UpdateReviewerMailLink(Entities.ReviewerMailLink reviewerMailLink)
        {
            reviewerMailLink.ModifiedDate = System.DateTime.Now;
            reviewerMailLink.ModifiedBy = "";
            reviewerMailLink.IsActive = false;
            
        }
        public void UpdateReferenceReviewerlink(Entities.ReferenceReviewerlink referenceReviewerlink)
        {
           
        }
        public void DeleteReviewerMailLink(Entities.ReviewerMailLink reviewerMailLink)
        {
            reviewerIndexDBContext.ReviewerMailLink.Remove(reviewerMailLink);
            reviewerIndexDBContext.Entry(reviewerMailLink).State = EntityState.Deleted;

        }

        //ReviewerMaster
        public int? AddReviewerMaster(Entities.ReviewerMaster reviewerMaster)
        {
            reviewerMaster.IsActive = true;
            reviewerMaster.CreatedBy = "";
            reviewerMaster.CreatedDate = System.DateTime.Now;
            reviewerIndexDBContext.ReviewerMaster.Add(reviewerMaster);
            reviewerIndexDBContext.SaveChanges();
            return reviewerMaster.ID;
        }
        public void UpdateReviewerMaster(Entities.ReviewerMaster reviewerMaster)
        {
            reviewerMaster.ModifiedDate = System.DateTime.Now;
            reviewerMaster.ModifiedBy = "";
            reviewerIndexDBContext.Entry(reviewerMaster).State = EntityState.Modified;
        }

        //TitleMaster
        public int? AddTitleMaster(Entities.TitleMaster titleMaster)
        {
            titleMaster.IsActive = true;
            titleMaster.CreatedBy = "";
            titleMaster.CreatedDate = System.DateTime.Now;
            reviewerIndexDBContext.TitleMaster.Add(titleMaster);
            reviewerIndexDBContext.SaveChanges();
            return titleMaster.TitleID;
        }
        public void UpdateTitleMaster(Entities.TitleMaster titleMaster)
        {
            titleMaster.ModifiedDate = System.DateTime.Now;
            titleMaster.ModifiedBy = "";
            reviewerIndexDBContext.Entry(titleMaster).State = EntityState.Modified;
        }

        //TitleReviewerlink
        public int? AddTitleReviewerlink(Entities.TitleReviewerlink titleReviewerlink)
        {
            titleReviewerlink.IsActive = true;
            titleReviewerlink.CreatedBy = titleReviewerlink.CreatedBy;
            titleReviewerlink.CreatedDate = System.DateTime.Now;
            reviewerIndexDBContext.TitleReviewerlink.Add(titleReviewerlink);
            reviewerIndexDBContext.SaveChanges();
            return titleReviewerlink.ID;
        }
        public void UpdateTitleReviewerlink(Entities.TitleReviewerlink titleReviewerlink)
        {
            titleReviewerlink.ModifiedDate = System.DateTime.Now;
            titleReviewerlink.ModifiedBy = "";
            reviewerIndexDBContext.Entry(titleReviewerlink).State = EntityState.Modified;
        }
        public void DeleteTitleReviewerlink(Entities.TitleReviewerlink titleReviewerlink)
        {
            reviewerIndexDBContext.TitleReviewerlink.Remove(titleReviewerlink);
            reviewerIndexDBContext.Entry(titleReviewerlink).State = EntityState.Deleted;

        }



        public void SaveChanges()
        {
            reviewerIndexDBContext.SaveChanges();
        }
         

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    reviewerIndexDBContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
