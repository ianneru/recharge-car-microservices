
# Microsserviços de Recarga de Carro Elétrico

Este projeto demonstra uma arquitetura de microsserviços para um sistema de recarga de carros elétricos, utilizando .NET 9 e comunicação por eventos.

## Visão de Negócio

O produto visa fornecer uma solução de recarga de veículos elétricos (EV) confiável e fácil de usar. O fluxo de negócios principal é o seguinte:

1.  **Início da Recarga**: O motorista do EV inicia uma sessão de recarga em uma de nossas estações.
2.  **Autorização de Pagamento**: O sistema autoriza o pagamento do motorista para a sessão de recarga.
3.  **Recarga da Bateria**: Uma vez que o pagamento é autorizado, a estação de recarga começa a carregar o veículo.
4.  **Notificações em Tempo Real**: O motorista é notificado em tempo real sobre o status de sua sessão de recarga (iniciada, pagamento autorizado/recusado, finalizada).

Este modelo de negócios permite uma experiência de usuário perfeita e garante que os pagamentos sejam processados antes que a energia seja dispensada, minimizando o risco para o operador da estação de recarga.

## Arquitetura

O sistema é composto pelos seguintes microsserviços:

- **Contracts**: Uma biblioteca de classes contendo os eventos compartilhados entre os serviços.
- **Carregamento**: Responsável por iniciar o fluxo de recarga e publicar o evento `CarregamentoIniciado`.
- **Pagamentos**: Ouve o evento `CarregamentoIniciado`, processa o pagamento e publica os eventos `PagamentoAutorizado` ou `PagamentoRecusado`.
- **Estacao**: Ouve o evento `PagamentoAutorizado`, simula o carregamento da bateria e publica o evento `CarregamentoFinalizado`.
- **Notificacoes**: Ouve todos os eventos e envia notificações para o usuário.
- **MessageBus**: Uma implementação simples de um message bus em memória para a comunicação entre os serviços.

## Tecnologias Utilizadas

- **.NET 9
- **ASP.NET Core
- **C# 13**
- **Comunicação Orientada a Eventos**
- **Message Bus em Memória**: Para este exemplo, um simples message bus em memória é usado para facilitar a comunicação entre os serviços. Em um ambiente de produção, isso seria substituído por uma solução de message broker mais robusta, como RabbitMQ ou Azure Service Bus.
- **REST APIs**

## Fluxo de Eventos

1.  `Usuário inicia carregamento` -> O serviço `Carregamento` publica o evento `CarregamentoIniciado`.
2.  O serviço de `Pagamentos` recebe o evento, reserva o valor (pré-autorização) e publica `PagamentoAutorizado` ou `PagamentoRecusado`.
3.  O serviço de `Estacao` recebe `PagamentoAutorizado`, inicia o carregamento real e publica `CarregamentoFinalizado` quando termina.
4.  O serviço de `Notificacoes` reage a todos os eventos e envia notificações para o usuário.

## Como Executar

Para executar a solução, você precisará abrir um terminal para cada um dos seguintes projetos e executar o comando `dotnet run`:

- `Carregamento`
- `Pagamentos`
- `Estacao`
- `Notificacoes`

**Exemplo:**

```bash
dotnet run --project Carregamento
```

## Simulando uma Requisição

Com todos os serviços em execução, você pode enviar uma requisição POST para o serviço de `Carregamento` para iniciar o fluxo.

**Endpoint:** `POST /carregamento`

**Corpo da Requisição:**

```json
{
  "usuario": "John Doe",
  "valor": 50
}
```

**Exemplo com cURL:**

```bash
curl -X POST -H "Content-Type: application/json" -d '{"usuario":"John Doe","valor":50}' http://localhost:5180/Carregamento
```
