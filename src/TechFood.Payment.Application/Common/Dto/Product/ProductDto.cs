using System;

namespace TechFood.Payment.Application.Common.Dto.Product
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
    }
}
