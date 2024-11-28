using Prover.Entrevista.Core.Application.DTO;

namespace Prover.Entrevista.Core.Application.Interfaces;

public interface IAuthService
{
    Task<string> AuthenticateUser(LoginDTO login);
}