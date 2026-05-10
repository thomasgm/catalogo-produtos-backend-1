using CatalogoProdutos.Data;
using CatalogoProdutos.DTOs;
using CatalogoProdutos.Models;
using CatalogoProdutos.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CatalogoProdutos.Tests.Services;

public class ProdutoServiceTests
{
    // Cria um banco em memória novo pra cada teste
    private AppDbContext CriarContexto()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_DeveCriarProduto_QuandoDadosValidos()
    {
        // Arrange (prepara)
        var context = CriarContexto();
        var service = new ProdutoService(context);
        var dto = new ProdutoCreateDto
        {
            Nome = "Teclado Mecânico",
            Descricao = "Teclado gamer",
            Preco = 299.99m,
            Estoque = 10
        };

        // Act (executa)
        var resultado = await service.CreateAsync(dto);

        // Assert (verifica)
        resultado.Should().NotBeNull();
        resultado.Id.Should().BeGreaterThan(0);
        resultado.Nome.Should().Be("Teclado Mecânico");
        resultado.Preco.Should().Be(299.99m);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarProduto_QuandoExiste()
    {
        // Arrange
        var context = CriarContexto();
        var service = new ProdutoService(context);
        context.Produtos.Add(new Produto
        {
            Nome = "Mouse Gamer",
            Preco = 150.00m,
            Estoque = 5
        });
        await context.SaveChangesAsync();

        // Act
        var resultado = await service.GetByIdAsync(1);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Nome.Should().Be("Mouse Gamer");
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarNull_QuandoNaoExiste()
    {
        // Arrange
        var context = CriarContexto();
        var service = new ProdutoService(context);

        // Act
        var resultado = await service.GetByIdAsync(999);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarProdutosPaginados()
    {
        // Arrange
        var context = CriarContexto();
        var service = new ProdutoService(context);
        context.Produtos.AddRange(
            new Produto { Nome = "Produto 1", Preco = 10m, Estoque = 1 },
            new Produto { Nome = "Produto 2", Preco = 20m, Estoque = 2 },
            new Produto { Nome = "Produto 3", Preco = 30m, Estoque = 3 }
        );
        await context.SaveChangesAsync();

        // Act
        var resultado = await service.GetAllAsync(page: 1, pageSize: 2);

        // Assert
        resultado.Should().HaveCount(2);
    }

    [Fact]
    public async Task UpdateAsync_DeveAtualizarProduto_QuandoExiste()
    {
        // Arrange
        var context = CriarContexto();
        var service = new ProdutoService(context);
        context.Produtos.Add(new Produto
        {
            Nome = "Headset Velho",
            Preco = 100m,
            Estoque = 3
        });
        await context.SaveChangesAsync();

        var dto = new ProdutoUpdateDto
        {
            Nome = "Headset Novo",
            Preco = 200m,
            Estoque = 5
        };

        // Act
        var resultado = await service.UpdateAsync(1, dto);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Nome.Should().Be("Headset Novo");
        resultado.Preco.Should().Be(200m);
    }

    [Fact]
    public async Task UpdateAsync_DeveRetornarNull_QuandoNaoExiste()
    {
        // Arrange
        var context = CriarContexto();
        var service = new ProdutoService(context);

        var dto = new ProdutoUpdateDto
        {
            Nome = "Qualquer",
            Preco = 10m,
            Estoque = 1
        };

        // Act
        var resultado = await service.UpdateAsync(999, dto);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_DeveRetornarTrue_QuandoExiste()
    {
        // Arrange
        var context = CriarContexto();
        var service = new ProdutoService(context);
        context.Produtos.Add(new Produto
        {
            Nome = "Produto Deletar",
            Preco = 50m,
            Estoque = 1
        });
        await context.SaveChangesAsync();

        // Act
        var resultado = await service.DeleteAsync(1);

        // Assert
        resultado.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_DeveRetornarFalse_QuandoNaoExiste()
    {
        // Arrange
        var context = CriarContexto();
        var service = new ProdutoService(context);

        // Act
        var resultado = await service.DeleteAsync(999);

        // Assert
        resultado.Should().BeFalse();
    }

    [Fact]
    public async Task SearchByNameAsync_DeveRetornarProdutos_QuandoNomeCorresponde()
    {
        // Arrange
        var context = CriarContexto();
        var service = new ProdutoService(context);
        context.Produtos.AddRange(
            new Produto { Nome = "Teclado Mecânico", Preco = 300m, Estoque = 5 },
            new Produto { Nome = "Teclado Comum", Preco = 50m, Estoque = 10 },
            new Produto { Nome = "Mouse Gamer", Preco = 150m, Estoque = 8 }
        );
        await context.SaveChangesAsync();

        // Act
        var resultado = await service.SearchByNameAsync("Teclado");

        // Assert
        resultado.Should().HaveCount(2);
    }
}