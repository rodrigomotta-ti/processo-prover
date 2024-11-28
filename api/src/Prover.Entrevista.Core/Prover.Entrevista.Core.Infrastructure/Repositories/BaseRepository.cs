using Dapper;
using Dommel;
using Prover.Entrevista.Core.Common.Extensions;
using Prover.Entrevista.Core.Domain.Entities;
using Prover.Entrevista.Core.Domain.Interfaces.Repositories;
using Prover.Entrevista.Core.Infrastructure.Extensions;
using Prover.Entrevista.Core.Infrastructure.Interfaces.Context;
using static Dapper.SqlMapper;

namespace Prover.Entrevista.Core.Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly IDbContext context;

    public BaseRepository(IDbContext context)
    { 
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var tableName = Resolvers.Table(typeof(T), context.Connection);
        return await context.Connection.QueryAsync<T>($"SELECT * FROM {tableName};");
    }

    public async Task<T> GetByIdAsync(int id)
    {
        var tableName = Resolvers.Table(typeof(T), context.Connection);
        var internalIdColunmName = Resolvers.Column(PropertyHelper<T>.GetProperty(x => x.Id), context.Connection);

        return await context.Connection.QuerySingleOrDefaultAsync<T>($"SELECT * FROM {tableName} WHERE {internalIdColunmName} = @id;", new { id });
    }

    public async Task<int> CreateAsync(T entity)
    {
        var tableName = Resolvers.Table(typeof(T), context.Connection);
        var sql = GenerateInsertSql(tableName, entity);

        return await context.Connection.ExecuteAsync(sql, entity);
    }

    public async Task<int> UpdateAsync(T entity)
    {
        var tableName = Resolvers.Table(typeof(T), context.Connection);
        var sql = GenerateUpdateSql(tableName, entity);

        return await context.Connection.ExecuteAsync(sql, entity);
    }

    public async Task<int> DeleteAsync(string tableName, int id)
        => await context.Connection.ExecuteAsync($"DELETE FROM {tableName} WHERE Id = @Id", new { Id = id });

    public async Task InsertList(IEnumerable<T> entities)
        => await context.Connection.InsertAllAsync(entities);

    public async Task UpdateBatch(IEnumerable<T> entities) 
        => await context.Connection.UpdateBatch(entities);

    public async Task RemoveBatch(IEnumerable<T> entities)
    {
        var ids = entities.Select(x => x.Id);
        var tableName = Resolvers.Table(typeof(T), context.Connection);
        var internalIdColunmName = Dommel.Resolvers.Column(PropertyHelper<T>.GetProperty(x => x.Id), context.Connection);

        var sql = $"DELETE FROM {tableName} WHERE {internalIdColunmName} IN @ids;";

        await context.Connection.ExecuteAsync(sql, new { ids = ids.ToArray() });
    }

    private string GenerateInsertSql(string tableName, T entity)
    {
        var sqlResult = string.Empty;
        var properties = typeof(T).GetProperties()
                                .Where(p => p.CanRead && !Attribute.IsDefined(p, typeof(IgnoreAttribute)) && p.Name != "Id" && p.Name != "UpdateAt")
                                .Select(p => p.Name);

        var columns = string.Join(", ", properties);
        var parameters = string.Join(", ", properties.Select(p => $"@{p}"));
        
        sqlResult = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";
        
        return sqlResult;
    }

    private string GenerateUpdateSql(string tableName, T entity)
    {
        var sqlResult = string.Empty;
        var properties = typeof(T).GetProperties()
                                .Where(p => p.CanRead && !Attribute.IsDefined(p, typeof(IgnoreAttribute)) && p.Name != "Id" && p.Name != "CreateAt")
                                .Select(p => p.Name);

        var setClause = string.Join(", ", properties.Select(p => $"{p} = @{p}"));
        sqlResult = $"UPDATE {tableName} SET {setClause} WHERE Id = @Id";

        return sqlResult;
    }
}