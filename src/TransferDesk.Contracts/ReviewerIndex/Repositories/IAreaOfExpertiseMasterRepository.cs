using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.ReviewerIndex.Repositories
{
    public interface IAreaOfExpertiseMasterRepository:IDisposable
    {
        int? AddAreaOfExpertiseMaster(Entities.AreaOfExpertiseMaster areaOfExpertiseMaster);
        void UpdateAreaOfExpertiseMaster(Entities.AreaOfExpertiseMaster areaOfExpertiseMaster);
        void DeleteAreaOfExpertiseMaster(Entities.AreaOfExpertiseMaster areaOfExpertiseMaster);

        void SaveChanges();
    }
}
