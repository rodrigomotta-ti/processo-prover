using Dapper;
using Prover.Entrevista.Core.Domain.Entities;
using Prover.Entrevista.Core.Domain.Interfaces.Repositories;
using Prover.Entrevista.Core.Infrastructure.Interfaces.Context;

namespace Prover.Entrevista.Core.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbContext context;

    public UserRepository(IDbContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Add(User user)
    {
        const string query = "INSERT INTO users (name, email, password) VALUES (@Name, @Email, @Password)";

        await context.Connection.ExecuteAsync(query, user);
    }

    public async Task<User> GetByEmail(string email)
    {
        var query = "SELECT * FROM users WHERE email = @email";
        var user = await context.Connection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });
        
        return user!;
    }
}