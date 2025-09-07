
using Contracts;
using MessageBus;

namespace Pagamentos.Services;

public class PagamentoService : IHostedService
{
    private readonly IMessageBus _messageBus;

    public PagamentoService(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return _messageBus.SubscribeAsync("pagamentos", async message =>
        {
            if (message is CarregamentoIniciado command)
            {
                if (command.Valor > 100)
                {
                    var pagamentoRecusado = new PagamentoRecusado
                    {
                        Id = command.Id,
                        Usuario = command.Usuario,
                        Motivo = "Valor muito alto"
                    };
                    await _messageBus.PublishAsync(pagamentoRecusado);
                    Console.WriteLine($"Pagamento recusado para o usuário {pagamentoRecusado.Usuario} pelo motivo: {pagamentoRecusado.Motivo}");
                }
                else
                {
                    var pagamentoAutorizado = new PagamentoAutorizado
                    {
                        Id = command.Id,
                        Usuario = command.Usuario
                    };
                    await _messageBus.PublishAsync(pagamentoAutorizado);
                    Console.WriteLine($"Pagamento autorizado para o usuário {pagamentoAutorizado.Usuario}");
                }
            }
        });
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
