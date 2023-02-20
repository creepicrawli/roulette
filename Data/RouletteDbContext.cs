using Microsoft.EntityFrameworkCore;
using roulette.Models;

public class RouletteDbContext : DbContext
{
    public DbSet<Bet> Bets { get; set; }
    public DbSet<RouletteSpin> RouletteSpins { get; set; }

    public RouletteDbContext(DbContextOptions<RouletteDbContext> options)
        : base(options)
    {
    }
}
