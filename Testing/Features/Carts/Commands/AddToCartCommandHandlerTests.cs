using Application.Common.Models;
using Application.Exceptions;
using Application.Features.Carts.Commands.AddToCart;
using Application.Features.Carts.Queries.GetCartByUserId;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace YourApplication.UnitTests.Features.Carts.Commands
{
    public class AddToCartCommandTests
    {
        [Fact]
        public async Task Handle_ValidCommand_ShouldAddToCart()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userContextServiceMock = new Mock<IUserContextService>();
            var mapperMock = new Mock<IMapper>();
            var productRepositoryMock = new Mock<IRepositoryAsync<Product>>();
            var cartRepositoryMock = new Mock<IRepositoryAsync<Cart>>();

            var userId = Guid.NewGuid();
            var productId = 1;
            var productName = "TestProduct";
            var productPrice = 10.0m;
            var productStockQuantity = 100;

            var command = new AddToCartCommand
            {
                ProductId = productId,
                Quantity = 5
            };

            var product = new Product
            {
                Id = productId,
                Name = productName,
                Price = productPrice,
                StockQuantity = productStockQuantity
            };

            var cart = new Cart(userId);

            unitOfWorkMock.Setup(u => u.GetRepository<Product>()).Returns(productRepositoryMock.Object);
            unitOfWorkMock.Setup(u => u.GetRepository<Cart>()).Returns(cartRepositoryMock.Object);
            userContextServiceMock.Setup(uc => uc.GetUserContext()).Returns(new UserContext(userId.ToString(), "TestUser", DateTime.UtcNow.ToString()));

            productRepositoryMock
                .Setup(pr => pr.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);
            cartRepositoryMock.Setup(cr => cr.FirstOrDefaultAsync(It.IsAny<CartByUserIdSpecification>(), It.IsAny<CancellationToken>())).ReturnsAsync(cart);

            var sut = new AddToCartCommandHandler(unitOfWorkMock.Object, mapperMock.Object, userContextServiceMock.Object);

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeGreaterThan(0);
            product.StockQuantity.Should().Be(productStockQuantity - command.Quantity);
            cart.Items.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Handle_InvalidProductId_ShouldThrowNotFoundException()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userContextServiceMock = new Mock<IUserContextService>();
            var mapperMock = new Mock<IMapper>();
            var productRepositoryMock = new Mock<IRepositoryAsync<Product>>();

            var userId = Guid.NewGuid().ToString();
            var productId = 1;

            var command = new AddToCartCommand
            {
                ProductId = productId,
                Quantity = 5
            };

            productRepositoryMock
                .Setup(pr => pr.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);
            unitOfWorkMock.Setup(u => u.GetRepository<Product>()).Returns(productRepositoryMock.Object);
            userContextServiceMock.Setup(uc => uc.GetUserContext()).Returns(new UserContext(userId, "TestUser", DateTime.UtcNow.ToString()));

            var sut = new AddToCartCommandHandler(unitOfWorkMock.Object, mapperMock.Object, userContextServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await sut.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_InsufficientStock_ShouldReturnFailureResult()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userContextServiceMock = new Mock<IUserContextService>();
            var mapperMock = new Mock<IMapper>();
            var productRepositoryMock = new Mock<IRepositoryAsync<Product>>();
            var cartRepositoryMock = new Mock<IRepositoryAsync<Cart>>();

            var userId = Guid.NewGuid().ToString();
            var productId = 1;
            var productStockQuantity = 10;

            var command = new AddToCartCommand
            {
                ProductId = productId,
                Quantity = 15 // Greater than available stock
            };

            var product = new Product
            {
                Id = productId,
                Name = "TestProduct",
                Price = 10.0m,
                StockQuantity = productStockQuantity
            };

            var cart = new Cart(Guid.Parse(userId));            

            productRepositoryMock
                .Setup(pr => pr.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);
            cartRepositoryMock
                .Setup(pr => pr.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);
            unitOfWorkMock.Setup(u => u.GetRepository<Product>()).Returns(productRepositoryMock.Object);
            unitOfWorkMock.Setup(u => u.GetRepository<Cart>()).Returns(cartRepositoryMock.Object);
            userContextServiceMock.Setup(uc => uc.GetUserContext()).Returns(new UserContext(userId, "TestUser", DateTime.UtcNow.ToString()));

            var sut = new AddToCartCommandHandler(unitOfWorkMock.Object, mapperMock.Object, userContextServiceMock.Object);

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void AddToCartCommandValidator_ValidCommand_ShouldBeValid()
        {
            // Arrange
            var validator = new AddToCartCommandValidator();
            var command = new AddToCartCommand
            {
                ProductId = 1,
                Quantity = 5
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(0, 5)]
        [InlineData(1, 0)]
        [InlineData(0, 0)]
        [InlineData(-1, 5)]
        [InlineData(1, -5)]
        [InlineData(-1, -5)]
        public void AddToCartCommandValidator_InvalidCommand_ShouldBeInvalid(int productId, int quantity)
        {
            // Arrange
            var validator = new AddToCartCommandValidator();
            var command = new AddToCartCommand
            {
                ProductId = productId,
                Quantity = quantity
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
        }
    }
}
