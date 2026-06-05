using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VH_Burguer.Applications.Services;
using VH_Burguer.DTOs.CategoriaDto;
using VH_Burguer.Exceptions;
using VH_Burguer.Interfaces;

namespace VHBurguer.Tests.Services
{
    public class CategoriaServiceTests
    {
        [Fact] // O Fact marca um método como sendo um teste unitário, é um atributo do xUnit
        public void Adicionar_DeveGerarErro_QuandoEstiverVazio()
        {
            // Cria um objeto falso (mock) do repositório
            // Simula o comportamento do repositório de categorias durante o teste
            // Não acessa o banco de dados
            Mock<ICategoriaRepository> repositoryMock = new Mock<ICategoriaRepository>();

            CategoriaService service = new CategoriaService(repositoryMock.Object); // Instanciando o serviço passando o mock (objeto falso) do repositório

            // Cria a DTO com um nome vazio para gerar erro
            CriarCategoriaDto categoriaDto = new CriarCategoriaDto 
            {
                Nome = ""
            };

            // Define a ação que será executada durante o teste 
            Action acao = () => service.Adicionar(categoriaDto);

            // Traz o que a ação deveria retornar
            acao.Should()
                .Throw<DomainException>() // Retornar uma DomainException
                .WithMessage("Nome é obrigatório."); // Com essa mensagem
        }

        [Fact]
        public void Adicionar_DeveGerarErro_QuandoCategoriaJaExistir()
        {
            Mock<ICategoriaRepository> repositoryMock = new Mock<ICategoriaRepository>();

            // .Setup -> Configura o comportamento do método mock
            // Nesse caso está configurando para retornar true quando o método NomeExiste for chamado com o nome "Lanche"
            // Quando o método NomeExiste for chamado com "Lanche" retorna true, simulando que a categoria já existe
            repositoryMock
                .Setup(categoria => categoria.NomeExiste("Lanche", It.IsAny<int?>())) //  It.IsAny<int?>() -> pode usar qualquer valor para o id 
                .Returns(true);

            CategoriaService service = new CategoriaService(repositoryMock.Object);

            CriarCategoriaDto categoriaDto = new CriarCategoriaDto
            {
                Nome = "Lanche"
            };

            Action acao = () => service.Adicionar(categoriaDto);

            
            acao.Should()
                .Throw<DomainException>()
                .WithMessage("Categoria já existe.");
        }
    }
}
