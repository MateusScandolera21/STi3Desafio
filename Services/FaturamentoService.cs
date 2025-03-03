using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VendasAPI.Models;

namespace VendasAPI.Services
{
    public class FaturamentoService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FaturamentoService> _logger;

        public FaturamentoService(HttpClient httpClient, ILogger<FaturamentoService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<bool> EnviarParaFaturamento(Venda venda, decimal subtotal, decimal descontos)
        {
            try
            {
                // Preparar o corpo da requisição
                var requestBody = new
                {
                    identificador = venda.Identificador,
                    subTotal   = Math.Round(subtotal, 2), // 2 casas decimais
                    descontos  = Math.Round(descontos, 2), // 2 casas decimais
                    valorTotal = Math.Round(venda.ValorTotal, 2), // 2 casas decimais
                    itens = venda.Itens.Select(i => new
                    {
                        quantidade = i.Quantidade,
                        precoUnitario = Math.Round(i.PrecoUnitario, 2) // 2 casas decimais
                    }).ToList()
                };

                // Serializar o corpo para JSON
                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Adicionar cabeçalho com o e-mail
                _httpClient.DefaultRequestHeaders.Clear(); // Limpa cabeçalhos anteriores
                _httpClient.DefaultRequestHeaders.Add("email", "mateus.scandolera@gmail.com");

                // Enviar a requisição
                _logger.LogInformation("Enviando requisição para o serviço de faturamento...");
                var response = await _httpClient.PostAsync("https://sti3-faturamento.azurewebsites.net/api/vendas", content);

                // Verificar se a requisição foi bem-sucedida
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Faturamento processado com sucesso.");
                    return true;
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Erro ao enviar para o faturamento: {response.StatusCode}. Resposta: {responseContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção ao enviar para o faturamento: {ex.Message}");
                return false;
            }
        }
    }
}