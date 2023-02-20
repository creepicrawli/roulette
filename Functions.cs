namespace roulette
{
    public class Functions
    {
        //Implement the Spin function to generate a random number/color pair as in the casino game roulette.
        private static Random _random = new Random();

        public static (int, string) Spin()
        {
            // Generate a random number between 0 and 36
            int number = _random.Next(37);

            // Determine the color of the number
            var color = number == 0 ? "green" :
                number % 2 == 1 ? "red" : "black";

            // Return the number and color pair
            return (number, color);
        }

        //Implement the Payout function to determine if the user wins and generate an accurate payout amount based on the casino game roulette odds.
        public static double Payout(string betType, int betAmount, (int, string) spinResult)
        {
            int number = spinResult.Item1;
            string color = spinResult.Item2;

            switch (betType)
            {
                case "black":
                    return color == "black" ? betAmount * 2 : 0;
                case "red":
                    return color == "red" ? betAmount * 2 : 0;
                case "even":
                    return number % 2 == 0 ? betAmount * 2 : 0;
                case "odd":
                    return number % 2 == 1 ? betAmount * 2 : 0;
                default: // number bet
                    if (number.ToString() == betType)
                    {
                        return betAmount * 36;
                    }
                    else
                    {
                        return 0;
                    }
            }
        }
    }
}
