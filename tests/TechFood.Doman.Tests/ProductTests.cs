namespace TechFood.Doman.Tests
{
    public class ProductTests
    {
        [Fact(DisplayName = "Should create product with valid data")]
        [Trait("Product", "Create Product")]
        public void ShouldCreateProduct_WithValidData()
        {
            // Arrange
            var name = "X-Burger";
            var description = "Delicious burger with cheese";
            var categoryId = Guid.NewGuid();
            var imageFileName = "burger.jpg";
            var price = 25.90m;

            // Act
            var product = new TechFood.Payment.Domain.Entities.Product(
                name: name,
                description: description,
                categoryId: categoryId,
                imageFileName: imageFileName,
                price: price);

            // Assert
            Assert.NotEqual(Guid.Empty, product.Id);
            Assert.Equal(name, product.Name);
            Assert.Equal(description, product.Description);
            Assert.Equal(categoryId, product.CategoryId);
            Assert.Equal(imageFileName, product.ImageFileName);
            Assert.Equal(price, product.Price);
            Assert.False(product.OutOfStock);
        }

        [Fact(DisplayName = "Should throw exception when name is empty")]
        [Trait("Product", "Validation")]
        public void ShouldThrowException_WhenNameIsEmpty()
        {
            // Arrange & Act & Assert
            Assert.Throws<TechFood.Shared.Domain.Exceptions.DomainException>(() =>
                new TechFood.Payment.Domain.Entities.Product(
                    name: "",
                    description: "Valid description",
                    categoryId: Guid.NewGuid(),
                    imageFileName: "image.jpg",
                    price: 10m));
        }

        [Fact(DisplayName = "Should throw exception when description is empty")]
        [Trait("Product", "Validation")]
        public void ShouldThrowException_WhenDescriptionIsEmpty()
        {
            // Arrange & Act & Assert
            Assert.Throws<TechFood.Shared.Domain.Exceptions.DomainException>(() =>
                new TechFood.Payment.Domain.Entities.Product(
                    name: "Valid Name",
                    description: "",
                    categoryId: Guid.NewGuid(),
                    imageFileName: "image.jpg",
                    price: 10m));
        }

        [Fact(DisplayName = "Should throw exception when category ID is empty")]
        [Trait("Product", "Validation")]
        public void ShouldThrowException_WhenCategoryIdIsEmpty()
        {
            // Arrange & Act & Assert
            Assert.Throws<TechFood.Shared.Domain.Exceptions.DomainException>(() =>
                new TechFood.Payment.Domain.Entities.Product(
                    name: "Valid Name",
                    description: "Valid description",
                    categoryId: Guid.Empty,
                    imageFileName: "image.jpg",
                    price: 10m));
        }

        [Fact(DisplayName = "Should throw exception when image filename is empty")]
        [Trait("Product", "Validation")]
        public void ShouldThrowException_WhenImageFileNameIsEmpty()
        {
            // Arrange & Act & Assert
            Assert.Throws<TechFood.Shared.Domain.Exceptions.DomainException>(() =>
                new TechFood.Payment.Domain.Entities.Product(
                    name: "Valid Name",
                    description: "Valid description",
                    categoryId: Guid.NewGuid(),
                    imageFileName: "",
                    price: 10m));
        }

        [Theory(DisplayName = "Should throw exception when price is negative")]
        [Trait("Product", "Validation")]
        [InlineData(-1)]
        [InlineData(-10.50)]
        public void ShouldThrowException_WhenPriceIsNegative(decimal price)
        {
            // Arrange & Act & Assert
            Assert.Throws<TechFood.Shared.Domain.Exceptions.DomainException>(() =>
                new TechFood.Payment.Domain.Entities.Product(
                    name: "Valid Name",
                    description: "Valid description",
                    categoryId: Guid.NewGuid(),
                    imageFileName: "image.jpg",
                    price: price));
        }

        [Fact(DisplayName = "Should update product successfully")]
        [Trait("Product", "Update Product")]
        public void ShouldUpdateProduct_Successfully()
        {
            // Arrange
            var product = new TechFood.Payment.Domain.Entities.Product(
                name: "Original Name",
                description: "Original Description",
                categoryId: Guid.NewGuid(),
                imageFileName: "original.jpg",
                price: 10m);

            var newName = "Updated Name";
            var newDescription = "Updated Description";
            var newCategoryId = Guid.NewGuid();
            var newImageFileName = "updated.jpg";
            var newPrice = 20m;

            // Act
            product.Update(newName, newDescription, newImageFileName, newPrice, newCategoryId);

            // Assert
            Assert.Equal(newName, product.Name);
            Assert.Equal(newDescription, product.Description);
            Assert.Equal(newCategoryId, product.CategoryId);
            Assert.Equal(newImageFileName, product.ImageFileName);
            Assert.Equal(newPrice, product.Price);
        }

        [Fact(DisplayName = "Should set category successfully")]
        [Trait("Product", "Set Category")]
        public void ShouldSetCategory_Successfully()
        {
            // Arrange
            var product = new TechFood.Payment.Domain.Entities.Product(
                name: "Product",
                description: "Description",
                categoryId: Guid.NewGuid(),
                imageFileName: "image.jpg",
                price: 10m);

            var newCategoryId = Guid.NewGuid();

            // Act
            product.SetCategory(newCategoryId);

            // Assert
            Assert.Equal(newCategoryId, product.CategoryId);
        }

        [Fact(DisplayName = "Should set out of stock successfully")]
        [Trait("Product", "Set Out Of Stock")]
        public void ShouldSetOutOfStock_Successfully()
        {
            // Arrange
            var product = new TechFood.Payment.Domain.Entities.Product(
                name: "Product",
                description: "Description",
                categoryId: Guid.NewGuid(),
                imageFileName: "image.jpg",
                price: 10m);

            // Act
            product.SetOutOfStock(true);

            // Assert
            Assert.True(product.OutOfStock);

            // Act again
            product.SetOutOfStock(false);

            // Assert
            Assert.False(product.OutOfStock);
        }

        [Fact(DisplayName = "Should set image filename successfully")]
        [Trait("Product", "Set Image")]
        public void ShouldSetImageFileName_Successfully()
        {
            // Arrange
            var product = new TechFood.Payment.Domain.Entities.Product(
                name: "Product",
                description: "Description",
                categoryId: Guid.NewGuid(),
                imageFileName: "original.jpg",
                price: 10m);

            var newImageFileName = "new-image.jpg";

            // Act
            product.SetImageFileName(newImageFileName);

            // Assert
            Assert.Equal(newImageFileName, product.ImageFileName);
        }

        [Theory(DisplayName = "Should create product with various valid prices")]
        [Trait("Product", "Create Product")]
        [InlineData(0.01)]
        [InlineData(5.50)]
        [InlineData(10.00)]
        [InlineData(99.99)]
        [InlineData(1000.00)]
        public void ShouldCreateProduct_WithVariousValidPrices(decimal price)
        {
            // Arrange & Act
            var product = new TechFood.Payment.Domain.Entities.Product(
                name: "Product",
                description: "Description",
                categoryId: Guid.NewGuid(),
                imageFileName: "image.jpg",
                price: price);

            // Assert
            Assert.Equal(price, product.Price);
        }
    }
}
