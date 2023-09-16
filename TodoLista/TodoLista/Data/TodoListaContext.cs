using TodoLista.Models;
using Microsoft.EntityFrameworkCore;

namespace TodoLista.Data
{
    public class TodoListaContext:DbContext
    {
        public TodoListaContext(DbContextOptions<TodoListaContext> options) : base(options) { }
        public DbSet<Kategorija> Kategorija { get; set;}
        public DbSet<Korisnik> Korisnik { get; set;}
        public DbSet<Todo_Lista>Todo_lista { get; set;}
        public DbSet<Zadatak>Zadatak { get; set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo_Lista>().HasOne(t => t.Korisnik);
            modelBuilder.Entity<Zadatak>().HasOne(z => z.Todo_Lista);
            modelBuilder.Entity<Zadatak>().HasOne(z => z.Kategorija);
        }
    }
}
