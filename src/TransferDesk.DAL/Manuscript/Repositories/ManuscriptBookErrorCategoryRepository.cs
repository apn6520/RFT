using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.Repositories;
using Entities = TransferDesk.Contracts.Manuscript.Entities;

using TransferDesk.DAL.Manuscript.DataContext;
using System.Data.Entity;

namespace TransferDesk.DAL.Manuscript.Repositories
{
    public class ManuscriptBookErrorCategoryRepository
    {
         private ManuscriptDBContext context;

        //dispose calls
        private bool disposed = false;

        public ManuscriptBookErrorCategoryRepository(ManuscriptDBContext manuscriptDbContext)
        {
            this.context = manuscriptDbContext;
        }
        public ManuscriptBookErrorCategoryRepository(string conString)
        {
            this.context = new ManuscriptDBContext(conString);
        }

        public void AddManuscriptErrorCategory(Entities.ManuscriptBookErrorCategory manuscriptBookErrorCategory)
        {
            manuscriptBookErrorCategory.ModifiedDateTime = System.DateTime.Now;
            manuscriptBookErrorCategory.Status = 1;
            context.ManuscriptBookErrorCategory.Add(manuscriptBookErrorCategory);
           context.SaveChanges();
        }

        public void UpdateManuscriptErrorCategory(Entities.ManuscriptBookErrorCategory manuscriptErrorCategory)
        {
            manuscriptErrorCategory.ModifiedDateTime = System.DateTime.Now;
            manuscriptErrorCategory.Status = 2;
           context.Entry(manuscriptErrorCategory).State = EntityState.Modified;
           context.SaveChanges();
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
