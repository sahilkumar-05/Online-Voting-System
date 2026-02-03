using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONLINEVS_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddVotingRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VoteCastedatTime",
                table: "Votings",
                newName: "CastAt");

            migrationBuilder.RenameColumn(
                name: "Vote_Id",
                table: "Votings",
                newName: "VoteId");

            migrationBuilder.AddColumn<int>(
                name: "CandidateId",
                table: "Votings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ElectionId",
                table: "Votings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VoterId",
                table: "Votings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Votings_CandidateId",
                table: "Votings",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_Votings_ElectionId",
                table: "Votings",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Votings_VoterId",
                table: "Votings",
                column: "VoterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Votings_Candidates_CandidateId",
                table: "Votings",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "Candidate_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votings_Elections_ElectionId",
                table: "Votings",
                column: "ElectionId",
                principalTable: "Elections",
                principalColumn: "Election_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votings_Voters_VoterId",
                table: "Votings",
                column: "VoterId",
                principalTable: "Voters",
                principalColumn: "VoterID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votings_Candidates_CandidateId",
                table: "Votings");

            migrationBuilder.DropForeignKey(
                name: "FK_Votings_Elections_ElectionId",
                table: "Votings");

            migrationBuilder.DropForeignKey(
                name: "FK_Votings_Voters_VoterId",
                table: "Votings");

            migrationBuilder.DropIndex(
                name: "IX_Votings_CandidateId",
                table: "Votings");

            migrationBuilder.DropIndex(
                name: "IX_Votings_ElectionId",
                table: "Votings");

            migrationBuilder.DropIndex(
                name: "IX_Votings_VoterId",
                table: "Votings");

            migrationBuilder.DropColumn(
                name: "CandidateId",
                table: "Votings");

            migrationBuilder.DropColumn(
                name: "ElectionId",
                table: "Votings");

            migrationBuilder.DropColumn(
                name: "VoterId",
                table: "Votings");

            migrationBuilder.RenameColumn(
                name: "CastAt",
                table: "Votings",
                newName: "VoteCastedatTime");

            migrationBuilder.RenameColumn(
                name: "VoteId",
                table: "Votings",
                newName: "Vote_Id");
        }
    }
}
