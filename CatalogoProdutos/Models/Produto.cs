using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CatalogoProdutos.Models;

public class Produto
{
    public int Id { get; set; }

    [Required]
    [StringLength(255, ErrorMessage = "O nome deve ter no máximo 255 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    [Precision(18, 2)]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser um número positivo.")]
    public decimal Preco { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "O estoque deve ser um número inteiro não negativo.")]
    public int Estoque { get; set; }

    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}