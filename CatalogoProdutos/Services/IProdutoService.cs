using CatalogoProdutos.DTOs;

namespace CatalogoProdutos.Services;

public interface IProdutoService
{
    Task<IEnumerable<ProdutoResponseDto>> GetAllAsync(int page, int pageSize);
    Task<ProdutoResponseDto?> GetByIdAsync(int id);
    Task<ProdutoResponseDto> CreateAsync(ProdutoCreateDto dto);
    Task<ProdutoResponseDto?> UpdateAsync(int id, ProdutoUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<ProdutoResponseDto>> SearchByNameAsync(string nome);
}