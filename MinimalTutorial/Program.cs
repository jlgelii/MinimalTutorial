using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options
    => options.UseSqlServer("Data Source=localhost;Initial Catalog=MinimalAPI;Integrated Security=True;TrustServerCertificate=True"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



// Get all movies
app.MapGet("movie", (AppDbContext context) =>
{
    var movielist = context.Movie.ToList();
    return Results.Ok(movielist);
});

// Create new Movie
app.MapPost("movie", (Movie movie, AppDbContext context) =>
{
    context.Movie.Add(movie);
    context.SaveChanges();

    var movielist = context.Movie.ToList();

    return Results.Ok(movielist);
});

// Update a movie
app.MapPut("movie/{id}", (Movie toUpdateMovie, int id, AppDbContext context) =>
{
    var movie = context.Movie.SingleOrDefault(m => m.Id == id);

    if (movie is null)
        return Results.NotFound("Movie does not found.");

    movie.Name = toUpdateMovie.Name;
    movie.Category = toUpdateMovie.Category;

    context.SaveChanges();

    var movielist = context.Movie.ToList();

    return Results.Ok(movielist);
});

// Delete a movie
app.MapDelete("movie/{id}", (int id, AppDbContext context) =>
{
    var movie = context.Movie.SingleOrDefault(m => m.Id == id);

    if (movie is null)
        return Results.NotFound("Movie does not found.");

    context.Movie.Remove(movie);
    context.SaveChanges();

    var movielist = context.Movie.ToList();

    return Results.Ok(movielist);
});

app.Run();


class Movie
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
}


class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movie { get; set; }
}