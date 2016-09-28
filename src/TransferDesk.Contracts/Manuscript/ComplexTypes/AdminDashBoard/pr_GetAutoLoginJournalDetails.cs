using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.ComplexTypes.AdminDashBoard
{
    public class pr_GetAutoLoginJournalDetails
    {
        public Int32? ID { get; set; }
        public Int32? JournalID { get; set; }
        public string JournalTitle { get; set; }
        public Int32? ArticleTypeID { get; set; }
        public string ArticleTypeName { get; set; }
        public string MSID { get; set; }
        public string ArticleTitle { get; set; }
        public DateTime? InitialSubmissionDate { get; set; }
        public string AdditionalComments { get; set; }
        public string Subject { get; set; }
        public string FromMail { get; set; }
        public string ToMail { get; set; }
        public string CCMail { get; set; }
        public string BCCMail { get; set; }
        public string MailBody { get; set; }
        public string EmailHtmlBody { get; set; }
        public string EmailImportance { get; set; }
        public Int32? Status { get; set; }
        public string ErrorDescription { get; set; }
        public DateTime? MailReceivedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
