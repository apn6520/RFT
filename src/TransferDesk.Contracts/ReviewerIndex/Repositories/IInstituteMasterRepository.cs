using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.ReviewerIndex.Repositories
{
    public interface IInstituteMasterRepository:IDisposable
    {
        int? AddInstituteMaster(Entities.InstituteMaster instituteMaster);
        void UpdateInstituteMaster(Entities.InstituteMaster instituteMaster);

        void SaveChanges();
    }


}
