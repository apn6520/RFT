using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.Entities;

namespace TransferDesk.Contracts.Manuscript.DTO
{
    public class AdminDashBoardDTO
    {
        public List<Entities.ManuscriptLoginDetails> manuscriptLoginDetails { get; set; }
        public List<Entities.ManuscriptBookLoginDetails> manuscriptBookLoginDetails{ get; set; }
        public int CrestId { get; set; }
        public string ServiceType { get; set; }
        public string Role { get; set; }
        public string JobProcessingStatus { get; set; }
        public string AssociateName { get; set; }
        public string JobType { get; set; }
        public AdminDashBoardDTO()
        {
            manuscriptLoginDetails = new List<Entities.ManuscriptLoginDetails>();
            manuscriptBookLoginDetails=new List<ManuscriptBookLoginDetails>();
        }
        
    }
}
