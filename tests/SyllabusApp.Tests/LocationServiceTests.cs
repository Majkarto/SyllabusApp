using Microsoft.EntityFrameworkCore;
using SyllabusApp.Application.Dtos.Locations;
using SyllabusApp.Infrastructure.Persistence;
using SyllabusApp.Infrastructure.Services;

namespace SyllabusApp.Tests;

public class LocationServiceTests
{
    // Kazdy test dostaje wlasna, pusta baze w pamieci
    private static SyllabusDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<SyllabusDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new SyllabusDbContext(options);
    }

    private static CreateLocationDto SampleDto(string name = "Budynek A") => new()
    {
        Name = name,
        Address = "ul. Kilinskiego 109",
        City = "Lodz",
        PostalCode = "90-011",
        BuildingCode = "A"
    };

    [Fact]
    public async Task CreateAsync_SavesLocation_AndReturnsDtoWithId()
    {
        using var db = CreateDb();
        var service = new LocationService(db);

        var result = await service.CreateAsync(SampleDto());

        Assert.True(result.Id > 0);
        Assert.Equal("Budynek A", result.Name);
        Assert.Equal(1, await db.Locations.CountAsync());
    }

    [Fact]
    public async Task GetAllAsync_ReturnsLocationsSortedByName()
    {
        using var db = CreateDb();
        var service = new LocationService(db);
        await service.CreateAsync(SampleDto("Zeta"));
        await service.CreateAsync(SampleDto("Alfa"));

        var result = (await service.GetAllAsync()).ToList();

        Assert.Equal(["Alfa", "Zeta"], result.Select(l => l.Name));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        using var db = CreateDb();
        var service = new LocationService(db);

        Assert.Null(await service.GetByIdAsync(999));
    }

    [Fact]
    public async Task UpdateAsync_ChangesData_WhenLocationExists()
    {
        using var db = CreateDb();
        var service = new LocationService(db);
        var created = await service.CreateAsync(SampleDto());

        var ok = await service.UpdateAsync(created.Id, new UpdateLocationDto
        {
            Name = "Budynek B",
            Address = "ul. Sienkiewicza 9",
            City = "Lodz",
            PostalCode = "90-113"
        });

        Assert.True(ok);
        var updated = await service.GetByIdAsync(created.Id);
        Assert.Equal("Budynek B", updated!.Name);
        Assert.Null(updated.BuildingCode);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFalse_WhenNotFound()
    {
        using var db = CreateDb();
        var service = new LocationService(db);

        var ok = await service.UpdateAsync(999, new UpdateLocationDto { Name = "X" });

        Assert.False(ok);
    }

    [Fact]
    public async Task DeleteAsync_RemovesLocation()
    {
        using var db = CreateDb();
        var service = new LocationService(db);
        var created = await service.CreateAsync(SampleDto());

        Assert.True(await service.DeleteAsync(created.Id));
        Assert.False(await service.DeleteAsync(created.Id));
        Assert.Equal(0, await db.Locations.CountAsync());
    }
}
