using Microsoft.Extensions.Configuration;

namespace Prover.Entrevista.Core.Infrastructure.Context;

public static class ConnectionStringBuilder
{
    private static readonly IConfiguration _configuration;

    public static string GetConnectionString()
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("A string de conexão 'DefaultConnection' não foi encontrada no arquivo appsettings.json.");

        return connectionString;
    }
}