using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.ComplexTypes.AdminDashBoard;
using RepositoryInterfaces = TransferDesk.Contracts.Manuscript.Repositories;
using Entities = TransferDesk.Contracts.Manuscript.Entities;
using DataContexts = TransferDesk.DAL.Manuscript.DataContext;
using TransferDesk.Contracts.Manuscript.DTO;
using TransferDesk.Contracts.Manuscript.Entities;
using System.Data.SqlClient;

namespace TransferDesk.DAL.Manuscript.Repositories
{
    public class AdminDashBoardReposistory : IDisposable
    {
        private bool _disposed = false;
        private DataContexts.ManuscriptDBContext manuscriptDataContextRead;
        private ManuscriptLoginDetailsRepository _manuscriptLoginDetailsRepository;
        private ManuscriptBookLoginDetailsRepository _manuscriptBookLoginDetailsRepository;
        private ManuscriptDBRepositoryReadSide _manuscriptDBRepositoryReadSide;
        private ManuscriptLoginDBRepositoryReadSide _manuscriptLoginDBRepositoryReadSide;
        private int _serviceTypeId, _roleId;
        private int jobProcessStatusIdForHold;
        private int assignedstatus_id;
        private int fetchstatus_id;
        private int openstatus_id;

        public AdminDashBoardReposistory(string conString)
        {
            this.manuscriptDataContextRead = new DataContexts.ManuscriptDBContext(conString);
            _manuscriptLoginDetailsRepository = new ManuscriptLoginDetailsRepository(conString);
            _manuscriptDBRepositoryReadSide = new ManuscriptDBRepositoryReadSide(conString);
            _manuscriptLoginDBRepositoryReadSide = new ManuscriptLoginDBRepositoryReadSide(conString);
            _manuscriptBookLoginDetailsRepository = new ManuscriptBookLoginDetailsRepository(conString);
        }

        public bool IsAssociateAllocatedToMSID(AdminDashBoardDTO adminDashBoardDTO)
        {
            _serviceTypeId = GetServiceTypeId(adminDashBoardDTO.ServiceType);
            _roleId = GetRoleId(adminDashBoardDTO.Role);
            var jobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "assigned").Select(x => x.ID).FirstOrDefault();
            var jobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
            var userRoleId = (from loginDetails in manuscriptDataContextRead.ManuscriptLoginDetails
                              where loginDetails.CrestId == adminDashBoardDTO.CrestId
                              && loginDetails.ServiceTypeStatusId == _serviceTypeId
                              && loginDetails.RoleId == _roleId
                              && loginDetails.JobProcessStatusId == jobProcessStatusId 
                              && loginDetails.JobStatusId == jobStatusId
                              select loginDetails.UserRoleId).FirstOrDefault();

            if (Convert.ToInt32(userRoleId) == 0 || userRoleId.ToString().Trim() == "")
            {
                return true;
            }
            else
            {

                return false;
            }
        }

        public bool IsAssociateAllocatedToMSIDChapter(AdminDashBoardDTO adminDashBoardDTO)
        {
            _serviceTypeId = GetServiceTypeId(adminDashBoardDTO.ServiceType);
            _roleId = GetRoleId(adminDashBoardDTO.Role);
            var jobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "assigned").Select(x => x.ID).FirstOrDefault();
            var jobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
            var userRoleId = (from loginDetails in manuscriptDataContextRead.ManuscriptBookLoginDetails
                              where loginDetails.ManuscriptBookLoginId == adminDashBoardDTO.CrestId
                              && loginDetails.ServiceTypeStatusId == _serviceTypeId
                              && loginDetails.RoleId == _roleId
                              && loginDetails.JobProcessStatusId == jobProcessStatusId
                              && loginDetails.JobStatusId == jobStatusId
                              select loginDetails.UserRoleId).FirstOrDefault();

            if (Convert.ToInt32(userRoleId) == 0 || userRoleId.ToString().Trim() == "")
            {
                return true;
            }
            else
            {

                return false;
            }
        }

        public bool IsAssociateAllocatedToChapter(AdminDashBoardDTO _adminDashBoardDTO)
        {
            try
            {
                _serviceTypeId = GetServiceTypeId(_adminDashBoardDTO.ServiceType);
                _roleId = GetRoleId(_adminDashBoardDTO.Role);
                var jobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
                var fetchStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "in-process").Select(x => x.ID).FirstOrDefault();
                var qualityFetchStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "in qc").Select(x => x.ID).FirstOrDefault();
                var jobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "assigned").Select(x => x.ID).FirstOrDefault();
                var MSIDAssociateToUser = (from MSD in manuscriptDataContextRead.ManuscriptBookLoginDetails
                                           where MSD.ManuscriptBookLoginId == _adminDashBoardDTO.CrestId
                                                 && MSD.RoleId == _roleId
                                                 && MSD.ServiceTypeStatusId == _serviceTypeId
                                                 && MSD.JobStatusId == jobStatusId
                                                 && (MSD.JobProcessStatusId == jobProcessStatusId || MSD.JobProcessStatusId == fetchStatusId || MSD.JobProcessStatusId == qualityFetchStatusId)
                                           select MSD.UserRoleId).SingleOrDefault();
                if (Convert.ToString(MSIDAssociateToUser) == "")
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool UnallocateAssociateUser(AdminDashBoardDTO _adminDashBoardDTO)
        {
            if (IsAssociateAllocateToMSID(_adminDashBoardDTO))
            {
                var jobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "assigned").Select(x => x.ID).FirstOrDefault();
                var fetchStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "in-process").Select(x => x.ID).FirstOrDefault();
                var qualityFetchStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "in qc").Select(x => x.ID).FirstOrDefault();
                var jobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
                var _updatemanuscriptLoginDetails = (from loginDetails in manuscriptDataContextRead.ManuscriptLoginDetails
                                                     where loginDetails.CrestId == _adminDashBoardDTO.CrestId
                                                           && loginDetails.RoleId == _roleId
                                                           && loginDetails.ServiceTypeStatusId == _serviceTypeId
                                                           && (loginDetails.JobProcessStatusId == jobProcessStatusId || loginDetails.JobProcessStatusId == fetchStatusId || loginDetails.JobProcessStatusId == qualityFetchStatusId)
                                                           && loginDetails.JobStatusId == jobStatusId
                                                     select loginDetails).FirstOrDefault();

                var manuscriptLoginDetails = new ManuscriptLoginDetails
                {
                    CrestId = _updatemanuscriptLoginDetails.CrestId,
                    JobStatusId = _updatemanuscriptLoginDetails.JobStatusId,
                    ServiceTypeStatusId = _updatemanuscriptLoginDetails.ServiceTypeStatusId,
                    RoleId = _updatemanuscriptLoginDetails.RoleId,
                    JobProcessStatusId = _updatemanuscriptLoginDetails.JobStatusId,
                };
                _adminDashBoardDTO.manuscriptLoginDetails.Add(manuscriptLoginDetails);

                _updatemanuscriptLoginDetails.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "unallocate").Select(x => x.ID).FirstOrDefault();
                _updatemanuscriptLoginDetails.FetchedDate = DateTime.Now;
                _updatemanuscriptLoginDetails.SubmitedDate = DateTime.Now;
                _updatemanuscriptLoginDetails.JobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "close").Select(x => x.ID).FirstOrDefault();
                _adminDashBoardDTO.manuscriptLoginDetails.Add(_updatemanuscriptLoginDetails);
                SaveManuscriptLoginDetails(_adminDashBoardDTO);

                return true;
            }
            else
            {

                return false;
            }
        }

        public bool UnallocateAssociateUserFromChapter(AdminDashBoardDTO _adminDashBoardDTO)
        {
            if (IsAssociateAllocatedToChapter(_adminDashBoardDTO))
            {
                var jobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "assigned").Select(x => x.ID).FirstOrDefault();
                var fetchStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "in-process").Select(x => x.ID).FirstOrDefault();
                var qualityFetchStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "in qc").Select(x => x.ID).FirstOrDefault();
                var jobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
                var _updatemanuscriptBookLoginDetails = (from loginDetails in manuscriptDataContextRead.ManuscriptBookLoginDetails
                                                         where loginDetails.ManuscriptBookLoginId == _adminDashBoardDTO.CrestId
                                                               && loginDetails.RoleId == _roleId
                                                               && loginDetails.ServiceTypeStatusId == _serviceTypeId
                                                               && (loginDetails.JobProcessStatusId == jobProcessStatusId || loginDetails.JobProcessStatusId == fetchStatusId || loginDetails.JobProcessStatusId == qualityFetchStatusId)
                                                               && loginDetails.JobStatusId == jobStatusId
                                                         select loginDetails).FirstOrDefault();

                var manuscriptBookLoginDetails = new ManuscriptBookLoginDetails()
                {
                    ManuscriptBookLoginId = _updatemanuscriptBookLoginDetails.ManuscriptBookLoginId,
                    JobStatusId = _updatemanuscriptBookLoginDetails.JobStatusId,
                    ServiceTypeStatusId = _updatemanuscriptBookLoginDetails.ServiceTypeStatusId,
                    RoleId = _updatemanuscriptBookLoginDetails.RoleId,
                    JobProcessStatusId = _updatemanuscriptBookLoginDetails.JobStatusId,
                };
                _adminDashBoardDTO.manuscriptBookLoginDetails.Add(manuscriptBookLoginDetails);

                _updatemanuscriptBookLoginDetails.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "unallocate").Select(x => x.ID).FirstOrDefault();
                _updatemanuscriptBookLoginDetails.FetchedDate = DateTime.Now;
                _updatemanuscriptBookLoginDetails.SubmitedDate = DateTime.Now;
                _updatemanuscriptBookLoginDetails.JobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "close").Select(x => x.ID).FirstOrDefault();
                _adminDashBoardDTO.manuscriptBookLoginDetails.Add(_updatemanuscriptBookLoginDetails);
                SaveManuscriptBookLoginDetails(_adminDashBoardDTO);

                return true;
            }
            else
            {

                return false;
            }
        }

        private bool IsAssociateAllocateToMSID(AdminDashBoardDTO _adminDashBoardDTO)
        {
            try
            {
                _serviceTypeId = GetServiceTypeId(_adminDashBoardDTO.ServiceType);
                _roleId = GetRoleId(_adminDashBoardDTO.Role);
                var jobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
                var fetchStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "in-process").Select(x => x.ID).FirstOrDefault();
                var qualityFetchStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "in qc").Select(x => x.ID).FirstOrDefault();
                var jobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "assigned").Select(x => x.ID).FirstOrDefault();
                var MSIDAssociateToUser = (from MSD in manuscriptDataContextRead.ManuscriptLoginDetails
                                           where MSD.CrestId == _adminDashBoardDTO.CrestId
                                                 && MSD.RoleId == _roleId
                                                 && MSD.ServiceTypeStatusId == _serviceTypeId
                                                 && MSD.JobStatusId == jobStatusId
                                                 && (MSD.JobProcessStatusId == jobProcessStatusId || MSD.JobProcessStatusId == fetchStatusId || MSD.JobProcessStatusId == qualityFetchStatusId)
                                           select MSD.UserRoleId).SingleOrDefault();
                if (Convert.ToString(MSIDAssociateToUser) == "")
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool AllocateJob_ForHoldJob(AdminDashBoardDTO _adminDashBoardDTO)
        {
            try
            {
                var prGetAssociateInfoResult = _manuscriptDBRepositoryReadSide.GetAssociateName(_adminDashBoardDTO.AssociateName).FirstOrDefault();
                var jobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "on hold").Select(x => x.ID).FirstOrDefault();
                var jobStatusIDvalue = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
                var _updatemanuscriptLoginDetailsForAssigned = (from loginDetails in manuscriptDataContextRead.ManuscriptLoginDetails
                                                                where loginDetails.CrestId == _adminDashBoardDTO.CrestId
                                                                      && loginDetails.RoleId == _roleId
                                                                      && loginDetails.ServiceTypeStatusId == _serviceTypeId
                                                                      && loginDetails.JobStatusId == jobStatusIDvalue
                                                                      && loginDetails.JobProcessStatusId == jobProcessStatusId
                                                                select loginDetails).FirstOrDefault();
                var jobProcessStatusIdOfAssigned = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "assigned").Select(x => x.ID).FirstOrDefault();

                var manuscriptLoginDetails = new ManuscriptLoginDetails
                {
                    CrestId = _adminDashBoardDTO.CrestId,
                    JobStatusId = _updatemanuscriptLoginDetailsForAssigned.JobStatusId,
                    ServiceTypeStatusId = _serviceTypeId,
                    RoleId = _updatemanuscriptLoginDetailsForAssigned.RoleId,
                    JobProcessStatusId = jobProcessStatusIdOfAssigned,
                    UserRoleId = prGetAssociateInfoResult.ID,
                    AssignedDate = DateTime.Now,
                };
                _adminDashBoardDTO.manuscriptLoginDetails.Add(manuscriptLoginDetails);
                _updatemanuscriptLoginDetailsForAssigned.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "on hold").Select(x => x.ID).FirstOrDefault();
                _updatemanuscriptLoginDetailsForAssigned.FetchedDate = DateTime.Now;
                _updatemanuscriptLoginDetailsForAssigned.SubmitedDate = DateTime.Now;
                _updatemanuscriptLoginDetailsForAssigned.ModifiedDate = DateTime.Now;
                _updatemanuscriptLoginDetailsForAssigned.JobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "close").Select(x => x.ID).FirstOrDefault();
                _adminDashBoardDTO.manuscriptLoginDetails.Add(_updatemanuscriptLoginDetailsForAssigned);
                SaveManuscriptLoginDetails(_adminDashBoardDTO);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AllocateBookChapter_ForHoldJob(AdminDashBoardDTO _adminDashBoardDTO)
        {
            try
            {
                var prGetAssociateInfoResult = _manuscriptDBRepositoryReadSide.GetAssociateName(_adminDashBoardDTO.AssociateName).FirstOrDefault();
                var jobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "on hold").Select(x => x.ID).FirstOrDefault();
                var jobStatusIDvalue = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
                var _updatemanuscriptLoginDetailsForAssigned = (from loginDetails in manuscriptDataContextRead.ManuscriptBookLoginDetails
                                                                where loginDetails.ManuscriptBookLoginId == _adminDashBoardDTO.CrestId
                                                                      && loginDetails.RoleId == _roleId
                                                                      && loginDetails.ServiceTypeStatusId == _serviceTypeId
                                                                      && loginDetails.JobStatusId == jobStatusIDvalue
                                                                      && loginDetails.JobProcessStatusId == jobProcessStatusId
                                                                select loginDetails).FirstOrDefault();
                var jobProcessStatusIdOfAssigned = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "assigned").Select(x => x.ID).FirstOrDefault();

                var manuscriptLoginDetails = new ManuscriptBookLoginDetails
                {
                    ManuscriptBookLoginId = _adminDashBoardDTO.CrestId,
                    JobStatusId = _updatemanuscriptLoginDetailsForAssigned.JobStatusId,
                    ServiceTypeStatusId = _serviceTypeId,
                    RoleId = _updatemanuscriptLoginDetailsForAssigned.RoleId,
                    JobProcessStatusId = jobProcessStatusIdOfAssigned,
                    UserRoleId = prGetAssociateInfoResult.ID,
                    AssignedDate = DateTime.Now,
                };
                _adminDashBoardDTO.manuscriptBookLoginDetails.Add(manuscriptLoginDetails);
                _updatemanuscriptLoginDetailsForAssigned.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "on hold").Select(x => x.ID).FirstOrDefault();
                _updatemanuscriptLoginDetailsForAssigned.FetchedDate = DateTime.Now;
                _updatemanuscriptLoginDetailsForAssigned.SubmitedDate = DateTime.Now;
                _updatemanuscriptLoginDetailsForAssigned.ModifiedDate = DateTime.Now;
                _updatemanuscriptLoginDetailsForAssigned.JobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "close").Select(x => x.ID).FirstOrDefault();
                _adminDashBoardDTO.manuscriptBookLoginDetails.Add(_updatemanuscriptLoginDetailsForAssigned);
                SaveManuscriptBookLoginDetails(_adminDashBoardDTO);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AllocateAssociateToMSID(AdminDashBoardDTO _adminDashBoardDTO)
        {
            try
            {
                if (IsAssociateAllocatedToMSID(_adminDashBoardDTO))
                {
                    if (_adminDashBoardDTO.JobProcessingStatus == "On Hold")
                    {
                        bool result = AllocateJob_ForHoldJob(_adminDashBoardDTO);
                        return result;
                    }
                    else
                    {
                        var prGetAssociateInfoResult = _manuscriptDBRepositoryReadSide.GetAssociateName(_adminDashBoardDTO.AssociateName).FirstOrDefault();
                        var jobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
                        var jobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
                        var _updatemanuscriptLoginDetails = (from loginDetails in manuscriptDataContextRead.ManuscriptLoginDetails
                                                             where loginDetails.CrestId == _adminDashBoardDTO.CrestId
                                                                   && loginDetails.RoleId == _roleId
                                                                   && loginDetails.ServiceTypeStatusId == _serviceTypeId
                                                                   && loginDetails.JobProcessStatusId == jobProcessStatusId
                                                                   && loginDetails.JobStatusId == jobStatusId
                                                             select loginDetails).SingleOrDefault();
                        _updatemanuscriptLoginDetails.UserRoleId = prGetAssociateInfoResult.ID;
                        _updatemanuscriptLoginDetails.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "assigned").Select(x => x.ID).FirstOrDefault();
                        _updatemanuscriptLoginDetails.AssignedDate = DateTime.Now;
                        _adminDashBoardDTO.manuscriptLoginDetails.Add(_updatemanuscriptLoginDetails);
                        SaveManuscriptLoginDetails(_adminDashBoardDTO);
                        return true;
                    }
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AllocateAssociateToChapter(AdminDashBoardDTO _adminDashBoardDTO)
        {
            try
            {
                if (IsAssociateAllocatedToMSIDChapter(_adminDashBoardDTO))
                {
                    if (_adminDashBoardDTO.JobProcessingStatus == "On Hold")
                    {
                        bool result = AllocateBookChapter_ForHoldJob(_adminDashBoardDTO);
                        return result;
                    }
                    else
                    {
                        var prGetAssociateInfoResult = _manuscriptDBRepositoryReadSide.GetAssociateName(_adminDashBoardDTO.AssociateName).FirstOrDefault();
                        var jobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
                        var jobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
                        var _updatemanuscriptLoginDetails = (from loginDetails in manuscriptDataContextRead.ManuscriptBookLoginDetails
                                                             where loginDetails.ManuscriptBookLoginId == _adminDashBoardDTO.CrestId
                                                                   && loginDetails.RoleId == _roleId
                                                                   && loginDetails.ServiceTypeStatusId == _serviceTypeId
                                                                   && loginDetails.JobProcessStatusId == jobProcessStatusId
                                                                   && loginDetails.JobStatusId == jobStatusId
                                                             select loginDetails).SingleOrDefault();
                        _updatemanuscriptLoginDetails.UserRoleId = prGetAssociateInfoResult.ID;
                        _updatemanuscriptLoginDetails.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "assigned").Select(x => x.ID).FirstOrDefault();
                        _updatemanuscriptLoginDetails.AssignedDate = DateTime.Now;
                        _adminDashBoardDTO.manuscriptBookLoginDetails.Add(_updatemanuscriptLoginDetails);
                        SaveManuscriptBookLoginDetails(_adminDashBoardDTO);
                        return true;
                    }
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public void SaveManuscriptLoginDetails(AdminDashBoardDTO _adminDashBoardDTO)
        {
            foreach (var item in _adminDashBoardDTO.manuscriptLoginDetails)
            {
                if (item.Id == 0 || item.Id == null)
                    _manuscriptLoginDetailsRepository.AddManuscriptLoginDetails(item);
                else
                {
                    _manuscriptLoginDetailsRepository.UpdateManuscriptLoginDetails(item);
                }
            }
            _manuscriptLoginDetailsRepository.SaveChanges();

        }

        public void SaveManuscriptBookLoginDetails(AdminDashBoardDTO _adminDashBoardDTO)
        {
            foreach (var item in _adminDashBoardDTO.manuscriptBookLoginDetails)
            {
                if (item.Id == 0 || item.Id == null)
                    _manuscriptBookLoginDetailsRepository.AddManuscriptBookLoginDetails(item);
                else
                {
                    _manuscriptBookLoginDetailsRepository.UpdateManuscriptBookLoginDetails(item);
                }
            }
            _manuscriptBookLoginDetailsRepository.SaveChanges();

        }


        private int GetJobStatusId()
        {
            var jobStatusId = (from jobStatus in manuscriptDataContextRead.StatusMaster
                               where jobStatus.Description == "assigned"
                               select jobStatus.ID).First();
            return Convert.ToInt32(jobStatusId);
        }

        private int GetRoleId(string role)
        {
            var roleId = (from roles in manuscriptDataContextRead.Roles
                          where roles.RoleName == role
                          select roles.ID).FirstOrDefault();
            return Convert.ToInt32(roleId);
        }

        private int GetServiceTypeId(string serviceType)
        {
            var serviceTypeId = (from statusMaster in manuscriptDataContextRead.StatusMaster
                                 where statusMaster.Description == serviceType
                                 select statusMaster.ID).FirstOrDefault();
            return Convert.ToInt32(serviceTypeId);
        }

        public bool HoldMSIDForJob(AdminDashBoardDTO _adminDashBoardDTO)
        {
            jobProcessStatusIdForHold = IsJobOpenMSID(_adminDashBoardDTO);
            assignedstatus_id = _manuscriptDBRepositoryReadSide.GetStatusID("assigned");
            fetchstatus_id = _manuscriptDBRepositoryReadSide.GetStatusID(" In-Process");
            openstatus_id = _manuscriptDBRepositoryReadSide.GetStatusID("Open");
            try
            {
                if (jobProcessStatusIdForHold == assignedstatus_id || jobProcessStatusIdForHold == fetchstatus_id)
                {
                    var jobStatusIDvalue = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
                    var _updatemanuscriptLoginDetailsForAssigned = (from loginDetails in manuscriptDataContextRead.ManuscriptLoginDetails
                                                                    where loginDetails.CrestId == _adminDashBoardDTO.CrestId
                                                                          && loginDetails.RoleId == _roleId
                                                                          && loginDetails.ServiceTypeStatusId == _serviceTypeId
                                                                          && loginDetails.JobStatusId == jobStatusIDvalue
                                                                          && loginDetails.JobProcessStatusId == jobProcessStatusIdForHold
                                                                    select loginDetails).FirstOrDefault();
                    var jobProcessStatusIdOfHold = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "on hold").Select(x => x.ID).FirstOrDefault();

                    var manuscriptLoginDetails = new ManuscriptLoginDetails
                    {
                        CrestId = _adminDashBoardDTO.CrestId,
                        JobStatusId = _updatemanuscriptLoginDetailsForAssigned.JobStatusId,
                        ServiceTypeStatusId = _serviceTypeId,
                        RoleId = _updatemanuscriptLoginDetailsForAssigned.RoleId,
                        JobProcessStatusId = jobProcessStatusIdOfHold,
                        AssignedDate = DateTime.Now,
                    };
                    _adminDashBoardDTO.manuscriptLoginDetails.Add(manuscriptLoginDetails);
                    _updatemanuscriptLoginDetailsForAssigned.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "unallocate").Select(x => x.ID).FirstOrDefault();
                    _updatemanuscriptLoginDetailsForAssigned.FetchedDate = DateTime.Now;
                    _updatemanuscriptLoginDetailsForAssigned.SubmitedDate = DateTime.Now;
                    _updatemanuscriptLoginDetailsForAssigned.ModifiedDate = DateTime.Now;
                    _updatemanuscriptLoginDetailsForAssigned.JobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "close").Select(x => x.ID).FirstOrDefault();
                    _adminDashBoardDTO.manuscriptLoginDetails.Add(_updatemanuscriptLoginDetailsForAssigned);
                    SaveManuscriptLoginDetails(_adminDashBoardDTO);
                    return true;
                }
                else if (jobProcessStatusIdForHold == openstatus_id)
                {
                    var jobStatusIDvalue =
                        _manuscriptLoginDBRepositoryReadSide.GetStatusMaster()
                            .Where(x => x.Description.ToLower() == "open")
                            .Select(x => x.ID)
                            .FirstOrDefault();
                    var _updatemanuscriptLoginDetailsForOpen = (from loginDetails in manuscriptDataContextRead.ManuscriptLoginDetails
                                                                where loginDetails.CrestId == _adminDashBoardDTO.CrestId
                                                                                               && loginDetails.RoleId == _roleId
                                                                                               && loginDetails.ServiceTypeStatusId == _serviceTypeId
                                                                                               && loginDetails.JobStatusId == jobStatusIDvalue
                                                                                               && loginDetails.JobProcessStatusId == jobProcessStatusIdForHold
                                                                select loginDetails).FirstOrDefault();

                    _updatemanuscriptLoginDetailsForOpen.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "on hold").Select(x => x.ID).FirstOrDefault();
                    _updatemanuscriptLoginDetailsForOpen.ModifiedDate = DateTime.Now;
                    _updatemanuscriptLoginDetailsForOpen.AssignedDate = DateTime.Now;
                    _updatemanuscriptLoginDetailsForOpen.ModifiedDate = DateTime.Now;
                    _updatemanuscriptLoginDetailsForOpen.JobStatusId = _updatemanuscriptLoginDetailsForOpen.JobStatusId;
                    _adminDashBoardDTO.manuscriptLoginDetails.Add(_updatemanuscriptLoginDetailsForOpen);
                    SaveManuscriptLoginDetails(_adminDashBoardDTO);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool OnHoldBookChapter(AdminDashBoardDTO _adminDashBoardDTO)
        {
            jobProcessStatusIdForHold = CheckBookChapterIsopen(_adminDashBoardDTO);
            assignedstatus_id = _manuscriptDBRepositoryReadSide.GetStatusID("assigned");
            fetchstatus_id = _manuscriptDBRepositoryReadSide.GetStatusID("In-Process");
            openstatus_id = _manuscriptDBRepositoryReadSide.GetStatusID("Open");
            try
            {
                if (jobProcessStatusIdForHold == assignedstatus_id || jobProcessStatusIdForHold == fetchstatus_id)
                {
                    var jobStatusIDvalue = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
                    var _updatemanuscriptLoginDetailsForAssigned = (from loginDetails in manuscriptDataContextRead.ManuscriptBookLoginDetails
                                                                    where loginDetails.ManuscriptBookLoginId == _adminDashBoardDTO.CrestId
                                                                          && loginDetails.RoleId == _roleId
                                                                          && loginDetails.ServiceTypeStatusId == _serviceTypeId
                                                                          && loginDetails.JobStatusId == jobStatusIDvalue
                                                                          && loginDetails.JobProcessStatusId == jobProcessStatusIdForHold
                                                                    select loginDetails).FirstOrDefault();
                    var jobProcessStatusIdOfHold = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "on hold").Select(x => x.ID).FirstOrDefault();

                    var manuscriptLoginDetails = new ManuscriptBookLoginDetails()
                    {
                        ManuscriptBookLoginId = _adminDashBoardDTO.CrestId,
                        JobStatusId = _updatemanuscriptLoginDetailsForAssigned.JobStatusId,
                        ServiceTypeStatusId = _serviceTypeId,
                        RoleId = _updatemanuscriptLoginDetailsForAssigned.RoleId,
                        JobProcessStatusId = jobProcessStatusIdOfHold,
                        AssignedDate = DateTime.Now,
                    };
                    _adminDashBoardDTO.manuscriptBookLoginDetails.Add(manuscriptLoginDetails);
                    _updatemanuscriptLoginDetailsForAssigned.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "unallocate").Select(x => x.ID).FirstOrDefault();
                    _updatemanuscriptLoginDetailsForAssigned.FetchedDate = DateTime.Now;
                    _updatemanuscriptLoginDetailsForAssigned.SubmitedDate = DateTime.Now;
                    _updatemanuscriptLoginDetailsForAssigned.ModifiedDate = DateTime.Now;
                    _updatemanuscriptLoginDetailsForAssigned.JobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "close").Select(x => x.ID).FirstOrDefault();
                    _adminDashBoardDTO.manuscriptBookLoginDetails.Add(_updatemanuscriptLoginDetailsForAssigned);
                    SaveManuscriptBookLoginDetails(_adminDashBoardDTO);
                    return true;
                }
                else if (jobProcessStatusIdForHold == openstatus_id)
                {
                    var jobStatusIDvalue =
                        _manuscriptLoginDBRepositoryReadSide.GetStatusMaster()
                            .Where(x => x.Description.ToLower() == "open")
                            .Select(x => x.ID)
                            .FirstOrDefault();
                    var _updatemanuscriptLoginDetailsForOpen = (from loginDetails in manuscriptDataContextRead.ManuscriptBookLoginDetails
                                                                where loginDetails.ManuscriptBookLoginId == _adminDashBoardDTO.CrestId
                                                                                               && loginDetails.RoleId == _roleId
                                                                                               && loginDetails.ServiceTypeStatusId == _serviceTypeId
                                                                                               && loginDetails.JobStatusId == jobStatusIDvalue
                                                                                               && loginDetails.JobProcessStatusId == jobProcessStatusIdForHold
                                                                select loginDetails).FirstOrDefault();

                    _updatemanuscriptLoginDetailsForOpen.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "on hold").Select(x => x.ID).FirstOrDefault();
                    _updatemanuscriptLoginDetailsForOpen.ModifiedDate = DateTime.Now;
                    _updatemanuscriptLoginDetailsForOpen.AssignedDate = DateTime.Now;
                    _updatemanuscriptLoginDetailsForOpen.ModifiedDate = DateTime.Now;
                    _updatemanuscriptLoginDetailsForOpen.JobStatusId = _updatemanuscriptLoginDetailsForOpen.JobStatusId;
                    _adminDashBoardDTO.manuscriptBookLoginDetails.Add(_updatemanuscriptLoginDetailsForOpen);
                    SaveManuscriptBookLoginDetails(_adminDashBoardDTO);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        private int CheckBookChapterIsopen(AdminDashBoardDTO _adminDashBoardDTO)
        {
            try
            {
                _serviceTypeId = GetServiceTypeId(_adminDashBoardDTO.ServiceType);
                _roleId = GetRoleId(_adminDashBoardDTO.Role);
                var jobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
                var IsJobOpen = (from MSD in manuscriptDataContextRead.ManuscriptBookLoginDetails
                                 where MSD.ManuscriptBookLoginId == _adminDashBoardDTO.CrestId
                                       && MSD.RoleId == _roleId
                                       && MSD.ServiceTypeStatusId == _serviceTypeId
                                       && MSD.JobStatusId == jobStatusId
                                 select MSD.JobProcessStatusId).SingleOrDefault();
                if ((Convert.ToInt32(IsJobOpen)) == 7 || (Convert.ToInt32(IsJobOpen)) == 11 || (Convert.ToInt32(IsJobOpen)) == 13)
                    return (Convert.ToInt32(IsJobOpen));
                else
                    return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private int IsJobOpenMSID(AdminDashBoardDTO _adminDashBoardDTO)
        {
            try
            {
                _serviceTypeId = GetServiceTypeId(_adminDashBoardDTO.ServiceType);
                _roleId = GetRoleId(_adminDashBoardDTO.Role);
                var jobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
                var IsJobOpen = (from MSD in manuscriptDataContextRead.ManuscriptLoginDetails
                                 where MSD.CrestId == _adminDashBoardDTO.CrestId
                                       && MSD.RoleId == _roleId
                                       && MSD.ServiceTypeStatusId == _serviceTypeId
                                       && MSD.JobStatusId == jobStatusId
                                 select MSD.JobProcessStatusId).SingleOrDefault();
                if ((Convert.ToInt32(IsJobOpen)) == 7 || (Convert.ToInt32(IsJobOpen)) == 11 || (Convert.ToInt32(IsJobOpen)) == 13)
                    return (Convert.ToInt32(IsJobOpen));
                else
                    return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public IEnumerable<pr_AdminDashBoardGetAllOpenJobs_Result> pr_GetAllJobsDetails()
        {
            try
            {

                IEnumerable<pr_AdminDashBoardGetAllOpenJobs_Result> alljobsdetails = this.manuscriptDataContextRead.Database.SqlQuery
                                                                                  <pr_AdminDashBoardGetAllOpenJobs_Result>("exec pr_AdminDashBoardGetAllOpenJobs").ToList();
                return alljobsdetails;
            }
            catch
            {
                return null;//todo:check and remove this trycatchhandler
            }
            finally
            {

            }
        }

        public List<pr_AdminDashBoardGetAllOpenJobsForExport_Result> GetAdminDashBoardExportToExcel(string FromDate1, string ToDate1)
        {
            try
            {
                var FromDate = Convert.ToDateTime(FromDate1);
                var ToDate = Convert.ToDateTime(ToDate1);

                var FromDateParameter = FromDate != null ?
              new SqlParameter("FromDate", FromDate) :
              new SqlParameter("FromDate", typeof(global::System.DateTime));

                var ToDateParameter = ToDate != null ?
                    new SqlParameter("ToDate", ToDate) :
                    new SqlParameter("ToDate", typeof(global::System.DateTime));

                List<pr_AdminDashBoardGetAllOpenJobs_Result> AdminDashBoardExportToExcel =
             this.manuscriptDataContextRead.Database.SqlQuery<pr_AdminDashBoardGetAllOpenJobs_Result>("pr_AdminDashBoardExportToExcel @FromDate, @ToDate", FromDateParameter, ToDateParameter).ToList();

                var AdminDashBoardExportToExcelData = (from q in AdminDashBoardExportToExcel
                                                       select new pr_AdminDashBoardGetAllOpenJobsForExport_Result()
                                                       {
                                                           SrNo = q.SrNo,
                                                           CrestId = q.CrestId,
                                                           JobType = q.JobType,
                                                           ServiceType = q.ServiceType,
                                                           MSID = q.MSID,
                                                           JournalBookName = q.JournalBookName,
                                                           PageCount = q.PageCount,
                                                           Name = q.Name,
                                                           Role = q.Role,
                                                           Status = q.Status,
                                                           Task = q.Task,
                                                           Revision = q.Revision,
                                                           GroupNo = q.GroupNo,
                                                           CreatedDate = q.CreatedDate,
                                                           ReceivedDate = q.ReceivedDate.HasValue ? q.ReceivedDate.Value.ToString("dd/MM/yyyy") : String.Empty,
                                                           FetchedDate = q.FetchedDate,
                                                           Age = q.Age,
                                                           HandlingTime = q.HandlingTime

                                                       }).ToList<pr_AdminDashBoardGetAllOpenJobsForExport_Result>();
                return AdminDashBoardExportToExcelData;
            }
            catch
            {
                return null;//todo:check and remove this trycatchhandler
            }
            finally
            {

            }

        }

        public List<pr_GetLastAssociateName_Result> GetLastNameOfAssociate_ForHoldJob(int crestid, string servicetype, string JobType)
        {

            int serviceid = manuscriptDataContextRead.Database.SqlQuery<Int32>("SELECT ID from StatusMaster Where Description = '" + servicetype + "'").FirstOrDefault<Int32>();
            try
            {
                var crestidbyParameter = crestid != null ?
                    new SqlParameter("CrestID", crestid) :
                    new SqlParameter("CrestID", typeof(global::System.Int32));

                var servicetypebyparameter = serviceid != null ?
                    new SqlParameter("ServiceType", serviceid) :
                    new SqlParameter("ServiceType", typeof(global::System.Int32));

                var jobTypebyparameter = JobType != null ?
                  new SqlParameter("JobType", JobType) :
                  new SqlParameter("JobType", typeof(global::System.String));

                List<pr_GetLastAssociateName_Result> associatename = this.manuscriptDataContextRead.Database.SqlQuery<pr_GetLastAssociateName_Result>("exec pr_GetLastAssociateName @CrestID,@ServiceType,@JobType", crestidbyParameter, servicetypebyparameter, jobTypebyparameter).ToList();
                return associatename;

            }
            catch
            {
                return null;//todo:check and remove this trycatchhandler
            }

            return null;
        }

        public IEnumerable<pr_GetAssociate> GetAssociateResult(string searchAssociate, string RoleName)
        {
            try
            {
                var associateByParameter = RoleName != null ?
                    new SqlParameter("RoleName", RoleName) :
                    new SqlParameter("RoleName", typeof(global::System.String));

                var searchAssociateParameter = searchAssociate != null ?
                    new SqlParameter("searchAssociate", searchAssociate) :
                    new SqlParameter("searchAssociate", typeof(global::System.String));
                IEnumerable<pr_GetAssociate> empDetails = this.manuscriptDataContextRead.Database.SqlQuery<pr_GetAssociate>("exec pr_GetAssociate @searchAssociate,@RoleName", searchAssociateParameter, associateByParameter).ToList();
                return empDetails;

            }
            catch
            {
                return null;//todo:check and remove this trycatchhandler
            }
            finally
            {

            }

        }

        public IEnumerable<pr_GetAutoLoginJournalDetails> GetAutoLoginJournalDetails(string createdDateTime)
        {
            try
            {
                var createdDate = createdDateTime != null ?
                    new SqlParameter("CreatedDate", createdDateTime) :
                    new SqlParameter("CreatedDate", typeof(global::System.DateTime));

                IEnumerable<pr_GetAutoLoginJournalDetails> getAutoLoginJournalDetails = this.manuscriptDataContextRead.Database.SqlQuery<pr_GetAutoLoginJournalDetails>("exec spAutoLoginJournalDetails @CreatedDate", createdDate).ToList();
                return getAutoLoginJournalDetails;
                //IEnumerable<pr_GetAutoLoginJournalDetails> getAutoLoginJournalDetails = this.manuscriptDataContextRead.Database.SqlQuery<pr_GetAutoLoginJournalDetails>("exec spAutoLoginJournalDetails").ToList();
                //return getAutoLoginJournalDetails;

            }
            catch(Exception ex)
            {
                throw ex; 
            }
        }



        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    manuscriptDataContextRead.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
