using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONLINEVS_Project.Migrations
{
    /// <inheritdoc />
    public partial class InitialTab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Elections",
                columns: table => new
                {
                    ElectionID = table.Column<int>(name: "Election-ID", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(name: "Start-Date", type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(name: "End-Date", type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elections", x => x.ElectionID);
                });

            migrationBuilder.CreateTable(
                name: "Voters",
                columns: table => new
                {
                    VoterID = table.Column<int>(name: "Voter-ID", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoterCNIC = table.Column<string>(name: "Voter-CNIC", type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Contact_number = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voters", x => x.VoterID);
                });

            migrationBuilder.CreateTable(
                name: "Votings",
                columns: table => new
                {
                    Vote_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoteCastedatTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votings", x => x.Vote_Id);
                });

            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    Candidate_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Candidate_CNIC = table.Column<int>(type: "int", maxLength: 13, nullable: false),
                    CandidateName = table.Column<string>(name: "Candidate-Name", type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Party = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ElectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.Candidate_ID);
                    table.ForeignKey(
                        name: "FK_Candidates_Elections_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Elections",
                        principalColumn: "Election-ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_ElectionId",
                table: "Candidates",
                column: "ElectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "Voters");

            migrationBuilder.DropTable(
                name: "Votings");

            migrationBuilder.DropTable(
                name: "Elections");
        }
    }
}
