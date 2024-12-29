using ClubApi.Application.Commands.Users.RegisterUser;
using ClubApi.Application.Constants.Validations;
using ClubApi.Domain.Abstractions;
using ClubApi.Domain.Entities;
using FluentAssertions;
using Moq;

namespace ClubApi.Application.Tests.Commands.Users;

public class RegisterUserCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly RegisterUserCommandHandler _handler;

    public RegisterUserCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new RegisterUserCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_User_When_Valid_Data()
    {
        // Arrange
        var role = Role.CreateTest(1, "Admin");
        _unitOfWorkMock.Setup(u => u.Repository<Role, int>().GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);

        _unitOfWorkMock.Setup(u => u.Repository<User, int>().AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(User.CreateTest(1, "testuser", "Test User", "test@test.com", "password", 1, role));

        var command = new RegisterUserCommand
        {
            Username = "testuser",
            Name = "Test User",
            Email = "test@test.com",
            Password = "password",
            RoleId = 1
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Username.Should().Be("testuser");
        result.Role.Id.Should().Be(1);
        result.Role.Name.Should().Be("Admin");

        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Role_Not_Found()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.Repository<Role, int>().GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Role)null);

        var command = new RegisterUserCommand
        {
            Username = "testuser",
            Name = "Test User",
            Email = "test@test.com",
            Password = "password",
            RoleId = 1
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage(string.Format(ValidationMessages.NOT_FOUND, "Rol", 1));
    }
}
