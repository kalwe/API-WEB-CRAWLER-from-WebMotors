namespace VeiculosWebApi.Interfaces.Services
{
    public interface ISwitchActiveStatusService<TEntity>
    {
        TEntity SetActiveStatusFalse(TEntity entity);

        TEntity SetActiveStatusTrue(TEntity entity);
    }
}