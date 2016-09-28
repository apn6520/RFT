using System;
using System.Collections.Generic;
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
    public class BookUserReposistory : IDisposable
    {

        private ManuscriptDBContext context;

        public BookUserReposistory(ManuscriptDBContext context)
        {
            this.context = context;
        }

        public BookUserReposistory(string conString)
        {
            this.context = new ManuscriptDBContext(conString);
        }
        //dispose calls
        private bool disposed = false;

        public void AddBookUserDetails(Entities.BookUserRoles bookuser)
        {
            bookuser.CreatedDate = DateTime.Now;
            context.BookUserRoles.Add(bookuser);
        }

        public void UpdateBookUserDetails(Entities.BookUserRoles bookuser)
        {
            bookuser.ModifiedDate = DateTime.Now;
            BookUserRoles existing = context.BookUserRoles.Find(bookuser.ID);
            ((IObjectContextAdapter)context).ObjectContext.Detach(existing);
            context.Entry(bookuser).State = EntityState.Modified;
        }
        public void RemoveBookUserDetails(Entities.BookUserRoles Bookuser)
        {
            context.Entry(Bookuser).State = EntityState.Deleted;

        }
        public bool CheckBookUser(int userid, int bookid)
        {
            var result = from q in context.BookUserRoles
                         where q.UserRolesId == userid && q.BookMasterId == bookid
                         select q;
            if (result.Count() > 0)
                return true;
            else
            {
                return false;
            }

        }

        public int GetId(int uid, int jid)
        {
            var id = from q in context.BookUserRoles
                     where q.UserRolesId == uid && q.BookMasterId == jid
                     select q;
            foreach (var bookid in id)
            {
                return bookid.ID;
            }
            return 0;
        }


        public BookUserRoles GetBookDetails(int id)
        {
            var details = (from q in context.BookUserRoles
                           where q.ID == id
                           select q).FirstOrDefault();
            return details;
        }

        public List<BookUserRoles> GetBookDetailsForUserID(int userid)
        {
            var bookdetails = (from q in context.BookUserRoles
                               where q.UserRolesId == userid && q.Status==true
                               select q).ToList();
            return bookdetails;
        }

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
