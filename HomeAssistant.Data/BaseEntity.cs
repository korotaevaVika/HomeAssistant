using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAssistant.Data
{
    public class BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid nKey { get; set; }
        public DateTime? tCreated { get; set; }
        public DateTime? tUpdated { get; set; }
        public DateTime? tDeleted { get; set; }
    }
}
