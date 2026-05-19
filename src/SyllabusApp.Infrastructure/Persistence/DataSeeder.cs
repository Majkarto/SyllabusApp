using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SyllabusApp.Domain.Constants;
using SyllabusApp.Domain.Entities;

namespace SyllabusApp.Infrastructure.Persistence;

/// <summary>
/// Wypełnia bazę początkowymi danymi: rolami i kontem admina.
/// Idempotentny - można wywołać wielokrotnie, nie zduplikuje danych.
/// </summary>
public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        // Tworzymy scope - RoleManager i UserManager są Scoped,
        // a my jesteśmy w trakcie startupu (poza request scope), więc musimy
        // ręcznie zażądać scope'a
        using var scope = services.CreateScope();
        var sp = scope.ServiceProvider;

        var roleManager = sp.GetRequiredService<RoleManager<Role>>();
        var userManager = sp.GetRequiredService<UserManager<User>>();
        var configuration = sp.GetRequiredService<IConfiguration>();
        var logger = sp.GetRequiredService<ILogger<object>>();

        await SeedRolesAsync(roleManager, logger);
        await SeedDefaultAdminAsync(userManager, configuration, logger);
    }

    private static async Task SeedRolesAsync(RoleManager<Role> roleManager, ILogger logger)
    {
        // Opis ról - pole Description w naszej encji Role
        var roleDescriptions = new Dictionary<string, string>
        {
            { UserRoles.Admin, "Pełne uprawnienia administracyjne - zarządzanie systemem i rolami." },
            { UserRoles.Dean, "Dziekan - zatwierdzanie sylabusów na swoim wydziale." },
            { UserRoles.Lecturer, "Wykładowca - tworzenie i edycja sylabusów." },
            { UserRoles.Student, "Student - przeglądanie sylabusów." },
        };

        foreach (var roleName in UserRoles.All)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var role = new Role
                {
                    Name = roleName,
                    Description = roleDescriptions.GetValueOrDefault(roleName)
                };

                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    logger.LogInformation("Utworzono rolę: {RoleName}", roleName);
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    logger.LogError("Nie udało się utworzyć roli {RoleName}: {Errors}", roleName, errors);
                }
            }
        }
    }

    private static async Task SeedDefaultAdminAsync(
        UserManager<User> userManager,
        IConfiguration configuration,
        ILogger logger)
    {
        var adminEmail = configuration["DefaultAdmin:Email"];
        var adminPassword = configuration["DefaultAdmin:Password"];
        var adminFirstName = configuration["DefaultAdmin:FirstName"] ?? "Default";
        var adminLastName = configuration["DefaultAdmin:LastName"] ?? "Administrator";

        // Walidacja - bez tych danych admin nie zostanie utworzony, ale aplikacja
        // wystartuje normalnie (np. w testach na bazie in-memory nie chcemy admina)
        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
        {
            logger.LogWarning("DefaultAdmin nie skonfigurowany w User Secrets/appsettings - pomijam tworzenie admina.");
            return;
        }

        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
        if (existingAdmin != null)
        {
            logger.LogInformation("Admin {Email} już istnieje - pomijam.", adminEmail);
            return;
        }

        var admin = new User
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            FirstName = adminFirstName,
            LastName = adminLastName,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        var result = await userManager.CreateAsync(admin, adminPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            logger.LogError("Nie udało się utworzyć admina: {Errors}", errors);
            return;
        }

        var roleResult = await userManager.AddToRoleAsync(admin, UserRoles.Admin);
        if (!roleResult.Succeeded)
        {
            var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            logger.LogError("Nie udało się nadać roli Admin: {Errors}", errors);
            return;
        }

        logger.LogInformation("Utworzono domyślnego admina: {Email}", adminEmail);
    }
}