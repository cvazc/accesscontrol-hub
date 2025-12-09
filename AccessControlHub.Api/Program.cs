using AccessControlHub.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

var users = new List<User>()
{
    new User { Id = 1, Name = "John Doe", Email = "john@example.com" },
    new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com" }
};

app.MapGet("/users", () =>
{
    return users;
});


app.MapGet("/users{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
});

app.MapPost("/users", (User newUser) =>
{
    newUser.Id = users.Max(u => u.Id) + 1;
    users.Add(newUser);
    return Results.Created($"/users/{newUser.Id}", newUser);
});

app.MapPut("/users{id}", (int id, User  updatedUser) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if(user is null) return Results.NotFound();

    user.Name = updatedUser.Name;
    user.Email = updatedUser.Email;

    return Results.Ok(user);
});

app.MapDelete("/users{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if(user is null) return Results.NotFound();

    users.Remove(user);
    return Results.Ok();
});

app.Run();
