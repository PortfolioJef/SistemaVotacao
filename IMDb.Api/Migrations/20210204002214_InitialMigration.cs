using Microsoft.EntityFrameworkCore.Migrations;

namespace IMDb.Api.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "movies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 60, nullable: true),
                    Director = table.Column<string>(maxLength: 60, nullable: true),
                    Sinopse = table.Column<string>(maxLength: 1000, nullable: true),
                    Year = table.Column<int>(nullable: false),
                    UrlTrailer = table.Column<string>(maxLength: 60, nullable: true),
                    urlImage = table.Column<string>(nullable: true),
                    isDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ratings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RatingId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    MovieId = table.Column<int>(nullable: false),
                    Rate = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ratings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 60, nullable: true),
                    Lastname = table.Column<string>(maxLength: 60, nullable: true),
                    Email = table.Column<string>(maxLength: 60, nullable: true),
                    Password = table.Column<string>(maxLength: 60, nullable: true),
                    isDeleted = table.Column<bool>(nullable: false),
                    isAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "actors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 60, nullable: true),
                    MovieId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_actors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_actors_movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "movieGenres",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieGenreId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 60, nullable: true),
                    MovieId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movieGenres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_movieGenres_movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "Email", "Lastname", "Name", "Password", "isAdmin", "isDeleted" },
                values: new object[] { 1, "admin@empresa.net", "admin", "admin", "UUVHGRBjB7w=", true, false });

            migrationBuilder.CreateIndex(
                name: "IX_actors_MovieId",
                table: "actors",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_movieGenres_MovieId",
                table: "movieGenres",
                column: "MovieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "actors");

            migrationBuilder.DropTable(
                name: "movieGenres");

            migrationBuilder.DropTable(
                name: "ratings");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "movies");
        }
    }
}
