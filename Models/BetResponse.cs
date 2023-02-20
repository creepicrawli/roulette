namespace roulette.Models
{
    public class BetResponse
    {
        public bool Successful { get; set; }
        public object? Spin { get; set; }
        public bool Won { get; set; }
        public decimal AmountWon { get; set; }
    }
}
