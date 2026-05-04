using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SyllabusApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVersioningAndApprovals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SyllabusVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VersionNumber = table.Column<int>(type: "integer", nullable: false),
                    ChangeDescription = table.Column<string>(type: "text", nullable: false),
                    AcademicYear = table.Column<string>(type: "text", nullable: false),
                    EctsPoints = table.Column<int>(type: "integer", nullable: false),
                    LectureHours = table.Column<int>(type: "integer", nullable: false),
                    ExerciseHours = table.Column<int>(type: "integer", nullable: false),
                    LaboratoryHours = table.Column<int>(type: "integer", nullable: false),
                    CourseObjectives = table.Column<string>(type: "text", nullable: false),
                    AssessmentMethod = table.Column<string>(type: "text", nullable: false),
                    Prerequisites = table.Column<string>(type: "text", nullable: false),
                    SnapshotJson = table.Column<string>(type: "text", nullable: false),
                    SyllabusId = table.Column<int>(type: "integer", nullable: false),
                    CreatedById = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyllabusVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SyllabusVersions_Syllabuses_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "Syllabuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SyllabusVersions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SyllabusApprovals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Action = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    ActionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SyllabusId = table.Column<int>(type: "integer", nullable: false),
                    SyllabusVersionId = table.Column<int>(type: "integer", nullable: true),
                    PerformedById = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyllabusApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SyllabusApprovals_SyllabusVersions_SyllabusVersionId",
                        column: x => x.SyllabusVersionId,
                        principalTable: "SyllabusVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SyllabusApprovals_Syllabuses_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "Syllabuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SyllabusApprovals_Users_PerformedById",
                        column: x => x.PerformedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SyllabusApprovals_PerformedById",
                table: "SyllabusApprovals",
                column: "PerformedById");

            migrationBuilder.CreateIndex(
                name: "IX_SyllabusApprovals_SyllabusId",
                table: "SyllabusApprovals",
                column: "SyllabusId");

            migrationBuilder.CreateIndex(
                name: "IX_SyllabusApprovals_SyllabusVersionId",
                table: "SyllabusApprovals",
                column: "SyllabusVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_SyllabusVersions_CreatedById",
                table: "SyllabusVersions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SyllabusVersions_SyllabusId_VersionNumber",
                table: "SyllabusVersions",
                columns: new[] { "SyllabusId", "VersionNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SyllabusApprovals");

            migrationBuilder.DropTable(
                name: "SyllabusVersions");
        }
    }
}
