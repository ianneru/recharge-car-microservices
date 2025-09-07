
namespace Contracts;

public class PagamentoRecusado
{
    public Guid Id { get; set; }
    public string? Usuario { get; set; }
    public string? Motivo { get; set; }
}
