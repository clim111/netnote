using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NetNote.Migrations
{
    public partial class NotePassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Attachment",
                table: "Notes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Notes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "NoteModel",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attachment",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "NoteModel");
        }
    }
}
