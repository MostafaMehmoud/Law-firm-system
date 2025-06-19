using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Law.DAL.Migrations
{
    /// <inheritdoc />
    public partial class courtsession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "courtsSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    CourtSessionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IssueFileId = table.Column<int>(type: "int", nullable: false),
                    IssueNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssueName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateNow = table.Column<DateOnly>(type: "date", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ClientProperty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartyId = table.Column<int>(type: "int", nullable: false),
                    PartyPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartyAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartyWorkPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssueTypeId = table.Column<int>(type: "int", nullable: false),
                    IssueValueFees = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IssueDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssueDegreeNegotiation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourtId = table.Column<int>(type: "int", nullable: false),
                    ClaimNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearOfIssue = table.Column<int>(type: "int", nullable: true),
                    CenterId = table.Column<int>(type: "int", nullable: false),
                    RuleOfIssue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateNextSession = table.Column<DateOnly>(type: "date", nullable: true),
                    WhatHappenedInTheCourtSession = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssueImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courtsSessions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_courtsSessions_Code",
                table: "courtsSessions",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "courtsSessions");
        }
    }
}
