using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONLINEVS_Project.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Voter-CNIC",
                table: "Voters",
                newName: "VoterCNIC");

            migrationBuilder.RenameColumn(
                name: "Voter-ID",
                table: "Voters",
                newName: "VoterID");

            migrationBuilder.RenameColumn(
                name: "Start-Date",
                table: "Elections",
                newName: "Start_Date");

            migrationBuilder.RenameColumn(
                name: "End-Date",
                table: "Elections",
                newName: "End_Date");

            migrationBuilder.RenameColumn(
                name: "Election-ID",
                table: "Elections",
                newName: "Election_ID");

            migrationBuilder.RenameColumn(
                name: "Candidate-Name",
                table: "Candidates",
                newName: "CandidateName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Admin",
                newName: "Cnic");

            migrationBuilder.AlterColumn<string>(
                name: "VoterCNIC",
                table: "Voters",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Candidate_CNIC",
                table: "Candidates",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 13);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VoterCNIC",
                table: "Voters",
                newName: "Voter-CNIC");

            migrationBuilder.RenameColumn(
                name: "VoterID",
                table: "Voters",
                newName: "Voter-ID");

            migrationBuilder.RenameColumn(
                name: "Start_Date",
                table: "Elections",
                newName: "Start-Date");

            migrationBuilder.RenameColumn(
                name: "End_Date",
                table: "Elections",
                newName: "End-Date");

            migrationBuilder.RenameColumn(
                name: "Election_ID",
                table: "Elections",
                newName: "Election-ID");

            migrationBuilder.RenameColumn(
                name: "CandidateName",
                table: "Candidates",
                newName: "Candidate-Name");

            migrationBuilder.RenameColumn(
                name: "Cnic",
                table: "Admin",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Voter-CNIC",
                table: "Voters",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13);

            migrationBuilder.AlterColumn<int>(
                name: "Candidate_CNIC",
                table: "Candidates",
                type: "int",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13);
        }
    }
}
