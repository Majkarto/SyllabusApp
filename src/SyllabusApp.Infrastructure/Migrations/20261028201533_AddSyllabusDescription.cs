using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyllabusApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSyllabusDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Syllabuses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Syllabuses");
        }
    }
}
