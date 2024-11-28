using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prover.Entrevista.Core.Application.DTO;
using Prover.Entrevista.Core.Application.DTO.Filters;
using Prover.Entrevista.Core.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Prover.Entrevista.Core.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    [Produces("application/json")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDTO dto)
    {
        await _userService.RegisterUser(dto);
        return Ok("Usuário cadastrado com sucesso.");
    }

    [HttpGet("search")]
    [Produces("application/json")]
    public async Task<IActionResult> SearchByEmail([FromQuery][Required] SearchParametersLoginDTO login)
    {
        var user = await _userService.GetUserByEmail(login);
        return Ok(user);
    }
}