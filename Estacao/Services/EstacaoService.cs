
using Contracts;
using MessageBus;

namespace Estacao.Services;

public class EstacaoService : IHostedService
{
    private readonly IMessageBus _messageBus;

    public EstacaoService(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return _messageBus.SubscribeAsync("estacao", async message =>
        {
            if (message is PagamentoAutorizado command)
            {
                Console.WriteLine($"Iniciando carregamento para o usuário {command.Usuario}");

                // Simula o tempo de carregamento
                await Task.Delay(5000, cancellationToken);

                var carregamentoFinalizado = new CarregamentoFinalizado
                {
                    Id = command.Id,
                    Usuario = command.Usuario,
                    ValorCobrado = new Random().Next(10, 50) // Valor aleatório para simulação
                };

                await _messageBus.PublishAsync(carregamentoFinalizado);
                Console.WriteLine($"Carregamento finalizado para o usuário {carregamentoFinalizado.Usuario}. Valor cobrado: {carregamentoFinalizado.ValorCobrado}");
            }
        });
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
