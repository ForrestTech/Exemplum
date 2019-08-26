using Microsoft.EntityFrameworkCore.Migrations;

namespace qanda.Migrations
{
    public partial class Update_Question : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Users_QuestionerId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_QuestionerId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Questions");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Questions",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QuestionerId",
                table: "Questions",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuestionContent",
                table: "Questions",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionerId",
                table: "Questions",
                column: "QuestionerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Users_QuestionerId",
                table: "Questions",
                column: "QuestionerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Users_QuestionerId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_QuestionerId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "QuestionContent",
                table: "Questions");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Questions",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<int>(
                name: "QuestionerId",
                table: "Questions",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Questions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionerId",
                table: "Questions",
                column: "QuestionerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Users_QuestionerId",
                table: "Questions",
                column: "QuestionerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
