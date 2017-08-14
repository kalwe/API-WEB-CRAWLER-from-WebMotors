using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using VeiculosWebApi.Interfaces.Repositories;
using VeiculosWebApi.Interfaces.Services;

namespace VeiculosWebApi.Services
{
    public class ServiceBase<TEntity> : IServiceBase<TEntity> where TEntity : class
    {
        private readonly IRepositoryBase<TEntity> _repository;

        IList<TEntity> Entities;

        // Default Constructor
        public ServiceBase(IRepositoryBase<TEntity> repository)
        {
            _repository = repository;
            Entities = new List<TEntity>();
        }

        // Retorna uma entidade generica, pega o valor da prop 'Active'
        // converte no tipo boolean, inverte o valor e salva no database
        public async Task SwitchInactiveStatus(string id)
        {
            var entityResult = await FindAsync(id);
            var activeValue = (bool)entityResult.GetType().GetProperty("Active").GetValue(entityResult);

            if (activeValue)
                entityResult.GetType().GetProperty("Active").SetValue(entityResult, !(bool)activeValue);
            else
                entityResult.GetType().GetProperty("Active").SetValue(entityResult, !(bool)activeValue);

            await AddUpdateAsync(entityResult);
        }

        // Add entity for commit
        public void Add(TEntity entity)
        {
            Entities.Add(entity);
        }


        // Execute the commit in database
        public async Task CommitAsync()
        {
            if (Entities.Count > 0)
            {
                do
                {
                    try
                    {
                        await _repository.AddOrUpdateAsync(Entities.FirstOrDefault());
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Service error: method Commit() try execute repository AddOrUpdateAsync()");
                        // return ServiceResult();
                    }

                    Entities.Remove(Entities.FirstOrDefault());

                } while (Entities.Count > 0);
            }
        }

        public async Task AddUpdateAsync(TEntity entity)
        {
            await _repository.AddOrUpdateAsync(entity);
        }

        // Find entity by id async
        public async Task<TEntity> FindAsync(string id)
        {
            return await _repository.FindAsync(id);
        }

        // List
        public async Task<IEnumerable<TEntity>> ListAsync(int size)
        {
            return await _repository.ListAsync(size);
        }

        // ListAll
        public async Task<IEnumerable<TEntity>> ListAllAsync()
        {
            return await _repository.ListAllAsync();
        }

        // Delete
        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}