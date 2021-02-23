using System;

namespace budget4home.Models.Dtos
{
    public class ExpenseDto : BaseDto
    {
        public ExpenseType Type { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }

        public long LabelId { get; set; }
        public string LabelName { get; set; }
    }

    public class ExpenseManageDto : BaseDto
    {
        public ExpenseType Type { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }

        public long LabelId { get; set; }
        public long GroupId { get; set; }
    }
}