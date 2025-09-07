
using Contracts;
using MessageBus;

namespace Notificacoes.Services;

public class NotificacaoService : IHostedService
{
    private readonly IMessageBus _messageBus;

    public NotificacaoService(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return _messageBus.SubscribeAsync("notificacoes", message =>
        {
            switch (message)
            {
                case CarregamentoIniciado ev:
                    Console.WriteLine($"Notificação: Carregamento iniciado para {ev.Usuario}.");
                    break;
                case PagamentoAutorizado ev:
                    Console.WriteLine($"Notificação: Pagamento autorizado para {ev.Usuario}.");
                    break;
                case PagamentoRecusado ev:
                    Console.WriteLine($"Notificação: Pagamento recusado para {ev.Usuario}. Motivo: {ev.Motivo}");
                    break;
                case CarregamentoFinalizado ev:
                    Console.WriteLine($"Notificação: Carregamento finalizado para {ev.Usuario}. Valor: {ev.ValorCobrado}");
                    break;
            }
            return Task.CompletedTask;
        });
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
