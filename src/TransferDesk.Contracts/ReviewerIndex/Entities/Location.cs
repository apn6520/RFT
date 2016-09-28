
using System.ComponentModel.DataAnnotations;

namespace TransferDesk.Contracts.ReviewerIndex.Entities
{
    public class Location
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int? Locationtype { get; set; }
        public int? Isvisible { get; set; }
        public int? Parentid { get; set; }

    }
}