using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using budget4home.App.Base;
using budget4home.App.Groups;

namespace budget4home.App.Labels
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