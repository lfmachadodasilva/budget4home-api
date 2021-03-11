using System;

namespace budget4home.App.Expenses.Requests
{
    public class AddExpenseRequest
    {
        public string Name { get; set; }
        public ExpenseType Type { get; set; } = ExpenseType.Outcoming;
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }
        public int Schedule { get; set; } = 1;

        public long LabelId { get; set; }
        public long GroupId { get; set; }
    }
}