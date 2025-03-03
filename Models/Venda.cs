using System;
using System.Collections.Generic;

namespace VendasAPI.Models
{
    public class Venda
    {
        public Guid VendaId { get; set; }
        public Guid Identificador { get; set; }
        public DateTime DataVenda { get; set; }
        public Cliente? Cliente { get; set; }
        public List<ItemVenda>? Itens { get; set; }
        public decimal ValorTotal { get; set; }
        public string Status { get; set; } = "PENDENTE";
    }
}