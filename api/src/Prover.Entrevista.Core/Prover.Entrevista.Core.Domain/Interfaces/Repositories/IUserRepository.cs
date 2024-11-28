using Prover.Entrevista.Core.Domain.Entities;

namespace Prover.Entrevista.Core.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> GetByEmail(string email);
    Task Add(User user);
}