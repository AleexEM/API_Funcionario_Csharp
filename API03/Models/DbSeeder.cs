using API03.Infra;

namespace API03.Models;





public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        // Se ja tem dado, nao popula de novo
        if (context.Setores.Any()) return;
 
        var ti = new Setor { Nome = "Tecnologia da Informacao" };
        var rh = new Setor { Nome = "Recursos Humanos" };
        var financeiro = new Setor { Nome = "Financeiro" };
 
        context.Setores.AddRange(ti, rh, financeiro);
        context.SaveChanges(); // aqui os Ids dos setores sao gerados
 
        var funcionarios = new List<Funcionario>
        {
            new Funcionario
            {
                Nome = "Ana Souza",
                Salario = 7800,
                SetorId = ti.Id,
                Enderecos =
                {
                    new Endereco
                    {
                        Pais = "Brasil", Estado = "SC", Cidade = "Florianopolis",
                        Bairro = "Trindade", Rua = "Rua Lauro Linhares",
                        Numero = 120, Cep = "88036-002"
                    },
                    new Endereco
                    {
                        Pais = "Brasil", Estado = "SC", Cidade = "Florianopolis",
                        Bairro = "Campeche", Rua = "Servidao das Palmeiras",
                        Numero = 45, Cep = "88063-000"
                    }
                }
            },
            new Funcionario
            {
                Nome = "Bruno Martins",
                Salario = 5200,
                SetorId = ti.Id,
                Enderecos =
                {
                    new Endereco
                    {
                        Pais = "Brasil", Estado = "SC", Cidade = "Blumenau",
                        Bairro = "Centro", Rua = "Rua XV de Novembro",
                        Numero = 900, Cep = "89010-001"
                    }
                }
            },
            new Funcionario
            {
                Nome = "Carla Ribeiro",
                Salario = 6100,
                SetorId = rh.Id,
                Enderecos =
                {
                    new Endereco
                    {
                        Pais = "Brasil", Estado = "SC", Cidade = "Itajai",
                        Bairro = "Fazenda", Rua = "Avenida Sete de Setembro",
                        Numero = 1450, Cep = "88301-200"
                    }
                }
            },
            new Funcionario
            {
                Nome = "Diego Alves",
                Salario = 4300,
                SetorId = financeiro.Id,
                Enderecos =
                {
                    new Endereco
                    {
                        Pais = "Brasil", Estado = "PR", Cidade = "Curitiba",
                        Bairro = "Batel", Rua = "Avenida do Batel",
                        Numero = 210, Cep = "80420-090"
                    },
                    new Endereco
                    {
                        Pais = "Portugal", Estado = "Lisboa", Cidade = "Lisboa",
                        Bairro = "Alfama", Rua = "Rua dos Remedios",
                        Numero = 78, Cep = "1100-441"
                    }
                }
            },
            new Funcionario
            {
                Nome = "Elisa Fontana",
                Salario = 9500,
                SetorId = ti.Id,
                Enderecos =
                {
                    new Endereco
                    {
                        Pais = "Brasil", Estado = "SC", Cidade = "Balneario Camboriu",
                        Bairro = "Centro", Rua = "Avenida Atlantica",
                        Numero = 3300, Cep = "88330-000"
                    }
                }
            },
            new Funcionario
            {
                // Funcionario sem endereco: bom para mostrar lista vazia na API
                Nome = "Felipe Nunes",
                Salario = 3800,
                SetorId = rh.Id
            }
        };
 
        context.Funcionarios.AddRange(funcionarios);
        context.SaveChanges(); // salva funcionarios e enderecos juntos
    }
}