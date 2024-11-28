using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prover.Entrevista.Core.Application.DTO;
using Prover.Entrevista.Core.Application.Interfaces;

namespace Prover.Entrevista.Core.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO login)
    {
        var token = await _authService.AuthenticateUser(login);

        if (token is null)
            return Unauthorized("Credenciais inválidas.");

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, 
            Secure = true, 
            SameSite = SameSiteMode.Strict, 
            Expires = DateTime.Now.AddHours(1) 
        };

        Response.Cookies.Append("auth_token", token, cookieOptions);

        return Ok(new { Token = token });
    }
}