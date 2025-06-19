using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Law.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addcolumnimage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "IssueImage",
                table: "issuesFiles",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssueImage",
                table: "issuesFiles");
        }
    }
}
