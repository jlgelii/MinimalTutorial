var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


var movielist = new List<Movie>()
{
    new Movie { Id = 1, Name = "Spiderman", Category = "Action" },
    new Movie { Id = 2, Name = "John wick", Category = "Action" },
    new Movie { Id = 3, Name = "IT", Category = "Horror" },
};


// Get all movies
app.MapGet("movie", () =>
{
    return Results.Ok(movielist);
});

// Create new Movie
app.MapPost("movie", (Movie movie) =>
{
    movielist.Add(movie);
    return Results.Ok(movielist);
});

// Update a movie
app.MapPut("movie/{id}", (Movie toUpdateMovie, int id) =>
{
    var movie = movielist.Find(m => m.Id == id);

    if (movie is null)
        return Results.NotFound("Movie does not found.");

    movie.Name = toUpdateMovie.Name;
    movie.Category = toUpdateMovie.Category;

    return Results.Ok(movielist);
});

// Delete a movie
app.MapDelete("movie/{id}", (int id) =>
{
    var movie = movielist.Find(m => m.Id == id);

    if (movie is null)
        return Results.NotFound("Movie does not found.");

    movielist.Remove(movie);

    return Results.Ok(movielist);
});

app.Run();


class Movie
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
}