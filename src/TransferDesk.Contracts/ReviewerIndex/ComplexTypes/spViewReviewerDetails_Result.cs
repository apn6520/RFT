using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.ReviewerIndex.ComplexTypes
{
    public class spViewReviewerDetails_Result
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
    }
}
