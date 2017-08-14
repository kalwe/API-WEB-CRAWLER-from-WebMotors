using System;
using Xunit;
using System.Threading.Tasks;
using NSubstitute;
using WebCrawler.Tools;

public class CrawlerWebToolsTests{

    private const string DataBaseUrl = "http://localhost:8686";

    // Make web request and check if result status is OK
    [Fact]
    public async Task ShouldMakeWebRequestToUrlAndGetResponseAsync()
    {
        // Arrange
        Uri url = new Uri(DataBaseUrl, UriKind.Absolute);
        var _getUrlResponse = Substitute.For<WebTools>();

        // Act
        await _getUrlResponse.GetUrlResponse(url);
        var webRequest = _getUrlResponse.WebResponse;
        var httpResponseStatus = _getUrlResponse.HttpResponseStatusOK(webRequest);

        // Assert
        Assert.True(httpResponseStatus);
    }

    // Return a StreamReader from web request and chek if responde lengh is greater than 0
    // and check if web request status is OK
    [Fact]
    public async Task ShouldReturnStreamReaderFromHttpResponseGetResponseStream()
    {
        // Arrange
        Uri url = new Uri(DataBaseUrl, UriKind.Absolute);
        var _getUrlResponse = Substitute.For<WebTools>();
        await _getUrlResponse.GetUrlResponse(url);
        var webResponse = _getUrlResponse.WebResponse;

        // Act
        var httpResponseStatusOK = _getUrlResponse.HttpResponseStatusOK(webResponse);
        var httpResponseLength = _getUrlResponse.HttpResponseContentExist(webResponse);
        var streamResult = _getUrlResponse.GetHttpResponseStream(webResponse);

        // Assert
        Assert.True(httpResponseStatusOK);
        Assert.True(httpResponseLength);
        Assert.NotNull(streamResult);
    }
}
