using Prover.Entrevista.Core.Application.DTO;
using Prover.Entrevista.Core.Application.Interfaces;
using Prover.Entrevista.Core.Common.Extensions;
using Prover.Entrevista.Core.Domain.Entities;
using Prover.Entrevista.Core.Domain.Interfaces.Repositories;
using System.Text;

namespace Prover.Entrevista.Core.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _tokenService;

    public AuthService(IUserRepository userRepository, IJwtTokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<string> AuthenticateUser(LoginDTO login)
    {
        var user = await _userRepository.GetByEmail(login.Email);
        if (user is null)
            throw new UnauthorizedAccessException("Usuário ou senha inválidos.");

        var hashedPassword = StringExtensions.ToSHA256(login.Password);
        if (user.Password != hashedPassword)
            throw new UnauthorizedAccessException("Usuário ou senha inválidos.");

        return _tokenService.GenerateAuthToken(user);
    }

    private string GenerateAuthToken(User user)
    {
        // Simulação de um token (substitua por JWT em produção)
        return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user.Email}:{DateTime.UtcNow}"));
    }
}