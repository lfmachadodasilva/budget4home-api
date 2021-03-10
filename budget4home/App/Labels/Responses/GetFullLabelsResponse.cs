namespace budget4home.App.Labels.Responses
{
    public class GetFullLabelsResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal CurrValue { get; set; }
        public decimal LastValue { get; set; }
        public decimal AvgValue { get; set; }
    }
}