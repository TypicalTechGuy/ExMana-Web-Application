using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUDTest.Migrations
{
    /// <inheritdoc />
    public partial class ForceDatetimeColumnToDatetime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Alter the 'date' column in the 'UserTransactions' table
            migrationBuilder.AlterColumn<DateTime>(
                name: "date",
                table: "UserTransactions",
                type: "datetime", // Explicitly set datetime
                nullable: false);

            // Removed table creation of TransactionCategories, Users and kept only the alteration of date column.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // If you need to revert the change, add the Down() method here
            // However, since we removed the table creation from Up(), we should
            // not have any table drops here.
            // If you had to revert the date column, you would add the revert code here.
            // example : migrationBuilder.AlterColumn<DateTime>(name: "date", table: "UserTransactions", type: "datetime2", nullable: false);
        }
    }
}