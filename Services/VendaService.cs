using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendasAPI.Data;
using VendasAPI.Models;

namespace VendasAPI.Services
{
    public class VendaService
    {
        private readonly VendasContext _context;
        private readonly FaturamentoService _faturamentoService;

        public VendaService(VendasContext context, FaturamentoService faturamentoService)
        {
            _context = context;
            _faturamentoService = faturamentoService;
        }

        public decimal CalcularDesconto(string categoria, decimal valorTotal)
        {
            return categoria switch
            {
                "REGULAR" => valorTotal > 500 ? valorTotal * 0.95m : valorTotal,
                "PREMIUM" => valorTotal > 300 ? valorTotal * 0.90m : valorTotal,
                "VIP" => valorTotal * 0.85m,
                _ => valorTotal
            };
        }

        public async Task ProcessarVenda(Venda venda)
        {
            // Verificar se a venda é nula
            if (venda == null)
                throw new ArgumentNullException(nameof(venda));

            // Verificar se os itens da venda são nulos ou vazios
            if (venda.Itens == null || !venda.Itens.Any())
                throw new ArgumentException("A venda deve conter pelo menos um item.");

            // Verificar se o cliente é nulo
            if (venda.Cliente == null)
                throw new ArgumentException("A venda deve estar associada a um cliente.");

            // Calcular o subtotal (soma dos itens sem desconto)
            var subtotal = venda.Itens.Sum(i => i.Quantidade * i.PrecoUnitario);

            // Calcular o valor total com desconto
            venda.ValorTotal = Math.Round(CalcularDesconto(venda.Cliente.Categoria, subtotal), 2); // 2 casas decimais

            // Calcular os descontos
            var descontos = Math.Round(subtotal - venda.ValorTotal, 2); // 2 casas decimais

            // Salvar a venda no banco de dados
            _context.Vendas.Add(venda);
            await _context.SaveChangesAsync();

            // Enviar para o serviço de faturamento
            bool faturamentoSucesso = await _faturamentoService.EnviarParaFaturamento(venda, subtotal, descontos);

            // Atualizar o status da venda
            if (faturamentoSucesso)
            {
                venda.Status = "CONCLUIDO";
                await _context.SaveChangesAsync();
            }
        }

        // Método para listar vendas
        public List<Venda> ListarVendas()
        {
            return _context.Vendas
                .Include(v => v.Cliente) // Inclui os dados do cliente
                .Include(v => v.Itens)  // Inclui os itens da venda
                .ToList();
        }
    }
}