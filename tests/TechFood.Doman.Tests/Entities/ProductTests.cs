using TechFood.Payment.Domain.Entities;
using TechFood.Shared.Domain.Exceptions;

namespace TechFood.Doman.Tests.Entities;

[Trait("Category", "Unit")]
[Trait("Domain", "Product")]
public class ProductTests
{
    [Fact]
    public void Constructor_Should_CreateProduct_WithValidData()
    {
        // Arrange
        var name = "Hamburguer";
        var description = "Delicious burger";
        var categoryId = Guid.NewGuid();
        var imageFileName = "burger.jpg";
        var price = 25.50m;

        // Act
        var product = new Product(name, description, categoryId, imageFileName, price);

        // Assert
        product.Should().NotBeNull();
        product.Name.Should().Be(name);
        product.Description.Should().Be(description);
        product.CategoryId.Should().Be(categoryId);
        product.ImageFileName.Should().Be(imageFileName);
        product.Price.Should().Be(price);
        product.OutOfStock.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_Should_ThrowDomainException_WhenNameIsEmpty(string name)
    {
        // Arrange
        var description = "Test description";
        var categoryId = Guid.NewGuid();
        var imageFileName = "test.jpg";
        var price = 10.00m;

        // Act
        var act = () => new Product(name, description, categoryId, imageFileName, price);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_Should_ThrowDomainException_WhenDescriptionIsEmpty(string description)
    {
        // Arrange
        var name = "Product";
        var categoryId = Guid.NewGuid();
        var imageFileName = "test.jpg";
        var price = 10.00m;

        // Act
        var act = () => new Product(name, description, categoryId, imageFileName, price);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Constructor_Should_ThrowDomainException_WhenCategoryIdIsEmpty()
    {
        // Arrange
        var name = "Product";
        var description = "Description";
        var categoryId = Guid.Empty;
        var imageFileName = "test.jpg";
        var price = 10.00m;

        // Act
        var act = () => new Product(name, description, categoryId, imageFileName, price);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_Should_ThrowDomainException_WhenImageFileNameIsEmpty(string imageFileName)
    {
        // Arrange
        var name = "Product";
        var description = "Description";
        var categoryId = Guid.NewGuid();
        var price = 10.00m;

        // Act
        var act = () => new Product(name, description, categoryId, imageFileName, price);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10.5)]
    public void Constructor_Should_ThrowDomainException_WhenPriceIsNegative(decimal price)
    {
        // Arrange
        var name = "Product";
        var description = "Description";
        var categoryId = Guid.NewGuid();
        var imageFileName = "test.jpg";

        // Act
        var act = () => new Product(name, description, categoryId, imageFileName, price);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void SetCategory_Should_UpdateCategoryId()
    {
        // Arrange
        var product = new Product("Test", "Description", Guid.NewGuid(), "image.jpg", 10.00m);
        var newCategoryId = Guid.NewGuid();

        // Act
        product.SetCategory(newCategoryId);

        // Assert
        product.CategoryId.Should().Be(newCategoryId);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void SetOutOfStock_Should_UpdateOutOfStockStatus(bool outOfStock)
    {
        // Arrange
        var product = new Product("Test", "Description", Guid.NewGuid(), "image.jpg", 10.00m);

        // Act
        product.SetOutOfStock(outOfStock);

        // Assert
        product.OutOfStock.Should().Be(outOfStock);
    }

    [Fact]
    public void SetImageFileName_Should_UpdateImageFileName()
    {
        // Arrange
        var product = new Product("Test", "Description", Guid.NewGuid(), "image.jpg", 10.00m);
        var newImageFileName = "new-image.jpg";

        // Act
        product.SetImageFileName(newImageFileName);

        // Assert
        product.ImageFileName.Should().Be(newImageFileName);
    }

    [Fact]
    public void Update_Should_UpdateAllProperties_WithValidData()
    {
        // Arrange
        var product = new Product("Old Name", "Old Description", Guid.NewGuid(), "old.jpg", 10.00m);
        var newName = "New Name";
        var newDescription = "New Description";
        var newImageFileName = "new.jpg";
        var newPrice = 25.00m;
        var newCategoryId = Guid.NewGuid();

        // Act
        product.Update(newName, newDescription, newImageFileName, newPrice, newCategoryId);

        // Assert
        product.Name.Should().Be(newName);
        product.Description.Should().Be(newDescription);
        product.ImageFileName.Should().Be(newImageFileName);
        product.Price.Should().Be(newPrice);
        product.CategoryId.Should().Be(newCategoryId);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Update_Should_ThrowDomainException_WhenNameIsEmpty(string name)
    {
        // Arrange
        var product = new Product("Test", "Description", Guid.NewGuid(), "image.jpg", 10.00m);

        // Act
        var act = () => product.Update(name, "Description", "image.jpg", 10.00m, Guid.NewGuid());

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10.5)]
    public void Update_Should_ThrowDomainException_WhenPriceIsNegative(decimal price)
    {
        // Arrange
        var product = new Product("Test", "Description", Guid.NewGuid(), "image.jpg", 10.00m);

        // Act
        var act = () => product.Update("Test", "Description", "image.jpg", price, Guid.NewGuid());

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Properties_Should_BeInitializedCorrectly()
    {
        // Arrange & Act
        var product = new Product("Pizza", "Pepperoni Pizza", Guid.NewGuid(), "pizza.jpg", 35.90m);

        // Assert
        product.Id.Should().NotBeEmpty();
        product.Name.Should().NotBeNullOrWhiteSpace();
        product.Description.Should().NotBeNullOrWhiteSpace();
        product.CategoryId.Should().NotBeEmpty();
        product.ImageFileName.Should().NotBeNullOrWhiteSpace();
        product.Price.Should().BeGreaterThan(0);
        product.OutOfStock.Should().BeFalse();
    }
}
