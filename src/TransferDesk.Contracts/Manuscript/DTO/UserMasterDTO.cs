using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.Entities;

namespace TransferDesk.Contracts.Manuscript.DTO
{
    public class UserRoleDTO
    {

        public Entities.UserRoles userroles { get; set; }
        public List<Entities.BookUserRoles> bookuser { get; set; }
        public List<Entities.JournalUserRoles> journaluser { get; set; }
        public Entities.JournalUserRoles JournalUserRoles { get; set; }
        public Entities.BookUserRoles BookUserRoles { get; set; }
        public List<Entities.JournalUserRoles> deleteJournalUser { get; set; }
        public List<Entities.BookUserRoles> deleteBookUser { get; set; } 
        public UserRoleDTO()
        {
            userroles = new UserRoles();
            bookuser = new List<BookUserRoles>();
            journaluser = new List<JournalUserRoles>();
            JournalUserRoles=new JournalUserRoles();
            BookUserRoles=new BookUserRoles();
        }

        public string loginuser { get; set; }
        public int UserRoleId { get; set; }
    //    public string UserID { get; set; }
        public List<int> SelectedJournalIDs { get; set; }
        public List<int> SelectedJournalID { get; set; }
        public List<int> SelectedBookID { get; set; }
        public List<int> SelectedBookIDs { get; set; }
    }
}
