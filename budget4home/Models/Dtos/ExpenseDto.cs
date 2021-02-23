using System;

namespace budget4home.Models.Dtos
{
    public class ExpenseDto : BaseDto
    {
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

    public class ExpenseAddDto
    {
        public string Name { get; set; }
        public ExpenseType Type { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }
        public int ScheduleTotal { get; set; } = 1;
        public long LabelId { get; set; }
        public long GroupId { get; set; }
    }

    public class ExpenseUpdateDto : BaseDto
    {
        public ExpenseType Type { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }
        public long LabelId { get; set; }
        public long GroupId { get; set; }
    }
}