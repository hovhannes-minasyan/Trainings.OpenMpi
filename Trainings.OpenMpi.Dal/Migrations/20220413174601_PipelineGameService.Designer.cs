// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Trainings.OpenMpi.Dal;

#nullable disable

namespace Trainings.OpenMpi.Dal.Migrations
{
    [DbContext(typeof(TrainingMpiDbContext))]
    [Migration("20220413174601_PipelineGameService")]
    partial class PipelineGameService
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "citext");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Trainings.OpenMpi.Dal.Entities.ConcurrencyGame", b =>
                {
                    b.Property<int>("GameId")
                        .HasColumnType("integer")
                        .HasColumnName("game_id");

                    b.Property<long>("CorrectSum")
                        .HasColumnType("bigint")
                        .HasColumnName("correct_sum");

                    b.Property<long>("CurrentSum")
                        .HasColumnType("bigint")
                        .HasColumnName("current_sum");

                    b.HasKey("GameId")
                        .HasName("pk_concurrency_games");

                    b.ToTable("concurrency_games", (string)null);
                });

            modelBuilder.Entity("Trainings.OpenMpi.Dal.Entities.ConcurrencyGameRound", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AddCoeff")
                        .HasColumnType("integer")
                        .HasColumnName("add_coeff");

                    b.Property<int>("ConcurrencyGameId")
                        .HasColumnType("integer")
                        .HasColumnName("concurrency_game_id");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("boolean")
                        .HasColumnName("is_finished");

                    b.Property<int>("OrderIndex")
                        .HasColumnType("integer")
                        .HasColumnName("order_index");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_concurrency_game_rounds");

                    b.HasIndex("ConcurrencyGameId")
                        .HasDatabaseName("ix_concurrency_game_rounds_concurrency_game_id");

                    b.ToTable("concurrency_game_rounds", (string)null);
                });

            modelBuilder.Entity("Trainings.OpenMpi.Dal.Entities.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_games");

                    b.ToTable("games", (string)null);
                });

            modelBuilder.Entity("Trainings.OpenMpi.Dal.Entities.PipelineGame", b =>
                {
                    b.Property<int>("GameId")
                        .HasColumnType("integer")
                        .HasColumnName("game_id");

                    b.Property<int>("PipelineLength")
                        .HasColumnType("integer")
                        .HasColumnName("pipeline_length");

                    b.Property<int>("RoundsLeft")
                        .HasColumnType("integer")
                        .HasColumnName("rounds_left");

                    b.HasKey("GameId")
                        .HasName("pk_pipeline_games");

                    b.ToTable("pipeline_games", (string)null);
                });

            modelBuilder.Entity("Trainings.OpenMpi.Dal.Entities.PipelineStep", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("DataValue")
                        .HasColumnType("numeric")
                        .HasColumnName("data_value");

                    b.Property<int>("Operation")
                        .HasColumnType("integer")
                        .HasColumnName("operation");

                    b.Property<int>("PipelineGameId")
                        .HasColumnType("integer")
                        .HasColumnName("pipeline_game_id");

                    b.Property<int>("QuizQuestionId")
                        .HasColumnType("integer")
                        .HasColumnName("quiz_question_id");

                    b.Property<int>("State")
                        .HasColumnType("integer")
                        .HasColumnName("state");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_pipeline_steps");

                    b.HasIndex("PipelineGameId")
                        .HasDatabaseName("ix_pipeline_steps_pipeline_game_id");

                    b.HasIndex("QuizQuestionId")
                        .HasDatabaseName("ix_pipeline_steps_quiz_question_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_pipeline_steps_user_id");

                    b.ToTable("pipeline_steps", (string)null);
                });

            modelBuilder.Entity("Trainings.OpenMpi.Dal.Entities.QuizQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Question")
                        .HasColumnType("text")
                        .HasColumnName("question");

                    b.Property<double>("Result")
                        .HasColumnType("double precision")
                        .HasColumnName("result");

                    b.HasKey("Id")
                        .HasName("pk_quiz_questions");

                    b.ToTable("quiz_questions", (string)null);
                });

            modelBuilder.Entity("Trainings.OpenMpi.Dal.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<string>("Password")
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("Username")
                        .HasColumnType("citext")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Trainings.OpenMpi.Dal.Entities.ConcurrencyGame", b =>
                {
                    b.HasOne("Trainings.OpenMpi.Dal.Entities.Game", "Game")
                        .WithOne("ConcurrencyGame")
                        .HasForeignKey("Trainings.OpenMpi.Dal.Entities.ConcurrencyGame", "GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_concurrency_games_games_game_id");

                    b.Navigation("Game");
                });

            modelBuilder.Entity("Trainings.OpenMpi.Dal.Entities.ConcurrencyGameRound", b =>
                {
                    b.HasOne("Trainings.OpenMpi.Dal.Entities.ConcurrencyGame", "ConcurrencyGame")
                        .WithMany("ConcurrencyGameRounds")
                        .HasForeignKey("ConcurrencyGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_concurrency_game_rounds_concurrency_games_concurrency_game_");

                    b.Navigation("ConcurrencyGame");
                });

            modelBuilder.Entity("Trainings.OpenMpi.Dal.Entities.PipelineGame", b =>
                {
                    b.HasOne("Trainings.OpenMpi.Dal.Entities.Game", "Game")
                        .WithOne("PipelineGame")
                        .HasForeignKey("Trainings.OpenMpi.Dal.Entities.PipelineGame", "GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_pipeline_games_games_game_id");

                    b.Navigation("Game");
                });

            modelBuilder.Entity("Trainings.OpenMpi.Dal.Entities.PipelineStep", b =>
                {
                    b.HasOne("Trainings.OpenMpi.Dal.Entities.PipelineGame", "PipelineGame")
                        .WithMany("PipelineSteps")
                        .HasForeignKey("PipelineGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_pipeline_steps_pipeline_games_pipeline_game_id");

                    b.HasOne("Trainings.OpenMpi.Dal.Entities.QuizQuestion", "QuizQuestion")
                        .WithMany()
                        .HasForeignKey("QuizQuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_pipeline_steps_quiz_questions_quiz_question_id");

                    b.HasOne("Trainings.OpenMpi.Dal.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_pipeline_steps_users_user_id");

                    b.Navigation("PipelineGame");

                    b.Navigation("QuizQuestion");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Trainings.OpenMpi.Dal.Entities.ConcurrencyGame", b =>
                {
                    b.Navigation("ConcurrencyGameRounds");
                });

            modelBuilder.Entity("Trainings.OpenMpi.Dal.Entities.Game", b =>
                {
                    b.Navigation("ConcurrencyGame");

                    b.Navigation("PipelineGame");
                });

            modelBuilder.Entity("Trainings.OpenMpi.Dal.Entities.PipelineGame", b =>
                {
                    b.Navigation("PipelineSteps");
                });
#pragma warning restore 612, 618
        }
    }
}
