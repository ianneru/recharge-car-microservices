
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Carregamento.Controllers;

[ApiController]
[Route("[controller]")]
public class CarregamentoController : ControllerBase
{
    private readonly IMessageBus _messageBus;

    public CarregamentoController(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    [HttpPost]
    public async Task<IActionResult> IniciarCarregamento([FromBody] CarregamentoIniciado command)
    {
        command.Id = Guid.NewGuid();
        await _messageBus.PublishAsync(command);
        Console.WriteLine($"Carregamento iniciado para o usuário {command.Usuario} com o valor de {command.Valor}");

        return Ok(command);
    }
}
