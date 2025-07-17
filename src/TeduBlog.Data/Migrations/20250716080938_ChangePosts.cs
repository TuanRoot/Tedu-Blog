using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeduBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangePosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Tags",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "Posts",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AuthorUserName",
                table: "Posts",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Posts",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CategorySlug",
                table: "Posts",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidDate",
                table: "Posts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FromStatus",
                table: "PostActivityLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ToStatus",
                table: "PostActivityLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "PostActivityLogs",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "AuthorUserName",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CategorySlug",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PaidDate",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "FromStatus",
                table: "PostActivityLogs");

            migrationBuilder.DropColumn(
                name: "ToStatus",
                table: "PostActivityLogs");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "PostActivityLogs");
        }
    }
}
