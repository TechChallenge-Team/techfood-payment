using System;
using TechFood.Shared.Domain.Entities;

namespace TechFood.Domain.Entities;

public class Product : Entity, IAggregateRoot
{
    private Product() { }

    public Product(
        string name,
        string description,
        Guid categoryId,
        string imageFileName,
        decimal price
        )
    {
        Name = name;
        Description = description;
        CategoryId = categoryId;
        ImageFileName = imageFileName;
        Price = price;

        Validate();
    }

    public string Name { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public Guid CategoryId { get; private set; }

    public bool OutOfStock { get; private set; }

    public string ImageFileName { get; private set; } = null!;

    public decimal Price { get; private set; }

    public void SetCategory(Guid categoryId)
        => CategoryId = categoryId;

    public void SetOutOfStock(bool outOfStock)
       => OutOfStock = outOfStock;

    public void SetImageFileName(string imageFileName)
       => ImageFileName = imageFileName;

    public void Update(
        string name,
        string description,
        string imageFileName,
        decimal price,
        Guid categoryId)
    {
        Name = name;
        Description = description;
        CategoryId = categoryId;
        ImageFileName = imageFileName;
        Price = price;

        Validate();
    }

    private void Validate()
    {
        Validations.ThrowIfEmpty(Name, Resources.Exceptions.Product_ThrowNameIsEmpty);
        Validations.ThrowIfEmpty(Description, Resources.Exceptions.Product_ThrowDescriptionIsEmpty);
        Validations.ThrowValidGuid(CategoryId, Resources.Exceptions.Product_ThrowCategoryIdInvalid);
        Validations.ThrowIfEmpty(ImageFileName, Resources.Exceptions.Product_ThrowCategoryImageFileIsEmpty);
        Validations.ThrowIsGreaterThanZero(Price, Resources.Exceptions.Product_ThrowPriceIsGreaterThanZero);
    }
}
