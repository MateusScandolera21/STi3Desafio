namespace VendasAPI.Models
{
    public class ItemVenda
    {
        public int ItemVendaId { get; set; }
        public int ProdutoId { get; set; }
        public string? Descricao { get; set; }
        public decimal Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }
}