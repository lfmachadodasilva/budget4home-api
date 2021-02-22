using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace budget4home.Models
{
    [Table("Label")]
    public class LabelModel : BaseModel
    {
        [Required]
        public long GroupId { get; set; }
        [ForeignKey("GroupId")]
        public GroupModel Group { get; set; }
    }

    public class LabelFullModel : LabelModel
    {
        public decimal CurrValue { get; set; }
        public decimal LastValue { get; set; }
        public decimal AvgValue { get; set; }
    }
}