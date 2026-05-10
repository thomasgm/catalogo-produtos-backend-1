using CatalogoProdutos.DTOs;
using CatalogoProdutos.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoProdutos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var token = await _authService.RegisterAsync(dto);
        if (token is null) return Conflict("Email já cadastrado.");
        return Ok(new { token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await _authService.LoginAsync(dto);
        if (token is null) return Unauthorized("Email ou senha inválidos.");
        return Ok(new { token });
    }
}