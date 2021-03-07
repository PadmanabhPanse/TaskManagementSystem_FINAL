namespace TaskManagementSystem.Models
{
    public class Subscription
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string taxType { get; set; }
        public decimal taxRate { get; set; }
        public QueryType queryType { get; set; }
        public QueryRange queryRange { get; set; }
        public decimal CostPerQuery { get; set; }
        public int Credits { get; set; }
        public decimal CostPerCredit { get; set; }
        public decimal TotalCost { get; set; }
    }
}
