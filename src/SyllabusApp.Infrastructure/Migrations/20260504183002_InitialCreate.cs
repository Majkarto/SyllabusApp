using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SyllabusApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    PostalCode = table.Column<string>(type: "text", nullable: false),
                    BuildingCode = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Semester = table.Column<int>(type: "integer", nullable: false),
                    FieldOfStudyId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    DeanId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Faculties_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FieldsOfStudy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    DurationSemesters = table.Column<int>(type: "integer", nullable: false),
                    FacultyId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldsOfStudy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldsOfStudy_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CurrentSemester = table.Column<int>(type: "integer", nullable: false),
                    StartYear = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    FieldOfStudyId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentGroups_FieldsOfStudy_FieldOfStudyId",
                        column: x => x.FieldOfStudyId,
                        principalTable: "FieldsOfStudy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    AcademicTitle = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    FacultyId = table.Column<int>(type: "integer", nullable: true),
                    StudentGroupId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_StudentGroups_StudentGroupId",
                        column: x => x.StudentGroupId,
                        principalTable: "StudentGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Syllabuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AcademicYear = table.Column<string>(type: "text", nullable: false),
                    EctsPoints = table.Column<int>(type: "integer", nullable: false),
                    LectureHours = table.Column<int>(type: "integer", nullable: false),
                    ExerciseHours = table.Column<int>(type: "integer", nullable: false),
                    LaboratoryHours = table.Column<int>(type: "integer", nullable: false),
                    CourseObjectives = table.Column<string>(type: "text", nullable: false),
                    AssessmentMethod = table.Column<string>(type: "text", nullable: false),
                    Prerequisites = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CourseId = table.Column<int>(type: "integer", nullable: false),
                    LecturerId = table.Column<int>(type: "integer", nullable: true),
                    CoordinatorId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Syllabuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Syllabuses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Syllabuses_Users_CoordinatorId",
                        column: x => x.CoordinatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Syllabuses_Users_LecturerId",
                        column: x => x.LecturerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LearningOutcomes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    SyllabusId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningOutcomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningOutcomes_Syllabuses_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "Syllabuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Literature",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Author = table.Column<string>(type: "text", nullable: false),
                    Publisher = table.Column<string>(type: "text", nullable: true),
                    PublicationYear = table.Column<int>(type: "integer", nullable: true),
                    Isbn = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    SyllabusId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Literature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Literature_Syllabuses_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "Syllabuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkloadItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Hours = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    SyllabusId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkloadItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkloadItems_Syllabuses_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "Syllabuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Code",
                table: "Courses",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_FieldOfStudyId",
                table: "Courses",
                column: "FieldOfStudyId");

            migrationBuilder.CreateIndex(
                name: "IX_Faculties_Code",
                table: "Faculties",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faculties_DeanId",
                table: "Faculties",
                column: "DeanId");

            migrationBuilder.CreateIndex(
                name: "IX_Faculties_LocationId",
                table: "Faculties",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldsOfStudy_Code",
                table: "FieldsOfStudy",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FieldsOfStudy_FacultyId",
                table: "FieldsOfStudy",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningOutcomes_SyllabusId",
                table: "LearningOutcomes",
                column: "SyllabusId");

            migrationBuilder.CreateIndex(
                name: "IX_Literature_SyllabusId",
                table: "Literature",
                column: "SyllabusId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_FieldOfStudyId",
                table: "StudentGroups",
                column: "FieldOfStudyId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_Name",
                table: "StudentGroups",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Syllabuses_CoordinatorId",
                table: "Syllabuses",
                column: "CoordinatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Syllabuses_CourseId",
                table: "Syllabuses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Syllabuses_LecturerId",
                table: "Syllabuses",
                column: "LecturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FacultyId",
                table: "Users",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_StudentGroupId",
                table: "Users",
                column: "StudentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkloadItems_SyllabusId",
                table: "WorkloadItems",
                column: "SyllabusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_FieldsOfStudy_FieldOfStudyId",
                table: "Courses",
                column: "FieldOfStudyId",
                principalTable: "FieldsOfStudy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Faculties_Users_DeanId",
                table: "Faculties",
                column: "DeanId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentGroups_FieldsOfStudy_FieldOfStudyId",
                table: "StudentGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Faculties_Locations_LocationId",
                table: "Faculties");

            migrationBuilder.DropForeignKey(
                name: "FK_Faculties_Users_DeanId",
                table: "Faculties");

            migrationBuilder.DropTable(
                name: "LearningOutcomes");

            migrationBuilder.DropTable(
                name: "Literature");

            migrationBuilder.DropTable(
                name: "WorkloadItems");

            migrationBuilder.DropTable(
                name: "Syllabuses");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "FieldsOfStudy");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Faculties");

            migrationBuilder.DropTable(
                name: "StudentGroups");
        }
    }
}
