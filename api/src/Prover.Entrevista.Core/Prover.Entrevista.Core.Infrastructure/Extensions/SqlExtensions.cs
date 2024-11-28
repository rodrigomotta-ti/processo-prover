using Dapper;
using System.Data;
using System.Text;

namespace Prover.Entrevista.Core.Infrastructure.Extensions;

public static class SqlExtensions
{
    public static async Task UpdateBatch<T>(this IDbConnection connection, IEnumerable<T> entities)
    {
        var sqlBuilder = new StringBuilder();
        var tableName = Dommel.Resolvers.Table(typeof(T), connection);

        var keyProperties = Dommel.Resolvers.KeyProperties(typeof(T));
        var typeProperties = Dommel.Resolvers.Properties(typeof(T))
            .Where(x => !x.IsGenerated)
            .Select(x => x.Property)
            .Except(keyProperties.Where(p => p.IsGenerated).Select(p => p.Property));

        var columnNames = typeProperties.Select(p => $"{Dommel.Resolvers.Column(p, connection)} = @{p.Name}");
        var whereClauses = keyProperties.Select(p => $"{Dommel.Resolvers.Column(p.Property, connection)} = @{p.Property.Name}");
        var sql = $"UPDATE {tableName} SET {string.Join(", ", columnNames)} WHERE {string.Join(" AND ", whereClauses)}";

        await connection.ExecuteAsync(sql, entities);
    }
}