using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAssistant.Data.Entities
{
    [Table("tblPerson")]
    public class Person: BaseEntity
    {
        [StringLength(128)]
        public string Name { get; set; }
        [StringLength(128)]
        public string Surname { get; set; }
        [StringLength(128)]
        public string Email { get; set; }

        //public byte[] Image { get; set; }

    }
}
