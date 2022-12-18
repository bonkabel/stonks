using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stonks.Migrations
{
    public partial class reports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportedMessageReplies",
                columns: table => new
                {
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReplyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportedMessageReplies", x => x.ReportId);
                });

            migrationBuilder.CreateTable(
                name: "ReportedReplyPosts",
                columns: table => new
                {
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReplyPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportedReplyPosts", x => x.ReportId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportedMessageReplies");

            migrationBuilder.DropTable(
                name: "ReportedReplyPosts");
        }
    }
}
