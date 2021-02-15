using System.ComponentModel.DataAnnotations.Schema;

namespace budget4home.Models
{
    [Table("Label")]
    public class LabelModel : BaseModel
    {
        public GroupModel Group { get; set; }
    }

    public class LabelFullModel : BaseModel
    {
        public GroupModel Group { get; set; }

        public decimal CurrValue { get; set; }
        public decimal LastValue { get; set; }
        public decimal AvgValue { get; set; }
    }
}