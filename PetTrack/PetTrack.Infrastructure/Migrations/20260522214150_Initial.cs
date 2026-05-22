using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetTrack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_CLINICA",
                columns: table => new
                {
                    ID_CLINICA = table.Column<long>(type: "NUMBER(19)", nullable: false, defaultValueSql: "SEQ_CLINICA.NEXTVAL"),
                    NM_CLINICA = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    NR_CNPJ = table.Column<string>(type: "NVARCHAR2(18)", maxLength: 18, nullable: false),
                    DS_EMAIL = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    NR_TELEFONE = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    DS_ENDERECO = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    DT_CADASTRO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "SYSDATE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CLINICA", x => x.ID_CLINICA);
                });

            migrationBuilder.CreateTable(
                name: "TB_TUTOR",
                columns: table => new
                {
                    ID_TUTOR = table.Column<long>(type: "NUMBER(19)", nullable: false, defaultValueSql: "SEQ_TUTOR.NEXTVAL"),
                    NM_TUTOR = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    DS_EMAIL = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    NR_TELEFONE = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    DS_ENDERECO = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    DT_CADASTRO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "SYSDATE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_TUTOR", x => x.ID_TUTOR);
                });

            migrationBuilder.CreateTable(
                name: "TB_PET",
                columns: table => new
                {
                    ID_PET = table.Column<long>(type: "NUMBER(19)", nullable: false, defaultValueSql: "SEQ_PET.NEXTVAL"),
                    NM_PET = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DS_ESPECIE = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    DS_RACA = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    DS_SEXO = table.Column<string>(type: "NVARCHAR2(1)", maxLength: 1, nullable: false),
                    NR_IDADE_ANOS = table.Column<decimal>(type: "NUMBER(3,1)", nullable: false),
                    NR_PESO_KG = table.Column<decimal>(type: "NUMBER(6,3)", nullable: false),
                    DT_CADASTRO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "SYSDATE"),
                    ID_TUTOR = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ID_CLINICA = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PET", x => x.ID_PET);
                    table.ForeignKey(
                        name: "FK_TB_PET_TB_CLINICA_ID_CLINICA",
                        column: x => x.ID_CLINICA,
                        principalTable: "TB_CLINICA",
                        principalColumn: "ID_CLINICA",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_PET_TB_TUTOR_ID_TUTOR",
                        column: x => x.ID_TUTOR,
                        principalTable: "TB_TUTOR",
                        principalColumn: "ID_TUTOR",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_ALERTA",
                columns: table => new
                {
                    ID_ALERTA = table.Column<long>(type: "NUMBER(19)", nullable: false, defaultValueSql: "SEQ_ALERTA.NEXTVAL"),
                    TP_ALERTA = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    DS_DESCRICAO = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    NR_VALOR_REF = table.Column<decimal>(type: "NUMBER(8,2)", nullable: true),
                    DT_ALERTA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "SYSDATE"),
                    ST_RESOLVIDO = table.Column<string>(type: "NVARCHAR2(1)", maxLength: 1, nullable: false),
                    ID_PET = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ALERTA", x => x.ID_ALERTA);
                    table.ForeignKey(
                        name: "FK_TB_ALERTA_TB_PET_ID_PET",
                        column: x => x.ID_PET,
                        principalTable: "TB_PET",
                        principalColumn: "ID_PET",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_BCS_HISTORICO",
                columns: table => new
                {
                    ID_BCS = table.Column<long>(type: "NUMBER(19)", nullable: false, defaultValueSql: "SEQ_BCS_HIST.NEXTVAL"),
                    NR_BCS = table.Column<byte>(type: "NUMBER(2)", nullable: true),
                    DS_FOTO_URL = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    DS_OBSERVACAO = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    DT_ANALISE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "SYSDATE"),
                    ID_PET = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_BCS_HISTORICO", x => x.ID_BCS);
                    table.ForeignKey(
                        name: "FK_TB_BCS_HISTORICO_TB_PET_ID_PET",
                        column: x => x.ID_PET,
                        principalTable: "TB_PET",
                        principalColumn: "ID_PET",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_COLLAR_LEITURA",
                columns: table => new
                {
                    ID_LEITURA = table.Column<long>(type: "NUMBER(19)", nullable: false, defaultValueSql: "SEQ_COLLAR.NEXTVAL"),
                    NR_TEMPERATURA = table.Column<decimal>(type: "NUMBER(5,2)", nullable: false),
                    NR_ATIVIDADE = table.Column<decimal>(type: "NUMBER(6,2)", nullable: true),
                    DT_LEITURA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "SYSDATE"),
                    DS_TOPICO_MQTT = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    ID_PET = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_COLLAR_LEITURA", x => x.ID_LEITURA);
                    table.ForeignKey(
                        name: "FK_TB_COLLAR_LEITURA_TB_PET_ID_PET",
                        column: x => x.ID_PET,
                        principalTable: "TB_PET",
                        principalColumn: "ID_PET",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_EVENTO_CLINICO",
                columns: table => new
                {
                    ID_EVENTO = table.Column<long>(type: "NUMBER(19)", nullable: false, defaultValueSql: "SEQ_EVENTO_CLINICO.NEXTVAL"),
                    TP_EVENTO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    DT_EVENTO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DS_DIAGNOSTICO = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    DS_OBSERVACAO = table.Column<string>(type: "NVARCHAR2(2000)", maxLength: 2000, nullable: true),
                    ID_PET = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ID_CLINICA = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_EVENTO_CLINICO", x => x.ID_EVENTO);
                    table.ForeignKey(
                        name: "FK_TB_EVENTO_CLINICO_TB_CLINICA_ID_CLINICA",
                        column: x => x.ID_CLINICA,
                        principalTable: "TB_CLINICA",
                        principalColumn: "ID_CLINICA",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_EVENTO_CLINICO_TB_PET_ID_PET",
                        column: x => x.ID_PET,
                        principalTable: "TB_PET",
                        principalColumn: "ID_PET",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_NOTIFICACAO",
                columns: table => new
                {
                    ID_NOTIFICACAO = table.Column<long>(type: "NUMBER(19)", nullable: false, defaultValueSql: "SEQ_NOTIFICACAO.NEXTVAL"),
                    TP_NOTIFICACAO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    DS_TITULO = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    DS_MENSAGEM = table.Column<string>(type: "NVARCHAR2(2000)", maxLength: 2000, nullable: true),
                    DT_ENVIO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "SYSDATE"),
                    ST_LIDA = table.Column<string>(type: "NVARCHAR2(1)", maxLength: 1, nullable: false),
                    ID_TUTOR = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ID_PET = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_NOTIFICACAO", x => x.ID_NOTIFICACAO);
                    table.ForeignKey(
                        name: "FK_TB_NOTIFICACAO_TB_PET_ID_PET",
                        column: x => x.ID_PET,
                        principalTable: "TB_PET",
                        principalColumn: "ID_PET",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_NOTIFICACAO_TB_TUTOR_ID_TUTOR",
                        column: x => x.ID_TUTOR,
                        principalTable: "TB_TUTOR",
                        principalColumn: "ID_TUTOR",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_PROTOCOLO_PREVENTIVO",
                columns: table => new
                {
                    ID_PROTOCOLO = table.Column<long>(type: "NUMBER(19)", nullable: false, defaultValueSql: "SEQ_PROTOCOLO.NEXTVAL"),
                    TP_PROTOCOLO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    NM_PROTOCOLO = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    DT_APLICACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DT_PROXIMA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ST_STATUS = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    ID_PET = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PROTOCOLO_PREVENTIVO", x => x.ID_PROTOCOLO);
                    table.ForeignKey(
                        name: "FK_TB_PROTOCOLO_PREVENTIVO_TB_PET_ID_PET",
                        column: x => x.ID_PET,
                        principalTable: "TB_PET",
                        principalColumn: "ID_PET",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_SCORE_HISTORICO",
                columns: table => new
                {
                    ID_SCORE = table.Column<long>(type: "NUMBER(19)", nullable: false, defaultValueSql: "SEQ_SCORE_HIST.NEXTVAL"),
                    NR_SCORE = table.Column<decimal>(type: "NUMBER(5,2)", nullable: false),
                    DT_REGISTRO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "SYSDATE"),
                    DS_OBSERVACAO = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    ID_PET = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_SCORE_HISTORICO", x => x.ID_SCORE);
                    table.ForeignKey(
                        name: "FK_TB_SCORE_HISTORICO_TB_PET_ID_PET",
                        column: x => x.ID_PET,
                        principalTable: "TB_PET",
                        principalColumn: "ID_PET",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_MEDICAMENTO",
                columns: table => new
                {
                    ID_MEDICAMENTO = table.Column<long>(type: "NUMBER(19)", nullable: false, defaultValueSql: "SEQ_MEDICAMENTO.NEXTVAL"),
                    NM_MEDICAMENTO = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    DS_DOSAGEM = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DS_FREQUENCIA = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DT_INICIO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DT_FIM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ID_EVENTO = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_MEDICAMENTO", x => x.ID_MEDICAMENTO);
                    table.ForeignKey(
                        name: "FK_TB_MEDICAMENTO_TB_EVENTO_CLINICO_ID_EVENTO",
                        column: x => x.ID_EVENTO,
                        principalTable: "TB_EVENTO_CLINICO",
                        principalColumn: "ID_EVENTO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_ADESAO_MEDICAMENTO",
                columns: table => new
                {
                    ID_ADESAO = table.Column<long>(type: "NUMBER(19)", nullable: false, defaultValueSql: "SEQ_ADESAO.NEXTVAL"),
                    DT_DOSE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    ST_TOMOU = table.Column<string>(type: "NVARCHAR2(1)", maxLength: 1, nullable: false),
                    DS_OBSERVACAO = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    ID_MEDICAMENTO = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ADESAO_MEDICAMENTO", x => x.ID_ADESAO);
                    table.ForeignKey(
                        name: "FK_TB_ADESAO_MEDICAMENTO_TB_MEDICAMENTO_ID_MEDICAMENTO",
                        column: x => x.ID_MEDICAMENTO,
                        principalTable: "TB_MEDICAMENTO",
                        principalColumn: "ID_MEDICAMENTO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_ADESAO_MEDICAMENTO_ID_MEDICAMENTO",
                table: "TB_ADESAO_MEDICAMENTO",
                column: "ID_MEDICAMENTO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ALERTA_ID_PET",
                table: "TB_ALERTA",
                column: "ID_PET");

            migrationBuilder.CreateIndex(
                name: "IX_TB_BCS_HISTORICO_ID_PET",
                table: "TB_BCS_HISTORICO",
                column: "ID_PET");

            migrationBuilder.CreateIndex(
                name: "TB_CLINICA_CNPJ_UN",
                table: "TB_CLINICA",
                column: "NR_CNPJ",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_COLLAR_LEITURA_ID_PET",
                table: "TB_COLLAR_LEITURA",
                column: "ID_PET");

            migrationBuilder.CreateIndex(
                name: "IX_TB_EVENTO_CLINICO_ID_CLINICA",
                table: "TB_EVENTO_CLINICO",
                column: "ID_CLINICA");

            migrationBuilder.CreateIndex(
                name: "IX_TB_EVENTO_CLINICO_ID_PET",
                table: "TB_EVENTO_CLINICO",
                column: "ID_PET");

            migrationBuilder.CreateIndex(
                name: "IX_TB_MEDICAMENTO_ID_EVENTO",
                table: "TB_MEDICAMENTO",
                column: "ID_EVENTO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_NOTIFICACAO_ID_PET",
                table: "TB_NOTIFICACAO",
                column: "ID_PET");

            migrationBuilder.CreateIndex(
                name: "IX_TB_NOTIFICACAO_ID_TUTOR",
                table: "TB_NOTIFICACAO",
                column: "ID_TUTOR");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PET_ID_CLINICA",
                table: "TB_PET",
                column: "ID_CLINICA");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PET_ID_TUTOR",
                table: "TB_PET",
                column: "ID_TUTOR");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PROTOCOLO_PREVENTIVO_ID_PET",
                table: "TB_PROTOCOLO_PREVENTIVO",
                column: "ID_PET");

            migrationBuilder.CreateIndex(
                name: "IX_TB_SCORE_HISTORICO_ID_PET",
                table: "TB_SCORE_HISTORICO",
                column: "ID_PET");

            migrationBuilder.CreateIndex(
                name: "TB_TUTOR_EMAIL_UN",
                table: "TB_TUTOR",
                column: "DS_EMAIL",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_ADESAO_MEDICAMENTO");

            migrationBuilder.DropTable(
                name: "TB_ALERTA");

            migrationBuilder.DropTable(
                name: "TB_BCS_HISTORICO");

            migrationBuilder.DropTable(
                name: "TB_COLLAR_LEITURA");

            migrationBuilder.DropTable(
                name: "TB_NOTIFICACAO");

            migrationBuilder.DropTable(
                name: "TB_PROTOCOLO_PREVENTIVO");

            migrationBuilder.DropTable(
                name: "TB_SCORE_HISTORICO");

            migrationBuilder.DropTable(
                name: "TB_MEDICAMENTO");

            migrationBuilder.DropTable(
                name: "TB_EVENTO_CLINICO");

            migrationBuilder.DropTable(
                name: "TB_PET");

            migrationBuilder.DropTable(
                name: "TB_CLINICA");

            migrationBuilder.DropTable(
                name: "TB_TUTOR");
        }
    }
}
