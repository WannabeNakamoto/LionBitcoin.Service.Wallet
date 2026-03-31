using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LionBitcoin.Service.Wallet.Client.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLastSyncTimeInWallet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastSyncedTime",
                table: "Wallets",
                type: "timestamptz",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastSyncedTime",
                table: "Wallets");
        }
    }
}
