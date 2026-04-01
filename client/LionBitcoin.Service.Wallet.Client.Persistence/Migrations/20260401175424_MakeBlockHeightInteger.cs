using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LionBitcoin.Service.Wallet.Client.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeBlockHeightInteger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BlockHeight",
                table: "Utxos",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "BlockHeight",
                table: "Utxos",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
