using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.ReviewerIndex.Repositories
{
    public interface IDepartmentMasterRepository: IDisposable
    {
        int? AddDepartmentMaster(Entities.DepartmentMaster departmentMaster);
        void UpdateDepartmentMaster(Entities.DepartmentMaster departmentMaster); 

        void SaveChanges();
    }
}
