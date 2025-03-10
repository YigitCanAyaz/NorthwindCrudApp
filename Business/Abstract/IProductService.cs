﻿using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IProductService
    {
        IDataResult<List<Product>> GetAll();
        IDataResult<List<Product>> GetAllByCategoryId(int id);
        IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max);
        IResult Add(Product product);
        IResult AddTransactionalTest(Product product);
        IResult Update(Product product);
        IDataResult<Product> GetById(int productId);

        IDataResult<List<ProductDetailDto>> GetProductDetails();

        // RESTFUL --> HTTP --> TCP (İki cihazı birbiriyle görüştürme vs.)
    }
}
