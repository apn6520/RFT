using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.DAL.Manuscript.DataContext;
using TransferDesk.Contracts.Manuscript.Entities;
using TransferDesk.Contracts.Manuscript.Repositories;
using TransferDesk.DAL.Manuscript.DataContext;
using Entities = TransferDesk.Contracts.Manuscript.Entities;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
namespace TransferDesk.DAL.Manuscript.Repositories
{
    public class JournalUserReposistory : IDisposable
    {
        private ManuscriptDBContext context;

        public JournalUserReposistory(ManuscriptDBContext context)
        {
            this.context = context;
        }

        public JournalUserReposistory(string conString)
        {
            this.context = new ManuscriptDBContext(conString);
        }

        public void AddJournalUserDetails(Entities.JournalUserRoles Journaluser)
        {
            Journaluser.CreatedDate = DateTime.Now;
            context.JournalUserRoles.Add(Journaluser);

        }
        public void RemoveJournalUserDetails(Entities.JournalUserRoles Journaluser)
        {
            context.Entry(Journaluser).State = EntityState.Deleted;

        }
        public bool CheckIfUserRoleForGivenJournal(int journalid, int userid)
        {

            var result = (from q in context.JournalUserRoles
                          where q.JournalMasterId == journalid && q.UserRolesId == userid
                          select q).Count();
            if (result > 0)
                return true;
            else
            {
                return false;
            }

        }

        public JournalUserRoles GetDetails(int id)
        {
            var details = (from q in context.JournalUserRoles
                           where q.ID == id
                           select q).FirstOrDefault();
            return details;
        }

        public int GetId(int uid, int jid)
        {
            var id = from q in context.JournalUserRoles
                     where q.UserRolesId == uid && q.JournalMasterId == jid
                     select q;
            foreach (var journid in id)
            {
                return journid.ID;
            }
            return 0;
        }

        public void UpdateJournalUserDetails(Entities.JournalUserRoles Journaluser)
        {
            Journaluser.ModifiedDate = DateTime.Now;
            JournalUserRoles existing = context.JournalUserRoles.Find(Journaluser.ID);
            ((IObjectContextAdapter)context).ObjectContext.Detach(existing);
            context.Entry(Journaluser).State = EntityState.Modified;
        }

        public bool CheckJournalUser(int userid, int journalid)
        {
            var result = from q in context.JournalUserRoles
                         where q.UserRolesId == userid && q.JournalMasterId == journalid
                         select q;
            if (result.Count() > 0)
                return true;
            else
            {
                return false;
            }

        }


        public List<Entities.JournalUserRoles> GetJournalDetailsForUserID(int userid)
        {
            var journaldetails = (from q in context.JournalUserRoles
                                  where q.UserRolesId == userid &&  q.Status==true
                                  select q).ToList();
            return journaldetails;
        }
        private bool disposed = false;

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
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
