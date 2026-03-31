using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LionBitcoin.Service.Wallet.Client.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpperCaseNamingConvention : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_wallets",
                table: "wallets");

            migrationBuilder.RenameTable(
                name: "wallets",
                newName: "Wallets");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Wallets",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Wallets",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "deposit_address",
                table: "Wallets",
                newName: "DepositAddress");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Wallets",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "account_private_key",
                table: "Wallets",
                newName: "AccountPrivateKey");

            migrationBuilder.RenameIndex(
                name: "ix_wallets_created_at",
                table: "Wallets",
                newName: "ix_wallets_created_at_index");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wallets",
                table: "Wallets",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Wallets",
                table: "Wallets");

            migrationBuilder.RenameTable(
                name: "Wallets",
                newName: "wallets");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "wallets",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "wallets",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "DepositAddress",
                table: "wallets",
                newName: "deposit_address");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "wallets",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "AccountPrivateKey",
                table: "wallets",
                newName: "account_private_key");

            migrationBuilder.RenameIndex(
                name: "ix_wallets_created_at_index",
                table: "wallets",
                newName: "ix_wallets_created_at");

            migrationBuilder.AddPrimaryKey(
                name: "pk_wallets",
                table: "wallets",
                column: "id");
        }
    }
}
