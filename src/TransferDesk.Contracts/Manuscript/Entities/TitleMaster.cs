using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class TitleMaster
    {
        [Key]
        public int TitleID { get; set; }
        public string MScriptID { get; set; }
        public string Name { get; set; }
        public string TitleName { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime? ModifiedDate { get; set; }
    }
}
