namespace budget4home.App.Expenses.Requests
{
    public class GetExpensesRequest
    {
        public long Group { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}