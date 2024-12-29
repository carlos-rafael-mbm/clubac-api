using ClubApi.Application.Commands.Users.UpdateUser;
using ClubApi.Application.Constants.Validations;
using ClubApi.Domain.Abstractions;
using ClubApi.Domain.Entities;
using FluentAssertions;
using Moq;

namespace ClubApi.Application.Tests.Commands.Users;

public class UpdateUserCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateUserCommandHandler _handler;

    public UpdateUserCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new UpdateUserCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Update_User_When_Valid_Data()
    {
        // Arrange
        var user = User.CreateTest(1, "olduser", "Old User", "old@test.com", "oldpassword", 1, Role.CreateTest(1, "Admin"));

        _unitOfWorkMock.Setup(u => u.Users.GetUserByIdWithRoleAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var command = new UpdateUserCommand
        {
            Id = 1,
            Username = "newuser",
            Name = "New User",
            Email = "new@test.com",
            Password = "newpassword"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Username.Should().Be("newuser");
        result.Name.Should().Be("New User");
        result.Email.Should().Be("new@test.com");

        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_User_Not_Found()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.Users.GetUserByIdWithRoleAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        var command = new UpdateUserCommand
        {
            Id = 1,
            Username = "newuser",
            Name = "New User",
            Email = "new@test.com",
            Password = "newpassword"
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage(string.Format(ValidationMessages.NOT_FOUND, "Usuario", 1));
    }
}
