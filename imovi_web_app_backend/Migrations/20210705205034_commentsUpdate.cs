using Microsoft.EntityFrameworkCore.Migrations;

namespace imovi_web_app_backend.Migrations
{
    public partial class commentsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CommentReplies_CommentId",
                table: "CommentReplies",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReplies_Comments_CommentId",
                table: "CommentReplies",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentReplies_Comments_CommentId",
                table: "CommentReplies");

            migrationBuilder.DropIndex(
                name: "IX_CommentReplies_CommentId",
                table: "CommentReplies");
        }
    }
}
