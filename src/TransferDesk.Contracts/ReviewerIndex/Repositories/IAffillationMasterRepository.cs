using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities = TransferDesk.Contracts.ReviewerIndex.Entities;

namespace TransferDesk.Contracts.ReviewerIndex.Repositories
{
    public interface IAffillationMasterRepository : IDisposable
    {
        int? AddAffillationMaster(Entities.AffillationMaster affillationMaster);
        void UpdateAffillationMaster(Entities.AffillationMaster affillationMaster);

        void SaveChanges();
    }
}
