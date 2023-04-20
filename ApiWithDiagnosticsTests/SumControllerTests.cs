using ApiWithDiagnostics.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithDiagnosticsTests;

public class SumControllerTests
{
    [Theory]
    [InlineData(5, new int[] { 1, 7, -3 })]
    [InlineData(0, new int[] { })]
    [InlineData(21, new int[] { 7, 9, 11, -6, -21, 21 })]
    public async Task ReturnsExpectedResult(int exp, int[] val)
    {
        var sut = new SumController();

        var result = await sut.SumInt(val);

        var okObjResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjResult.StatusCode);
        Assert.Equal(exp, okObjResult.Value);
    }
}