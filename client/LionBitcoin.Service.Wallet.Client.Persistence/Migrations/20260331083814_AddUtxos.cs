using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LionBitcoin.Service.Wallet.Client.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUtxos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Utxos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionId = table.Column<byte[]>(type: "bytea", maxLength: 32, nullable: false),
                    OutputIndex = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    BlockHeight = table.Column<long>(type: "bigint", nullable: false),
                    WalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utxos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Utxos_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_utxos_created_at_index",
                table: "Utxos",
                column: "CreatedAt")
                .Annotation("Npgsql:IndexMethod", "brin");

            migrationBuilder.CreateIndex(
                name: "IX_Utxos_WalletId",
                table: "Utxos",
                column: "WalletId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Utxos");
        }
    }
}
