using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Entities.OrderModule;
using System;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces
{
    public interface IUnitOfWork 
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;

        Task<int> CompleteAsync();
    }
}