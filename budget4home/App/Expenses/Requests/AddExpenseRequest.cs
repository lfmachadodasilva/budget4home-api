using budget4home.App.Groups;
using budget4home.App.Labels;
using System;
using System.ComponentModel.DataAnnotations;

namespace budget4home.App.Expenses.Requests
{
    public class AddExpenseRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, 1)]
        public ExpenseType Type { get; set; } = ExpenseType.Outcoming;

        [Required]
        [Range(0.0, (double)decimal.MaxValue)]
        public decimal Value { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }

        [Range(1, 12)]
        public int Schedule { get; set; } = 1;

        [LabelValidation]
        public long LabelId { get; set; }

        [GroupValidation]
        public long GroupId { get; set; }
    }
}