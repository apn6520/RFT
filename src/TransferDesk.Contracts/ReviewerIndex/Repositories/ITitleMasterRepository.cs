using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.ReviewerIndex.Repositories
{
    public interface ITitleMasterRepository: IDisposable
    {
        int? AddTitleMaster(Entities.TitleMaster titleMaster);
        void UpdateTitleMaster(Entities.TitleMaster titleMaster);       
        void SaveChanges();
    }
}
