// Contains url and name for RavenDB application database

namespace VeiculosWebApi.DbContext
{
    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }

        public string DefaultDatabase { get; set; }

        public ConnectionStrings(){
            DefaultConnection = "http://localhost:8686/";
            DefaultDatabase = "VeiculosDocs_Test";
        }
        
    }
}
