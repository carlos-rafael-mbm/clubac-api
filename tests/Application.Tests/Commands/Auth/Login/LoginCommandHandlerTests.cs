using ClubApi.Application.Commands.Auth.Login;
using ClubApi.Application.Configurations;
using ClubApi.Application.Constants.Validations;
using ClubApi.Domain.Abstractions;
using ClubApi.Domain.Entities;
using FluentAssertions;
using Moq;

namespace ClubApi.Application.Tests.Commands.Auth.Login;

public class LoginCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly JwtConfiguration _jwtConfig;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _jwtConfig = new JwtConfiguration
        {
            Secret = "SuperSecureSecretForTestingPurposes123!",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            AccessTokenExpirationMinutes = 15,
            RefreshTokenExpirationDays = 7
        };

        _handler = new LoginCommandHandler(_unitOfWorkMock.Object, _jwtConfig);
    }

    [Fact]
    public async Task Handle_Should_Return_LoginDto_When_Credentials_Are_Valid()
    {
        // Arrange
        var user = User.CreateTest(1, "testuser", "Test User", "test@test.com", "hashedpassword", 1, Role.CreateTest(1, "Admin"));
        _unitOfWorkMock.Setup(u => u.Users.GetByEmailOrUsernameAsync("testuser", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(new LoginCommand
        {
            EmailUsername = "testuser",
            Password = "hashedpassword"
        }, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeNullOrWhiteSpace();
        result.RefreshToken.Should().NotBeNullOrWhiteSpace();
        result.Username.Should().Be(user.Username);
    }

    [Fact]
    public async Task Handle_Should_Throw_UnauthorizedAccessException_When_User_Not_Found()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.Users.GetByEmailOrUsernameAsync("invaliduser", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(new LoginCommand
        {
            EmailUsername = "invaliduser",
            Password = "password"
        }, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage(ValidationMessages.INVALID_CREDENTIALS);
    }

    [Fact]
    public async Task Handle_Should_Throw_UnauthorizedAccessException_When_Password_Is_Invalid()
    {
        // Arrange
        var user = User.CreateTest(1, "testuser", "Test User", "test@test.com", "hashedpassword", 1, Role.CreateTest(1, "Admin"));
        _unitOfWorkMock.Setup(u => u.Users.GetByEmailOrUsernameAsync("testuser", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        Func<Task> act = async () => await _handler.Handle(new LoginCommand
        {
            EmailUsername = "testuser",
            Password = "wrongpassword"
        }, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage(ValidationMessages.INVALID_CREDENTIALS);
    }

    [Fact]
    public async Task Handle_Should_Save_RefreshToken_When_Credentials_Are_Valid()
    {
        // Arrange
        var user = User.CreateTest(1, "testuser", "Test User", "test@test.com", "hashedpassword", 1, Role.CreateTest(1, "Admin"));
        _unitOfWorkMock.Setup(u => u.Users.GetByEmailOrUsernameAsync("testuser", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _unitOfWorkMock.Setup(u => u.Users.AddRefreshTokenAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(new LoginCommand
        {
            EmailUsername = "testuser",
            Password = "hashedpassword"
        }, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(u => u.Users.AddRefreshTokenAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
