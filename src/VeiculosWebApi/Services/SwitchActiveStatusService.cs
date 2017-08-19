using System.Reflection;
using System.Threading.Tasks;
using VeiculosWebApi.Interfaces.Repositories;
using VeiculosWebApi.Interfaces.Services;

namespace VeiculosWebApi.Services
{
    public class SwitchActiveStatusService<TEntity> : ISwitchActiveStatusService<TEntity>
    {
        // Ativa o status 'Active' da entidade
        public TEntity SetActiveStatusTrue(TEntity entity)
        {
            if (!GetActiveStatus(entity))
                InvertAndSetActiveValue(entity);

            return entity;
        }

        // Desativa o status 'Active' da entidade
        public TEntity SetActiveStatusFalse(TEntity entity)
        {
            if (GetActiveStatus(entity))
                InvertAndSetActiveValue(entity);

            return entity;
        }

        // Seta o valor da propriedade 'Active' invertendo o valor atual
        private TEntity InvertAndSetActiveValue(TEntity entity)
        {
            entity.GetType().GetProperty("Active").SetValue(entity, !GetActiveStatus(entity));
            return entity;
        }


        // Obtem o valor atual da propriedade 'Active'
        private bool GetActiveStatus(TEntity entity)
        {
           var activeValue = (bool)entity.GetType().GetProperty("Active").GetValue(entity);
           return activeValue;
        }
    }
}