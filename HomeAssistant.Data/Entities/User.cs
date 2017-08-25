using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAssistant.Data.Entities
{
    [Table("tblUser")]
    public class User : BaseEntity
    {
        [Required]
        public virtual Person Person { get; set; }
        
        [StringLength(128)]
        public string Password { get; set; }

        //protected virtual string PasswordStored
        //{
        //    get;
        //    set;
        //}
    }
}
