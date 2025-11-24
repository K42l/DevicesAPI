
using DevicesApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace DevicesApi.Infrastructure;

public class DeviceDbContext : DbContext
{
    public DeviceDbContext(DbContextOptions<DeviceDbContext> o) : base(o)
    {
    }

    public DbSet<Device> Devices => Set<Device>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(d => d.Id);

            // Convert GUID to string for storage, since SQLite doesn't have a native GUID
            entity.Property(d => d.Id)
                .HasConversion<string>()
                .IsRequired();

            // Convert enum to string for simplicity but, a table lookup should be used here. 
            entity.Property(d => d.State)
                .HasConversion<string>()
                .HasMaxLength(9)
                .IsRequired();

            entity.Property(d => d.Name).IsRequired();
            entity.Property(d => d.Brand).IsRequired();
            entity.Property(d => d.CreatedAt).IsRequired();

            //Using this because DateTimeOffset.UtcNow is dynamic and causes issues with migrations
            //The same apply to the Ids, can't use Guid.NewGuid()
            var createdAt = new DateTimeOffset(new DateTime(2025, 11, 23, 21, 00, 0, 0, DateTimeKind.Unspecified), TimeSpan.Zero);

            entity.HasData(
                new Device
                {
                    Id = Guid.Parse("2fcaefb2-abe6-4a70-b327-bb34f58a66de"),
                    Name = "name 1",
                    Brand = "brand 1",
                    State = DeviceStateEnum.Available,
                    CreatedAt = createdAt
                },
                new Device
                {
                    Id = Guid.Parse("1c39ef8c-f794-481e-8ca9-4ec9a15d0029"),
                    Name = "name 2",
                    Brand = "brand 2",
                    State = DeviceStateEnum.InUse,
                    CreatedAt = createdAt
                },
                new Device
                {
                    Id = Guid.Parse("1dc798b8-febc-4256-b711-3b146203398a"),
                    Name = "name 3",
                    Brand = "brand 3",
                    State = DeviceStateEnum.Inactive,
                    CreatedAt = createdAt
                }
            );
        });
    }
}
