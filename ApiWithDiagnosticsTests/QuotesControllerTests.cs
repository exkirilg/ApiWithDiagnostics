using ApiWithDiagnostics.Controllers;
using ApiWithDiagnostics.DbAccess;
using ApiWithDiagnostics.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ApiWithDiagnosticsTests;

public class QuotesControllerTests
{
    private static readonly Quote _testQuote = new()
    {
        Id = 1,
        Text = "First and only",
        Author = "Humblee Meeself",
        Language = "gibberysh"
    };

    [Fact]
    public async Task RandomQuote_ReturnsExpectedResult()
    {
        var exp = _testQuote;

        var dbMock = new Mock<IDbAccess>();
        dbMock.Setup(x => x.GetRandomQuote()).ReturnsAsync(_testQuote);

        var sut = new QuotesController(dbMock.Object);

        var result = await sut.RandomQuote();

        var okObjResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjResult.StatusCode);
        Assert.Equal(exp, okObjResult.Value);
    }

    [Fact]
    public async Task NewQuote_ReturnsExpectedResult()
    {
        Quote resQuote = new();

        string expText = "TestText";
        string expAuthor = "TestAuthor";
        string expLanguage = "TestLanguage";

        var dbMock = new Mock<IDbAccess>();
        dbMock
            .Setup(x => x.NewQuote(It.IsAny<QuoteDto>()))
            .Callback((QuoteDto dto) =>
            {
                resQuote.Text = dto.Text;
                resQuote.Author = dto.Author;
                resQuote.Language = dto.Language;
            });

        var sut = new QuotesController(dbMock.Object);

        var result = await sut.NewQuote(new QuoteDto(expText, expAuthor, expLanguage));

        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(expText, resQuote.Text);
        Assert.Equal(expAuthor, resQuote.Author);
        Assert.Equal(expLanguage, resQuote.Language);
    }
}
