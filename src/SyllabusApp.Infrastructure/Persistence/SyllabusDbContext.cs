using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SyllabusApp.Domain.Entities;

namespace SyllabusApp.Infrastructure.Persistence;

public class SyllabusDbContext : IdentityDbContext<User, Role, int>
{
    public SyllabusDbContext(DbContextOptions<SyllabusDbContext> options)
        : base(options)
    {
    }
    public DbSet<Faculty> Faculties => Set<Faculty>();
    public DbSet<FieldOfStudy> FieldsOfStudy => Set<FieldOfStudy>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Syllabus> Syllabuses => Set<Syllabus>();
    public DbSet<LearningOutcome> LearningOutcomes => Set<LearningOutcome>();
    public DbSet<Literature> Literature => Set<Literature>();
    public DbSet<WorkloadItem> WorkloadItems => Set<WorkloadItem>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<StudentGroup> StudentGroups => Set<StudentGroup>();
    public DbSet<SyllabusVersion> SyllabusVersions => Set<SyllabusVersion>();
    public DbSet<SyllabusApproval> SyllabusApprovals => Set<SyllabusApproval>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Syllabus>()
            .HasOne(s => s.Lecturer)
            .WithMany(u => u.SyllabusesAsLecturer)
            .HasForeignKey(s => s.LecturerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Syllabus>()
            .HasOne(s => s.Coordinator)
            .WithMany(u => u.SyllabusesAsCoordinator)
            .HasForeignKey(s => s.CoordinatorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Faculty>()
            .HasOne(f => f.Dean)
            .WithMany()
            .HasForeignKey(f => f.DeanId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Faculty)
            .WithMany(f => f.Employees)
            .HasForeignKey(u => u.FacultyId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<User>().Ignore(u => u.FullName);


        modelBuilder.Entity<Faculty>()
            .HasIndex(f => f.Code)
            .IsUnique();

        modelBuilder.Entity<FieldOfStudy>()
            .HasIndex(fs => fs.Code)
            .IsUnique();

        modelBuilder.Entity<Course>()
            .HasIndex(c => c.Code);

        modelBuilder.Entity<StudentGroup>()
            .HasIndex(g => g.Name)
            .IsUnique();

        modelBuilder.Entity<SyllabusVersion>()
            .HasOne(v => v.CreatedBy)
            .WithMany()
            .HasForeignKey(v => v.CreatedById)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<SyllabusVersion>()
            .HasOne(v => v.Syllabus)
            .WithMany(s => s.Versions)
            .HasForeignKey(v => v.SyllabusId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SyllabusApproval>()
            .HasOne(a => a.PerformedBy)
            .WithMany()
            .HasForeignKey(a => a.PerformedById)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<SyllabusApproval>()
            .HasOne(a => a.Syllabus)
            .WithMany(s => s.Approvals)
            .HasForeignKey(a => a.SyllabusId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SyllabusApproval>()
            .HasOne(a => a.SyllabusVersion)
            .WithMany()
            .HasForeignKey(a => a.SyllabusVersionId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<SyllabusVersion>()
            .HasIndex(v => new { v.SyllabusId, v.VersionNumber })
            .IsUnique();
    }
}