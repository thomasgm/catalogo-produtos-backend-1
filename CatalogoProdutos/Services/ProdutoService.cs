using CatalogoProdutos.Data;
using CatalogoProdutos.DTOs;
using CatalogoProdutos.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoProdutos.Services;

public class ProdutoService : IProdutoService
{
    private readonly AppDbContext _context;

    public ProdutoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProdutoResponseDto>> GetAllAsync(int page, int pageSize)
    {
        return await _context.Produtos
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => ToResponseDto(p))
            .ToListAsync();
    }

    public async Task<ProdutoResponseDto?> GetByIdAsync(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        return produto is null ? null : ToResponseDto(produto);
    }

    public async Task<ProdutoResponseDto> CreateAsync(ProdutoCreateDto dto)
    {
        var produto = new Produto
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Preco = dto.Preco,
            Estoque = dto.Estoque
        };

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();

        return ToResponseDto(produto);
    }

    public async Task<ProdutoResponseDto?> UpdateAsync(int id, ProdutoUpdateDto dto)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto is null) return null;

        produto.Nome = dto.Nome;
        produto.Descricao = dto.Descricao;
        produto.Preco = dto.Preco;
        produto.Estoque = dto.Estoque;

        await _context.SaveChangesAsync();

        return ToResponseDto(produto);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto is null) return false;

        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<ProdutoResponseDto>> SearchByNameAsync(string nome)
    {
        return await _context.Produtos
            .Where(p => p.Nome.Contains(nome))
            .Select(p => ToResponseDto(p))
            .ToListAsync();
    }

    // Método privado pra converter Model → ResponseDto
    private static ProdutoResponseDto ToResponseDto(Produto p) => new()
    {
        Id = p.Id,
        Nome = p.Nome,
        Descricao = p.Descricao,
        Preco = p.Preco,
        Estoque = p.Estoque,
        CriadoEm = p.CriadoEm
    };
}