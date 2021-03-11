using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using budget4home.App.Base;
using budget4home.App.Groups;
using budget4home.App.Labels;

namespace budget4home.App.Expenses
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

        public int ScheduleBy { get; set; } = 1;
        public int ScheduleTotal { get; set; } = 1;
        public long? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public ExpenseModel Parent { get; set; }

        [Required]
        public long GroupId { get; set; }
        [ForeignKey("GroupId")]
        public GroupModel Group { get; set; }
        public long LabelId { get; set; }
        [ForeignKey("LabelId")]
        public LabelModel Label { get; set; }
    }
}