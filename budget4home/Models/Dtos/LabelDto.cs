using System.ComponentModel.DataAnnotations.Schema;

namespace budget4home.Models.Dtos
{
    public class LabelDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class LabelFullDto
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public decimal CurrValue { get; set; }
        public decimal LastValue { get; set; }
        public decimal AvgValue { get; set; }
    }
}