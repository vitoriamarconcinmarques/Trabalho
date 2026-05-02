using Microsoft.EntityFrameworkCore;

namespace TarefasAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Models.Usuario> Usuarios => Set<Models.Usuario>();
    public DbSet<Models.Categoria> Categorias => Set<Models.Categoria>();
    public DbSet<Models.Tarefa> Tarefas => Set<Models.Tarefa>();
}