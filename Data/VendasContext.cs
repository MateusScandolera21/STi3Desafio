using Microsoft.EntityFrameworkCore;
using VendasAPI.Models;

namespace VendasAPI.Data
{
    public class VendasContext : DbContext
    {
        public VendasContext(DbContextOptions<VendasContext> options) : base(options) { }

        public DbSet<Venda> Vendas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ItemVenda> ItensVenda { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Venda>()
                .HasKey(v => v.VendaId); // Define a chave primária para Venda
            modelBuilder.Entity<ItemVenda>()
                .HasKey(i => i.ItemVendaId); // Define a chave primária para ItemVenda
        }
    }
}