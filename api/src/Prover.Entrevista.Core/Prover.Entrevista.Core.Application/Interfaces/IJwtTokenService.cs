using Prover.Entrevista.Core.Domain.Entities;

namespace Prover.Entrevista.Core.Application.Interfaces;

public interface IJwtTokenService
{
    public string GenerateAuthToken(User user);
    //public IEnumerable<string> GetClaimsFromToken(string token, IEnumerable<string> claimTypes);
}