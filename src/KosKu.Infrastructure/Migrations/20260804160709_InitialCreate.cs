using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KosKu.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tipe_kamar",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    nama_tipe = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    deskripsi = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    harga_per_bulan = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false),
                    fasilitas = table.Column<string>(type: "longtext", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipe_kamar", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    nama = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    no_hp = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    role = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "kamar",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    nomor_kamar = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    lantai = table.Column<int>(type: "int", nullable: false),
                    tipe_kamar_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    foto_url = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kamar", x => x.id);
                    table.ForeignKey(
                        name: "FK_kamar_tipe_kamar_tipe_kamar_id",
                        column: x => x.tipe_kamar_id,
                        principalTable: "tipe_kamar",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "booking",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    kode_booking = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    user_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    kamar_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    tanggal_masuk = table.Column<DateOnly>(type: "date", nullable: false),
                    durasi_bulan = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking", x => x.id);
                    table.ForeignKey(
                        name: "FK_booking_kamar_kamar_id",
                        column: x => x.kamar_id,
                        principalTable: "kamar",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_booking_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "perbaikan_layanan",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    kamar_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    user_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    judul_laporan = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    deskripsi = table.Column<string>(type: "longtext", nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_perbaikan_layanan", x => x.id);
                    table.ForeignKey(
                        name: "FK_perbaikan_layanan_kamar_kamar_id",
                        column: x => x.kamar_id,
                        principalTable: "kamar",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_perbaikan_layanan_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sewa_aktif",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    booking_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    user_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    kamar_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    tanggal_mulai = table.Column<DateOnly>(type: "date", nullable: false),
                    tanggal_selesai = table.Column<DateOnly>(type: "date", nullable: false),
                    jatuh_tempo_berikutnya = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sewa_aktif", x => x.id);
                    table.ForeignKey(
                        name: "FK_sewa_aktif_booking_booking_id",
                        column: x => x.booking_id,
                        principalTable: "booking",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sewa_aktif_kamar_kamar_id",
                        column: x => x.kamar_id,
                        principalTable: "kamar",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sewa_aktif_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "denda",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    sewa_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    keterangan = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    jumlah = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_denda", x => x.id);
                    table.ForeignKey(
                        name: "FK_denda_sewa_aktif_sewa_id",
                        column: x => x.sewa_id,
                        principalTable: "sewa_aktif",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pembayaran",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    sewa_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    user_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    jumlah = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false),
                    tanggal_bayar = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    metode_pembayaran = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    status_pembayaran = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    bukti_pembayaran = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pembayaran", x => x.id);
                    table.ForeignKey(
                        name: "FK_pembayaran_sewa_aktif_sewa_id",
                        column: x => x.sewa_id,
                        principalTable: "sewa_aktif",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pembayaran_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_booking_kamar_id",
                table: "booking",
                column: "kamar_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_kode_booking",
                table: "booking",
                column: "kode_booking",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_booking_user_id",
                table: "booking",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_denda_sewa_id",
                table: "denda",
                column: "sewa_id");

            migrationBuilder.CreateIndex(
                name: "IX_kamar_nomor_kamar",
                table: "kamar",
                column: "nomor_kamar",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_kamar_tipe_kamar_id",
                table: "kamar",
                column: "tipe_kamar_id");

            migrationBuilder.CreateIndex(
                name: "IX_pembayaran_sewa_id",
                table: "pembayaran",
                column: "sewa_id");

            migrationBuilder.CreateIndex(
                name: "IX_pembayaran_user_id",
                table: "pembayaran",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_perbaikan_layanan_kamar_id",
                table: "perbaikan_layanan",
                column: "kamar_id");

            migrationBuilder.CreateIndex(
                name: "IX_perbaikan_layanan_user_id",
                table: "perbaikan_layanan",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_sewa_aktif_booking_id",
                table: "sewa_aktif",
                column: "booking_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sewa_aktif_kamar_id",
                table: "sewa_aktif",
                column: "kamar_id");

            migrationBuilder.CreateIndex(
                name: "IX_sewa_aktif_user_id",
                table: "sewa_aktif",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "denda");

            migrationBuilder.DropTable(
                name: "pembayaran");

            migrationBuilder.DropTable(
                name: "perbaikan_layanan");

            migrationBuilder.DropTable(
                name: "sewa_aktif");

            migrationBuilder.DropTable(
                name: "booking");

            migrationBuilder.DropTable(
                name: "kamar");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "tipe_kamar");
        }
    }
}
