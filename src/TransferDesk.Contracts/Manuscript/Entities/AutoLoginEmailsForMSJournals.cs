using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class AutoLoginEmailsForMsJournals
    {
        [Key]
        public int ID { get; set; }
        public int JournalID { get; set; }
        public int ArticleTypeID { get; set; }
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
        public int Status { get; set; }
        public string ErrorDescription { get; set; }
        public DateTime? MailReceivedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
