using System;

namespace TechFood.Application.Common.Data.Product
{
    public class ProductResult
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
    }
}
