using System.Text.Json.Serialization;

namespace API03.Models;

public class Endereco
{
    public int Id { get; set; }
    public string Pais { get; set; }
    public string Estado { get; set; }
    public string Cidade { get; set; }
    public string Bairro { get; set; }
    public string Rua { get; set; }
    public int Numero { get; set; }
    public string Cep { get; set; }
    
    public int FuncionarioId { get; set; }
    
    [JsonIgnore]
    public Funcionario? Funcionario { get; set; }
}