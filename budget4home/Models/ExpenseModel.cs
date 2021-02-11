using System;

namespace budget4home.Models
{
    public enum ExpenseType
    {
        Incoming = 0,
        Outcoming
    }

    public class ExpenseModel : BaseModel
    {
        public ExpenseType Type { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }
    }
}