using ClubApi.Application.Commands.Users.DeleteUser;
using ClubApi.Application.Constants.Validations;
using ClubApi.Domain.Abstractions;
using ClubApi.Domain.Entities;
using FluentAssertions;
using Moq;

namespace ClubApi.Application.Tests.Commands.Users;

public class DeleteUserCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteUserCommandHandler _handler;

    public DeleteUserCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new DeleteUserCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Delete_User_When_Valid_Id()
    {
        // Arrange
        var user = User.CreateTest(1, "testuser", "Test User", "test@test.com", "password", 1, Role.CreateTest(1, "Admin"));

        _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(new DeleteUserCommand { Id = 1 }, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_User_Not_Found()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(new DeleteUserCommand { Id = 1 }, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage(string.Format(ValidationMessages.NOT_FOUND, "Usuario", 1));
    }
}
