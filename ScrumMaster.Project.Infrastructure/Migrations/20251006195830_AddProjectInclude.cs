using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScrumMaster.Project.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectInclude : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserAccesses_ProjectId",
                table: "ProjectUserAccesses",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUserAccesses_Projects_ProjectId",
                table: "ProjectUserAccesses",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUserAccesses_Projects_ProjectId",
                table: "ProjectUserAccesses");

            migrationBuilder.DropIndex(
                name: "IX_ProjectUserAccesses_ProjectId",
                table: "ProjectUserAccesses");
        }
    }
}
