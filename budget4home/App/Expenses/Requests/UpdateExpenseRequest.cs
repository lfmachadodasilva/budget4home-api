using System;

namespace budget4home.App.Expenses.Requests
{
    public class UpdateExpenseRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ExpenseType Type { get; set; } = ExpenseType.Outcoming;
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }

        public long LabelId { get; set; }
    }
}