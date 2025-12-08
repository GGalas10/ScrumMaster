using Moq;

namespace ScrumMaster.Tasks.Tests.InfrastructureTest.Commons
{
    public static class IHttpFactoryInit
    {
        public static IHttpClientFactory Create()
        {
            var handler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(handler.Object);
            var httpFactory = new Mock<IHttpClientFactory>();
            httpFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
                       .Returns(httpClient);
            return httpFactory.Object;
        }
    }
}
