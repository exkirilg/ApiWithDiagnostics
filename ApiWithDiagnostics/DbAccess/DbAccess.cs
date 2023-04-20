using ApiWithDiagnostics.Models;
using Dapper;
using Npgsql;
using System.Data;

namespace ApiWithDiagnostics.DbAccess;

public class DbAccess : IDbAccess
{
    private readonly string _connectionString;

    public DbAccess(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("QuotesDBConnection")!;
    }

    public async Task CreateQuotesTable()
    {
        using IDbConnection connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(PostgreSQLScripts.CreateQuotesTable);
    }

    public async Task PopulateQuotes()
    {
        using IDbConnection connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(PostgreSQLScripts.PopulateQuotes);
    }

    public async Task<Quote> GetRandomQuote()
    {
        using IDbConnection connection = new NpgsqlConnection(_connectionString);
        return await connection.QuerySingleAsync<Quote>(PostgreSQLScripts.GetRandomQuote);
    }

    public async Task NewQuote(QuoteDto quoteDto)
    {
        using IDbConnection connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(PostgreSQLScripts.InsertQuote, quoteDto);
    }
}
