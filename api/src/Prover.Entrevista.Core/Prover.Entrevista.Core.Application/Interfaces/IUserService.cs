using Prover.Entrevista.Core.Application.DTO;
using Prover.Entrevista.Core.Application.DTO.Filters;
using Prover.Entrevista.Core.Domain.Entities;

namespace Prover.Entrevista.Core.Application.Interfaces;

public interface IUserService
{
    Task<User> RegisterUser(RegisterUserDTO dto);
    Task<User> GetUserByEmail(SearchParametersLoginDTO login);
}