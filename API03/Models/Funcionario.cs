using System.Text.Json.Serialization;

namespace API03.Models;

public class Funcionario
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public double Salario { get; set; }
    
    public int SetorId { get; set; }
    
    
    public Setor? Setor { get; set; }

    public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();

}