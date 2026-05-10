using CatalogoProdutos.Controllers;
using CatalogoProdutos.DTOs;
using CatalogoProdutos.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CatalogoProdutos.Tests.Controllers;

public class ProdutosControllerTests
{
    private readonly Mock<IProdutoService> _serviceMock;
    private readonly ProdutosController _controller;

    public ProdutosControllerTests()
    {
        _serviceMock = new Mock<IProdutoService>();
        _controller = new ProdutosController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetAll_DeveRetornar200_ComListaDeProdutos()
    {
        // Arrange
        var produtos = new List<ProdutoResponseDto>
        {
            new() { Id = 1, Nome = "Teclado", Preco = 200m, Estoque = 5 },
            new() { Id = 2, Nome = "Mouse", Preco = 100m, Estoque = 10 }
        };
        _serviceMock
            .Setup(s => s.GetAllAsync(1, 10))
            .ReturnsAsync(produtos);

        // Act
        var result = await _controller.GetAll(1, 10);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var retorno = okResult.Value.Should().BeAssignableTo<IEnumerable<ProdutoResponseDto>>().Subject;
        retorno.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetById_DeveRetornar200_QuandoProdutoExiste()
    {
        // Arrange
        var produto = new ProdutoResponseDto { Id = 1, Nome = "Teclado", Preco = 200m, Estoque = 5 };
        _serviceMock
            .Setup(s => s.GetByIdAsync(1))
            .ReturnsAsync(produto);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var retorno = okResult.Value.Should().BeOfType<ProdutoResponseDto>().Subject;
        retorno.Nome.Should().Be("Teclado");
    }

    [Fact]
    public async Task GetById_DeveRetornar404_QuandoProdutoNaoExiste()
    {
        // Arrange
        _serviceMock
            .Setup(s => s.GetByIdAsync(999))
            .ReturnsAsync((ProdutoResponseDto?)null);

        // Act
        var result = await _controller.GetById(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Create_DeveRetornar201_QuandoDadosValidos()
    {
        // Arrange
        var dto = new ProdutoCreateDto { Nome = "Headset", Preco = 300m, Estoque = 3 };
        var criado = new ProdutoResponseDto { Id = 1, Nome = "Headset", Preco = 300m, Estoque = 3 };
        _serviceMock
            .Setup(s => s.CreateAsync(dto))
            .ReturnsAsync(criado);

        // Act
        var result = await _controller.Create(dto);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        var retorno = createdResult.Value.Should().BeOfType<ProdutoResponseDto>().Subject;
        retorno.Id.Should().Be(1);
    }

    [Fact]
    public async Task Update_DeveRetornar200_QuandoProdutoExiste()
    {
        // Arrange
        var dto = new ProdutoUpdateDto { Nome = "Headset Pro", Preco = 500m, Estoque = 2 };
        var atualizado = new ProdutoResponseDto { Id = 1, Nome = "Headset Pro", Preco = 500m, Estoque = 2 };
        _serviceMock
            .Setup(s => s.UpdateAsync(1, dto))
            .ReturnsAsync(atualizado);

        // Act
        var result = await _controller.Update(1, dto);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var retorno = okResult.Value.Should().BeOfType<ProdutoResponseDto>().Subject;
        retorno.Nome.Should().Be("Headset Pro");
    }

    [Fact]
    public async Task Update_DeveRetornar404_QuandoProdutoNaoExiste()
    {
        // Arrange
        var dto = new ProdutoUpdateDto { Nome = "Qualquer", Preco = 10m, Estoque = 1 };
        _serviceMock
            .Setup(s => s.UpdateAsync(999, dto))
            .ReturnsAsync((ProdutoResponseDto?)null);

        // Act
        var result = await _controller.Update(999, dto);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Delete_DeveRetornar204_QuandoProdutoExiste()
    {
        // Arrange
        _serviceMock
            .Setup(s => s.DeleteAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_DeveRetornar404_QuandoProdutoNaoExiste()
    {
        // Arrange
        _serviceMock
            .Setup(s => s.DeleteAsync(999))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}