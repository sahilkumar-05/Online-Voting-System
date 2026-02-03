using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONLINEVS_Project.Migrations
{
    /// <inheritdoc />
    public partial class FixVotingModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "password",
                table: "Voters",
                newName: "Password");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Voters",
                newName: "password");
        }
    }
}
