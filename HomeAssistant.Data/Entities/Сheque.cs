using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAssistant.Data.Entities
{
    [Table("tblСheque")]
    public class Сheque : BaseEntity
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public virtual List<СhequeDetail> Details { get; set; }

        public DateTime Date { get; set; }

        public virtual Category Category { get; set; }
    }
}
