using budget4home.App.Labels;
using System;
using System.ComponentModel.DataAnnotations;

namespace budget4home.App.Expenses.Requests
{
    public class UpdateExpenseRequest
    {
        [ExpenseValidation]
        public long Id { get; set; }

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

        [LabelValidation]
        public long LabelId { get; set; }
    }
}