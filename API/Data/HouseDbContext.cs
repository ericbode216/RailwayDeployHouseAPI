using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

public class HouseDbContext : DbContext
{
    public HouseDbContext(DbContextOptions<HouseDbContext> o) :
        base(o){}
    public DbSet<HouseEntity> Houses => Set<HouseEntity>();
    public DbSet<BidEntity> BidEntities => Set<BidEntity>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);

        optionsBuilder
            .UseSqlite($"Data Source={Path.Join(path, "houses.db")}");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        SeedData.Seed(builder);
    }
}