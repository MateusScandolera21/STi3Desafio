using Microsoft.AspNetCore.Mvc;
using VendasAPI.Models;
using VendasAPI.Services;

namespace VendasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendasController : ControllerBase
    {
        private readonly VendaService _vendaService;

        public VendasController(VendaService vendaService)
        {
            _vendaService = vendaService;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessarVenda([FromBody] Venda venda)
        {
            await _vendaService.ProcessarVenda(venda);
            return Ok(venda);
        }

        [HttpGet]
        public IActionResult ListarVendas()
        {
            var vendas = _vendaService.ListarVendas();
            return Ok(vendas);
        }
    }
}