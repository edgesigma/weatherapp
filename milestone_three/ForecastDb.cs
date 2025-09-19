using Microsoft.EntityFrameworkCore;

class ForecastDb : DbContext
{
    public ForecastDb(DbContextOptions<ForecastDb> options)
        : base(options) { }

    public DbSet<Forecast> Forecasts => Set<Forecast>();
}