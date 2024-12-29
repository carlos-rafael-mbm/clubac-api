using ClubApi.Application.Commands.AccessLogs.Entry;
using ClubApi.Application.Constants.Validations;
using ClubApi.Domain.Abstractions;
using ClubApi.Domain.Entities;
using FluentAssertions;
using Moq;

namespace ClubApi.Application.Tests.Commands.AccessLogs;

public class RegisterEntryCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly RegisterEntryCommandHandler _handler;

    public RegisterEntryCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new RegisterEntryCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_Entry_When_Client_Exists()
    {
        // Arrange
        var client = Client.CreateTest(1, "John Doe", 1, "john.doe@example.com", "123456789", ClientType.CreateTest(1, "Member"));
        _unitOfWorkMock.Setup(u => u.Repository<Client, int>().GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(client);

        _unitOfWorkMock.Setup(u => u.Repository<AccessLog, long>().AddAsync(It.IsAny<AccessLog>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccessLog.RegisterEntry(1, DateTime.UtcNow));

        var command = new RegisterEntryCommand
        {
            ClientId = 1,
            EntryTime = DateTime.UtcNow
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.ClientId.Should().Be(1);
        result.EntryTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Client_Not_Found()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.Repository<Client, int>().GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Client)null);

        var command = new RegisterEntryCommand
        {
            ClientId = 1,
            EntryTime = DateTime.UtcNow
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage(string.Format(ValidationMessages.NOT_FOUND, "Cliente", command.ClientId));
    }
}