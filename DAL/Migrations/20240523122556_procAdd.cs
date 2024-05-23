using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class procAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"CREATE PROCEDURE GetNotesById
                                @Id int
                                AS
                                BEGIN
                                    Select * from Notes
	                                where Id = @Id
                                END
                                Return";
            migrationBuilder.Sql(procedure);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"Drop PROCEDURE GetNotesById
                                Return";
            migrationBuilder.Sql(procedure);

        }
    }
}
