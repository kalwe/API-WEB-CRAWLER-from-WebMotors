using Raven.Client;

namespace VeiculosWebApi.Interfaces
{
    public interface IVeiculosDbContext
    {
        IDocumentStore StoreDbInitialize();

        IAsyncDocumentSession OpenSessionAsync();
    }
}
