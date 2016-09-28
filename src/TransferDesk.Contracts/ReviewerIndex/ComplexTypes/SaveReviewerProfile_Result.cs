using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.ReviewerIndex.ComplexTypes
{
   public class SaveReviewerProfile_Result
    {
        public int ReviewerID { get; set; }
        public string Initials { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int? NoOfPublication { get; set; }
        public string ReviewerName { get; set; }
        public string StreetName { get; set; }
        public int? InstituteID { get; set; }
        public int? DeptID { get; set; }
        public string InstituteName { get; set; }
        public string DepartmentName { get; set; }
        public int? CityId { get; set; }
        public string City { get; set; }
        public int? StateId { get; set; }
        public string State { get; set; }
        public int? CountryID { get; set; }
        public string Country { get; set; }
        public int? TitleMasterID { get; set; }
        public string CreatedBy { get; set; }

        public List<ReviewerEmails_Result> ReviewerEmails { get; set; }      
        public List<ReferenceLink_Result> ReferenceLink { get; set; }
        public List<Journal_Result> Journal{ get; set; }
        public List<TitleReviewerlinkMaster_Result> TitleMaster { get; set; }       
        public List<AreaOfExpReviewerlink_Result> AreaOfExpReviewerlink { get; set; }

        public SaveReviewerProfile_Result()
        {
            ReviewerEmails = new List<ReviewerEmails_Result>();
            ReferenceLink = new List<ReferenceLink_Result>();
            Journal = new List<Journal_Result>();
            TitleMaster = new List<TitleReviewerlinkMaster_Result>();
            AreaOfExpReviewerlink = new List<AreaOfExpReviewerlink_Result>();        
        }
    }
}
