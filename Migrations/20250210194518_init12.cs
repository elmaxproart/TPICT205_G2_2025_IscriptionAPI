using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gradeManagerServerAPi.Migrations
{
    /// <inheritdoc />
    public partial class init12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Etudiants_Classes_ClasseId",
                table: "Etudiants");

            migrationBuilder.DropForeignKey(
                name: "FK_Inscriptions_Classes_ClasseId",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "ClasseId",
                table: "Classes");

            migrationBuilder.AddColumn<int>(
                name: "InscriptionId",
                table: "UEs",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClasseId",
                table: "Inscriptions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateNaissance",
                table: "Inscriptions",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Inscriptions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Matricule",
                table: "Inscriptions",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Nom",
                table: "Inscriptions",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Numero",
                table: "Inscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Prenom",
                table: "Inscriptions",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Sexe",
                table: "Inscriptions",
                type: "varchar(1)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UeIds",
                table: "Inscriptions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "semestre",
                table: "Inscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ClasseId",
                table: "Etudiants",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UEs_InscriptionId",
                table: "UEs",
                column: "InscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Etudiants_Classes_ClasseId",
                table: "Etudiants",
                column: "ClasseId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inscriptions_Classes_ClasseId",
                table: "Inscriptions",
                column: "ClasseId",
                principalTable: "Classes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UEs_Inscriptions_InscriptionId",
                table: "UEs",
                column: "InscriptionId",
                principalTable: "Inscriptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Etudiants_Classes_ClasseId",
                table: "Etudiants");

            migrationBuilder.DropForeignKey(
                name: "FK_Inscriptions_Classes_ClasseId",
                table: "Inscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_UEs_Inscriptions_InscriptionId",
                table: "UEs");

            migrationBuilder.DropIndex(
                name: "IX_UEs_InscriptionId",
                table: "UEs");

            migrationBuilder.DropColumn(
                name: "InscriptionId",
                table: "UEs");

            migrationBuilder.DropColumn(
                name: "DateNaissance",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "Matricule",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "Nom",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "Prenom",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "Sexe",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "UeIds",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "semestre",
                table: "Inscriptions");

            migrationBuilder.AlterColumn<int>(
                name: "ClasseId",
                table: "Inscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClasseId",
                table: "Etudiants",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ClasseId",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Etudiants_Classes_ClasseId",
                table: "Etudiants",
                column: "ClasseId",
                principalTable: "Classes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inscriptions_Classes_ClasseId",
                table: "Inscriptions",
                column: "ClasseId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
