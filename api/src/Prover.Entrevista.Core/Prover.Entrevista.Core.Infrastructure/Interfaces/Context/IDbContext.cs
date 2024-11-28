using System.Data;

namespace Prover.Entrevista.Core.Infrastructure.Interfaces.Context;

public interface IDbContext : IDisposable
{
    IDbConnection Connection { get; }
    void OpenConnection();
    void CloseConnection();
}