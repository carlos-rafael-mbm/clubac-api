using ClubApi.Application.Commands.AccessLogs.Exit;
using ClubApi.Application.Constants.Validations;
using ClubApi.Domain.Abstractions;
using ClubApi.Domain.Entities;
using FluentAssertions;
using Moq;

namespace ClubApi.Application.Tests.Commands.AccessLogs;

public class RegisterExitCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly RegisterExitCommandHandler _handler;

    public RegisterExitCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new RegisterExitCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Register_Exit_When_Entry_Exists()
    {
        // Arrange
        var accessLog = AccessLog.RegisterTest(1, 1, DateTime.UtcNow.AddHours(-1), null);
        _unitOfWorkMock.Setup(u => u.Repository<AccessLog, long>().GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(accessLog);

        var command = new RegisterExitCommand
        {
            AccessLogId = 1,
            ExitTime = DateTime.UtcNow
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.ExitTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_AccessLog_Not_Found()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.Repository<AccessLog, long>().GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AccessLog)null);

        var command = new RegisterExitCommand
        {
            AccessLogId = 1,
            ExitTime = DateTime.UtcNow
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage(string.Format(ValidationMessages.NOT_FOUND, "Log de Acceso", command.AccessLogId));
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Exit_Already_Registered()
    {
        // Arrange
        var accessLog = AccessLog.RegisterTest(1, 1, DateTime.UtcNow.AddHours(-1), DateTime.UtcNow.AddMinutes(-30));
        _unitOfWorkMock.Setup(u => u.Repository<AccessLog, long>().GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(accessLog);

        var command = new RegisterExitCommand
        {
            AccessLogId = 1,
            ExitTime = DateTime.UtcNow
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(ValidationMessages.EXIT_ALREADY_REGISTERED);
    }
}