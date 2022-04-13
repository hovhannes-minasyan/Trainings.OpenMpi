using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Trainings.OpenMpi.Dal.Migrations
{
    public partial class PipelineGameService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pipeline_games",
                columns: table => new
                {
                    game_id = table.Column<int>(type: "integer", nullable: false),
                    pipeline_length = table.Column<int>(type: "integer", nullable: false),
                    rounds_left = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pipeline_games", x => x.game_id);
                    table.ForeignKey(
                        name: "fk_pipeline_games_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pipeline_steps",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pipeline_game_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    quiz_question_id = table.Column<int>(type: "integer", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    operation = table.Column<int>(type: "integer", nullable: false),
                    data_value = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pipeline_steps", x => x.id);
                    table.ForeignKey(
                        name: "fk_pipeline_steps_pipeline_games_pipeline_game_id",
                        column: x => x.pipeline_game_id,
                        principalTable: "pipeline_games",
                        principalColumn: "game_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_pipeline_steps_quiz_questions_quiz_question_id",
                        column: x => x.quiz_question_id,
                        principalTable: "quiz_questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_pipeline_steps_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_pipeline_steps_pipeline_game_id",
                table: "pipeline_steps",
                column: "pipeline_game_id");

            migrationBuilder.CreateIndex(
                name: "ix_pipeline_steps_quiz_question_id",
                table: "pipeline_steps",
                column: "quiz_question_id");

            migrationBuilder.CreateIndex(
                name: "ix_pipeline_steps_user_id",
                table: "pipeline_steps",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pipeline_steps");

            migrationBuilder.DropTable(
                name: "pipeline_games");
        }
    }
}
