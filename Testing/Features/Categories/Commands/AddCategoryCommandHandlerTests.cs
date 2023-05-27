using Application.Features.Categories.Commands.AddCategoryCommand;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Features.Categories.Commands
{
    public class AddCategoryCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCommand_ShouldAddCategoryToRepository()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var repositoryMock = new Mock<IRepositoryAsync<Category>>();
            var mapperMock = new Mock<IMapper>();

            var command = new AddCategoryCommand
            {
                Name = "TestCategory"
            };

            var category = new Category
            {
                Id = 1,
                Name = command.Name
            };

            repositoryMock.Setup(r => r.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
                .Callback<Category, CancellationToken>((entity, cancellationToken) => category = entity)
                .ReturnsAsync(category);

            unitOfWorkMock.Setup(uow => uow.GetRepository<Category>())
                .Returns(repositoryMock.Object);

            mapperMock.Setup(mapper => mapper.Map<Category>(It.IsAny<AddCategoryCommand>()))
                .Returns(category);

            var sut = new AddCategoryCommandHandler(unitOfWorkMock.Object, mapperMock.Object);

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(category.Id);
            category.Name.Should().Be(command.Name);
            repositoryMock.Verify(r => r.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
            unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }
    }
}
