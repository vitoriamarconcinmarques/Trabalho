using Microsoft.EntityFrameworkCore;
using TarefasAPI.Data;
using TarefasAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=tarefas.db"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// ─── USUARIOS ───
app.MapGet("/usuarios", async (AppDbContext db) =>
    await db.Usuarios.ToListAsync());

app.MapGet("/usuarios/{id}", async (int id, AppDbContext db) =>
    await db.Usuarios.FindAsync(id) is Usuario u ? Results.Ok(u) : Results.NotFound());

app.MapPost("/usuarios", async (Usuario usuario, AppDbContext db) =>
{
    db.Usuarios.Add(usuario);
    await db.SaveChangesAsync();
    return Results.Created($"/usuarios/{usuario.Id}", usuario);
});

app.MapPut("/usuarios/{id}", async (int id, Usuario input, AppDbContext db) =>
{
    var usuario = await db.Usuarios.FindAsync(id);
    if (usuario is null) return Results.NotFound();
    usuario.Nome = input.Nome;
    usuario.Email = input.Email;
    await db.SaveChangesAsync();
    return Results.Ok(usuario);
});

app.MapDelete("/usuarios/{id}", async (int id, AppDbContext db) =>
{
    var usuario = await db.Usuarios.FindAsync(id);
    if (usuario is null) return Results.NotFound();
    db.Usuarios.Remove(usuario);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// ─── CATEGORIAS ───
app.MapGet("/categorias", async (AppDbContext db) =>
    await db.Categorias.ToListAsync());

app.MapGet("/categorias/{id}", async (int id, AppDbContext db) =>
    await db.Categorias.FindAsync(id) is Categoria c ? Results.Ok(c) : Results.NotFound());

app.MapPost("/categorias", async (Categoria categoria, AppDbContext db) =>
{
    db.Categorias.Add(categoria);
    await db.SaveChangesAsync();
    return Results.Created($"/categorias/{categoria.Id}", categoria);
});

app.MapPut("/categorias/{id}", async (int id, Categoria input, AppDbContext db) =>
{
    var categoria = await db.Categorias.FindAsync(id);
    if (categoria is null) return Results.NotFound();
    categoria.Nome = input.Nome;
    categoria.Descricao = input.Descricao;
    await db.SaveChangesAsync();
    return Results.Ok(categoria);
});

app.MapDelete("/categorias/{id}", async (int id, AppDbContext db) =>
{
    var categoria = await db.Categorias.FindAsync(id);
    if (categoria is null) return Results.NotFound();
    db.Categorias.Remove(categoria);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// ─── TAREFAS ───
app.MapGet("/tarefas", async (AppDbContext db) =>
    await db.Tarefas.Include(t => t.Usuario).Include(t => t.Categoria).ToListAsync());

app.MapGet("/tarefas/{id}", async (int id, AppDbContext db) =>
    await db.Tarefas.Include(t => t.Usuario).Include(t => t.Categoria)
        .FirstOrDefaultAsync(t => t.Id == id) is Tarefa t ? Results.Ok(t) : Results.NotFound());

app.MapPost("/tarefas", async (Tarefa tarefa, AppDbContext db) =>
{
    db.Tarefas.Add(tarefa);
    await db.SaveChangesAsync();
    return Results.Created($"/tarefas/{tarefa.Id}", tarefa);
});

app.MapPut("/tarefas/{id}", async (int id, Tarefa input, AppDbContext db) =>
{
    var tarefa = await db.Tarefas.FindAsync(id);
    if (tarefa is null) return Results.NotFound();
    tarefa.Titulo = input.Titulo;
    tarefa.Descricao = input.Descricao;
    tarefa.Concluida = input.Concluida;
    tarefa.UsuarioId = input.UsuarioId;
    tarefa.CategoriaId = input.CategoriaId;
    await db.SaveChangesAsync();
    return Results.Ok(tarefa);
});

app.MapDelete("/tarefas/{id}", async (int id, AppDbContext db) =>
{
    var tarefa = await db.Tarefas.FindAsync(id);
    if (tarefa is null) return Results.NotFound();
    db.Tarefas.Remove(tarefa);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();