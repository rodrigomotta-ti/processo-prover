using Prover.Entrevista.Core.Application.DTO;
using Prover.Entrevista.Core.Application.DTO.Filters;
using Prover.Entrevista.Core.Application.Interfaces;
using Prover.Entrevista.Core.Common.Extensions;
using Prover.Entrevista.Core.Domain.Entities;
using Prover.Entrevista.Core.Domain.Interfaces.Repositories;

namespace Prover.Entrevista.Core.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<User> RegisterUser(RegisterUserDTO dto)
    {
        var existingUser = await _userRepository.GetByEmail(dto.Email);
        if (existingUser != null)
            throw new InvalidOperationException("Email já registrado.");

        var hashedPassword = StringExtensions.ToSHA256(dto.Password);

        var count = hashedPassword.Count();

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = hashedPassword
        };

        await _userRepository.Add(user);

        return user;
    }

    public async Task<User> GetUserByEmail(SearchParametersLoginDTO login) 
        => await _userRepository.GetByEmail(login.Email);
}