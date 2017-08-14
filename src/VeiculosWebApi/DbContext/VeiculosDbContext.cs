using Raven.Client;
using Raven.Client.Document;
using VeiculosWebApi.Interfaces;

namespace VeiculosWebApi.DbContext
{
    public class VeiculosDbContext : ExceptionHelp, IVeiculosDbContext
    {
        public IDocumentStore Store { get; internal set; }
        public IAsyncDocumentSession SessionAsync { get; internal set; }

        // Default Constructor
        public VeiculosDbContext()
        {
            Store = StoreDbInitialize();
            SessionAsync = OpenSessionAsync();
        }

        // Initialize Database Store
        public IDocumentStore StoreDbInitialize()
        {
            Store = new DocumentStore()
            {
                Url = "http://localhost:8686/", // Url server and port
                DefaultDatabase = "VeiculosDocs_Test" //+ DateTime.Now.Ticks
            };
            Store.Initialize(); // Initialize document store, to connecting with server and load configurations
            return (DocumentStore)Store;
        }

        // Open a session to commit/save changes in database
        public IAsyncDocumentSession OpenSessionAsync()
        {
            SessionAsync = Store.OpenAsyncSession();
            return SessionAsync;
        }
    }
}
