using System;

namespace VendasAPI.Models
{
    public class Cliente
    {
        public Guid ClienteId { get; set; }
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public string? Categoria { get; set; }
    }
}