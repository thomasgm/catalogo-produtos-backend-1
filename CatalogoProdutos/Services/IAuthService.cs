using CatalogoProdutos.DTOs;

namespace CatalogoProdutos.Services;

public interface IAuthService
{
    Task<string?> RegisterAsync(RegisterDto dto);
    Task<string?> LoginAsync(LoginDto dto);
}