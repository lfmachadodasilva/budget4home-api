using System;

namespace budget4home.App.Expenses.Responses
{
    public class GetExpenseResponse
    {
        public string Name { get; set; }
        public ExpenseType Type { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }

        public int ScheduleBy { get; set; } = 1;
        public int ScheduleTotal { get; set; } = 1;
        public long ParentId { get; set; }

        public long LabelId { get; set; }
        public string LabelName { get; set; }
    }
}
