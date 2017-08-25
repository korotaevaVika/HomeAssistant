using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAssistant.Data.Entities
{
    [Table("tblСhequeDetail")]
    public class СhequeDetail : BaseEntity
    {
        [Required]
        public Person Person { get; set; }
        public decimal? Amount { get; set; }
        public double? Pie { get; set; }
    }
}
