using System.Threading.Tasks;
using Cookbook.API.Controllers;
using Cookbook.Core;
using Cookbook.SharedData;
using Cookbook.SharedData.Entities;
using Cookbook.SharedData.Mappers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Cookbook.API.Tests;

public class RecipesControllerUnitTests
{
    [Fact]
    public async Task GetBy_With_GoodId_Should_Return_Ok_With_Recipe()
    {
        var cookbookServiceMock = Mock.Of<ICookbookService>();
        Mock.Get(cookbookServiceMock)
            .Setup(s => s.GetRecipeByAsync(It.IsAny<int>()))
            .ReturnsAsync(new Recipe());

        var sut = new RecipesController(cookbookServiceMock);

        var actualResult = await sut.GetBy(1);
        var expectedResult = new OkObjectResult(new Recipe().ToRecipeResponse());

        Assert.IsType<OkObjectResult>(actualResult);

        Assert.Equivalent(expectedResult, actualResult);
    }

    [Fact]
    public async Task GetBy_With_BadId_Should_Return_NotFound()
    {
        var cookbookServiceMock = Mock.Of<ICookbookService>();
        Mock.Get(cookbookServiceMock)
            .Setup(s => s.GetRecipeByAsync(It.IsAny<int>()))
            .ThrowsAsync(new ResourceNotFoundException(typeof(Recipe)));

        var sut = new RecipesController(cookbookServiceMock);

        var exceptionMessage = "Recipe not found";

        var ex = await Assert.ThrowsAsync<ResourceNotFoundException>(
            () => sut.GetBy(1)
            );
        Assert.Contains(exceptionMessage, ex.Message);
        
    }
}