using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.ReviewerIndex.Repositories
{
    public interface ILocationRepository:IDisposable
    {
        int? AddLocation(Entities.Location location);
        void UpdateLocation(Entities.Location location);
        void SaveChanges();
    }
}
