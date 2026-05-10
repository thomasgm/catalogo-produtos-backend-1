using CatalogoProdutos.Data;
using CatalogoProdutos.DTOs;
using CatalogoProdutos.Models;
using CatalogoProdutos.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CatalogoProdutos.Tests.Services;

public class AuthServiceTests
{
    private AppDbContext CriarContexto()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private IConfiguration CriarConfiguration()
    {
        var config = new Dictionary<string, string?>
        {
            { "Jwt:Key", "chave-super-secreta-para-testes-12345678" },
            { "Jwt:Issuer", "CatalogoProdutos" },
            { "Jwt:Audience", "CatalogoProdutosUsers" }
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();
    }

    [Fact]
    public async Task RegisterAsync_DeveRetornarToken_QuandoDadosValidos()
    {
        // Arrange
        var context = CriarContexto();
        var config = CriarConfiguration();
        var service = new AuthService(context, config);

        var dto = new RegisterDto
        {
            Nome = "João Silva",
            Email = "joao@email.com",
            Senha = "senha123"
        };

        // Act
        var token = await service.RegisterAsync(dto);

        // Assert
        token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task RegisterAsync_DeveRetornarNull_QuandoEmailJaCadastrado()
    {
        // Arrange
        var context = CriarContexto();
        var config = CriarConfiguration();
        var service = new AuthService(context, config);

        context.Usuarios.Add(new Usuario
        {
            Nome = "João Silva",
            Email = "joao@email.com",
            SenhaHash = "hashqualquer"
        });
        await context.SaveChangesAsync();

        var dto = new RegisterDto
        {
            Nome = "João Silva",
            Email = "joao@email.com",
            Senha = "senha123"
        };

        // Act
        var token = await service.RegisterAsync(dto);

        // Assert
        token.Should().BeNull();
    }

    [Fact]
    public async Task LoginAsync_DeveRetornarToken_QuandoCredenciaisValidas()
    {
        // Arrange
        var context = CriarContexto();
        var config = CriarConfiguration();
        var service = new AuthService(context, config);

        // Primeiro registra
        await service.RegisterAsync(new RegisterDto
        {
            Nome = "Maria Silva",
            Email = "maria@email.com",
            Senha = "senha123"
        });

        var dto = new LoginDto
        {
            Email = "maria@email.com",
            Senha = "senha123"
        };

        // Act
        var token = await service.LoginAsync(dto);

        // Assert
        token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task LoginAsync_DeveRetornarNull_QuandoEmailNaoExiste()
    {
        // Arrange
        var context = CriarContexto();
        var config = CriarConfiguration();
        var service = new AuthService(context, config);

        var dto = new LoginDto
        {
            Email = "naoexiste@email.com",
            Senha = "senha123"
        };

        // Act
        var token = await service.LoginAsync(dto);

        // Assert
        token.Should().BeNull();
    }

    [Fact]
    public async Task LoginAsync_DeveRetornarNull_QuandoSenhaErrada()
    {
        // Arrange
        var context = CriarContexto();
        var config = CriarConfiguration();
        var service = new AuthService(context, config);

        await service.RegisterAsync(new RegisterDto
        {
            Nome = "Carlos Silva",
            Email = "carlos@email.com",
            Senha = "senha123"
        });

        var dto = new LoginDto
        {
            Email = "carlos@email.com",
            Senha = "senhaerrada"
        };

        // Act
        var token = await service.LoginAsync(dto);

        // Assert
        token.Should().BeNull();
    }
}