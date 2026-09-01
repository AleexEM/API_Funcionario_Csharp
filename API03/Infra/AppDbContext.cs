using API03.Models;
using Microsoft.EntityFrameworkCore;

namespace API03.Infra;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Setor> Setores => Set<Setor>();
    public DbSet<Funcionario> Funcionarios => Set<Funcionario>();
    public DbSet<Endereco> Enderecos => Set<Endereco>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Setor>()
            .HasMany(s => s.Funcionarios)
            .WithOne(s => s.Setor)
            .HasForeignKey(f => f.SetorId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Funcionario>()
            .HasMany(s => s.Enderecos)
            .WithOne(e => e.Funcionario)
            .HasForeignKey(e => e.FuncionarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}