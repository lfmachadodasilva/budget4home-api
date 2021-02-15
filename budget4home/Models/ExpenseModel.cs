using System;
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
        public ExpenseType Type { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }

        public GroupModel Group { get; set; }
        public GroupModel Label { get; set; }
    }
}