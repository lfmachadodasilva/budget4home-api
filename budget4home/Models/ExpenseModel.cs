using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace budget4home.Models
{
    public enum ExpenseType
    {
        Incoming = 0,
        Outcoming
    }

    [Table("Expense")]
    public class ExpenseModel : BaseModel
    {
        [Required]
        public ExpenseType Type { get; set; }
        [Required]
        public decimal Value { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string Comments { get; set; }

        [Required]
        public long GroupId { get; set; }
        [ForeignKey("GroupId")]
        public GroupModel Group { get; set; }
        public long LabelId { get; set; }
        [ForeignKey("LabelId")]
        public LabelModel Label { get; set; }
    }
}