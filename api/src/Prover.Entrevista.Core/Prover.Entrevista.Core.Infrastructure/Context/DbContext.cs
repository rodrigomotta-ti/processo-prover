using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Prover.Entrevista.Core.Infrastructure.Interfaces.Context;
using System.Data;

namespace Prover.Entrevista.Core.Infrastructure.Context;

public class DbContext : IDbContext
{
    private readonly IDbConnection _connection;
    private readonly IConfiguration _configuration;

    public DbContext(IConfiguration configuration)
    {
        _configuration = configuration;

        var connectionString = ObterConnectionString();
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            _connection = new MySqlConnection(connectionString + ";Connection Timeout=30;");
            _connection.Open();
        }
    }

    /// <summary>
    /// Retorna uma conexão aberta com o banco de dados.
    /// </summary>
    public IDbConnection Connection
    {
        get
        {
            if (_connection == null || _connection.State == ConnectionState.Closed)
                throw new InvalidOperationException("Não foi possível criar a conexão com o banco de dados.");

            return _connection;
        }
    }

    #region // Dispose

    protected virtual void Dispose(bool disposing)
    {
        if (_connection.State == ConnectionState.Open)
            _connection.Close();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion

    #region // Open

    public void Open()
    {
        if (_connection.State != ConnectionState.Open)
            _connection.Open();
    }

    public void OpenConnection()
    {
        Open();
    }

    #endregion

    #region // Close

    public void Close()
    {
        _connection?.Close();
    }

    public void CloseConnection()
    {
        if (_connection.State != ConnectionState.Closed)
            Close();
    }

    #endregion

    private string ObterConnectionString()
    {
        //var connectionString = ConnectionStringBuilder.GetConnectionString();
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("A string de conexão 'DefaultConnection' não foi encontrada no arquivo appsettings.json.");

        return connectionString;
    }
}