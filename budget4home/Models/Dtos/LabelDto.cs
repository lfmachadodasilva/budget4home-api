namespace budget4home.Models.Dtos
{
    public class LabelDto : BaseDto { }

    public class LabelFullDto : LabelDto
    {
        public decimal CurrValue { get; set; }
        public decimal LastValue { get; set; }
        public decimal AvgValue { get; set; }
    }

    public class LabelManageDto : LabelDto
    {
        public long GroupId { get; set; }
    }
}