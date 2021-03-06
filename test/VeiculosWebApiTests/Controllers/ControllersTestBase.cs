using System.Threading.Tasks;
using NSubstitute;
using VeiculosWebApi.DbContext;
using VeiculosWebApi.Interfaces;
using VeiculosWebApi.Interfaces.Repositories;
using VeiculosWebApi.Interfaces.Services;
using VeiculosWebApi.Repositories;
using VeiculosWebApi.Services;

namespace VeiculosWebApiTests.Controllers
{
    public class ControllersTestBase<TEntity> where TEntity : class
    {
        protected static readonly IVeiculosDbContext db = Substitute.For<VeiculosDbContext>();
        protected static readonly IRepositoryBase<TEntity> repositoryBase = new RepositoryBase<TEntity>(db);

        protected static readonly ISwitchActiveStatusService<TEntity> switchActiveStatus = new SwitchActiveStatusService<TEntity>();

        protected readonly IServiceBase<TEntity> serviceBase;

        public ControllersTestBase()
        {
            serviceBase = new ServiceBase<TEntity>(repositoryBase, switchActiveStatus);
        }

        public async Task AddAndCommit(TEntity entity)
        {
            serviceBase.Add(entity);
            await serviceBase.CommitAsync();
        }
    }
}