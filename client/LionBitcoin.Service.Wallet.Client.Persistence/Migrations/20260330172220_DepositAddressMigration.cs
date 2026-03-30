using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LionBitcoin.Service.Wallet.Client.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DepositAddressMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "deposit_address",
                table: "wallets",
                type: "character varying(62)",
                maxLength: 62,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deposit_address",
                table: "wallets");
        }
    }
}
