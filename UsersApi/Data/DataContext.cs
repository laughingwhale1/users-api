using Microsoft.EntityFrameworkCore;
using UsersApi.Models;
using UsersApi.Models.Extensions;

public class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    // public static void Initialize(DbContext context)
    // {
    //     context.Database.Migrate();
    //
    //     // could add seed data
    // }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json").Build();
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("SecondaryConnection"));
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     IConfigurationRoot configuration = new ConfigurationBuilder()
    //         .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    //         .AddJsonFile("appsettings.json")
    //         .Build();
    //     base.OnConfiguring(optionsBuilder);
    //     optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
    // }
    //
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        modelBuilder.AddPostgreSqlRules();
    }

    public DbSet<User> users { get; set; }
}