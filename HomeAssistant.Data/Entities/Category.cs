using HomeAssistant.Data.Converters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace HomeAssistant.Data.Entities
{
    [Table("tblCategory")]
    public class Category : BaseEntity
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; }
       
        // the mapped-to-column property 
        protected virtual byte[] bytesImage
        {
            get;
            set;
        }
        
        [NotMapped]
        public Image Password
        {
            get { return ImageToByteArrayConverter.byteArrayToImage(bytesImage); }
            set { bytesImage = ImageToByteArrayConverter.imageToByteArray(value); }
        }
    }
}
