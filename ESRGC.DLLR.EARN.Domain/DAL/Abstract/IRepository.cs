using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESRGC.DLLR.EARN.Domain.DAL.Abstract
{
    public interface IRepository<TEntity>
    {
        IQueryable<TEntity> Entities { get; }
        TEntity GetEntityByID(object ID);
        void DeleteByID(object ID);

        void InsertEntity(TEntity entity);
        void UpdateEntity(TEntity entity);
        void DeleteEntity(TEntity entity);
    }
}
