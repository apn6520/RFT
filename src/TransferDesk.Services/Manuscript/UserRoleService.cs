using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.BAL.Manuscript;
using Entities = TransferDesk.Contracts.Manuscript.Entities;
using TransferDesk.Contracts.Manuscript.DTO;
using TransferDesk.BAL.Manuscript;
using TransferDesk.Services.Manuscript.ViewModel;
using System.Collections;
using System.Configuration;
namespace TransferDesk.Services.Manuscript
{
    public class UserRoleService
    {
        public String _ConStringRead { get; set; }
        public String _ConStringWrite { get; set; }
        public UserRoleBL _userRoleBL { get; set; }
        Entities.UserRoles userRoles { get; set; }
        private UserRoleDTO userRoleDto { get; set; }
        public UserRoleService()
        {

        }


        public UserRoleService(String conString)
        {
            _userRoleBL = new UserRoleBL(conString);
        }

        public UserRoleService(String ConStringRead, String ConStringWrite)
        {
            _ConStringRead = ConStringRead;
            _ConStringWrite = ConStringWrite;
            //   CreateUserRoleBL();
        }



        public bool SaveUserRoleDetails(UserRoleVM userRoleVm, Entities.UserRoles userRoles)
        {

            userRoleDto = new UserRoleDTO();
            userRoleDto = userRoleVm.FetchDTO;

            if (_userRoleBL.SaveUserRoles(userRoleDto))
            {
                return true;
            }
            else
                return false;

        }


    }
}
