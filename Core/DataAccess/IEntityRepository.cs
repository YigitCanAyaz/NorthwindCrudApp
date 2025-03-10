﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.DataAccess
{
    // T bir referans tipi olmalı, ve IEntity olabilir ya da implement eden bir class olmalı
    // generic constraint
    // class: referans tip
    // IEntity: IEntity olabilir veya IEntity implemente eden bir nesne olabilir
    // new(): new'lenebilir olmalı, IEntity koymamızı engeller
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        List<T> GetAll(Expression<Func<T, bool>> filter = null);
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Update(T entity); 
        void Delete(T entity);
    }
}
