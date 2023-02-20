using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using roulette;
using roulette.Models;

namespace roulette.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouletteController : ControllerBase
    {
        private readonly ILogger<RouletteController> _logger;
        private readonly RouletteDbContext _dbContext;

        public RouletteController(ILogger<RouletteController> logger, RouletteDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpPost("PlaceBet")]
        public IActionResult PlaceBet([FromBody] Bet bet)
        {
            try
            {
                // Step 5: Validation
                if (bet.BetType != "black" && bet.BetType != "red" &&
                bet.BetType != "odd" && bet.BetType != "even" &&
                    (int.Parse(bet.BetType) < 0 || int.Parse(bet.BetType) > 36))
                {
                    return BadRequest("Invalid bet type.");
                }
                if (bet.BetAmount <= 0)
                {
                    return BadRequest("Bet amount must be positive.");
                }

                // Step 6: Save the bet to the database
                _dbContext.Bets.Add(bet);
                _dbContext.SaveChanges();

                // Step 7: Generate a spin
                var spin = Functions.Spin();

                // Step 8: Calculate payout
                var payout = Functions.Payout(bet.BetType, bet.BetAmount,spin);

                // Step 9: Build the response
                var response = new BetResponse
                {
                    Successful = true,
                    Spin = spin,
                    Won = spin==Functions.Spin(),
                    AmountWon = (decimal)payout
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error placing bet.");
                return StatusCode(500, "An error occurred while placing the bet.");
            }
        }


        [HttpGet]
        [Route("ShowPreviousSpins")]
        public async Task<ActionResult<List<RouletteSpin>>> ShowPreviousSpins()
        {
            try
            {
                // Retrieve all RouletteSpin records from the database
                var spins = await _dbContext.RouletteSpins.ToListAsync();

                // Return the list of RouletteSpin records
                return Ok(spins);
            }
            catch (Exception ex)
            {
                // Log the error message
                _logger.LogError($"Failed to retrieve previous RouletteSpins: {ex}");

                // Return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve previous RouletteSpins");
            }
        }


    }
}