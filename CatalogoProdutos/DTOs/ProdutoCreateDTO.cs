using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CatalogoProdutos.DTOs;

public class ProdutoCreateDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(255, ErrorMessage = "O nome deve ter no máximo 255 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "O preço é obrigatório.")]
    [Precision(18, 2)]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser um número positivo.")]
    public decimal Preco { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "O estoque deve ser um número inteiro não negativo.")]
    public int Estoque { get; set; }
}