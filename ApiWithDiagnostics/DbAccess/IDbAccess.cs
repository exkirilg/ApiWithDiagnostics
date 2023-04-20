using ApiWithDiagnostics.Models;

namespace ApiWithDiagnostics.DbAccess;

public interface IDbAccess
{
    Task CreateQuotesTable();
    Task PopulateQuotes();
    Task<Quote> GetRandomQuote();
    Task NewQuote(QuoteDto quoteDto);
}
