using ClubApi.Application.Commands.Clients.RegisterClient;
using ClubApi.Application.Constants.Validations;
using ClubApi.Domain.Abstractions;
using ClubApi.Domain.Entities;
using FluentAssertions;
using Moq;

namespace ClubApi.Application.Tests.Commands.Clients;

public class RegisterClientCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly RegisterClientCommandHandler _handler;

    public RegisterClientCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new RegisterClientCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_Client_When_Valid_Data()
    {
        // Arrange
        var clientType = ClientType.CreateTest(1, "Member");
        _unitOfWorkMock.Setup(u => u.Repository<ClientType, int>().GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(clientType);

        _unitOfWorkMock.Setup(u => u.Repository<Client, int>().AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Client.CreateTest(1, "John Doe", 1, "johndoe@example.com", "123456789", clientType));

        var command = new RegisterClientCommand
        {
            Name = "John Doe",
            ClientTypeId = 1,
            Email = "johndoe@example.com",
            Phone = "123456789"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("John Doe");
        result.Email.Should().Be("johndoe@example.com");
        result.Phone.Should().Be("123456789");
        result.ClientType.Id.Should().Be(1);
        result.ClientType.Name.Should().Be("Member");

        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_ClientType_Not_Found()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.Repository<ClientType, int>().GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ClientType)null);

        var command = new RegisterClientCommand
        {
            Name = "John Doe",
            ClientTypeId = 1,
            Email = "johndoe@example.com",
            Phone = "123456789"
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage(string.Format(ValidationMessages.NOT_FOUND, "Tipo de cliente", 1));
    }

    [Fact]
    public async Task Handle_Should_Rollback_Transaction_When_Exception_Occurs()
    {
        // Arrange
        var clientType = ClientType.CreateTest(1, "Member");
        _unitOfWorkMock.Setup(u => u.Repository<ClientType, int>().GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(clientType);

        _unitOfWorkMock.Setup(u => u.Repository<Client, int>().AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException(ValidationMessages.INTERNAL_SERVER_ERROR));

        var command = new RegisterClientCommand
        {
            Name = "John Doe",
            ClientTypeId = 1,
            Email = "johndoe@example.com",
            Phone = "123456789"
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(ValidationMessages.INTERNAL_SERVER_ERROR);

        _unitOfWorkMock.Verify(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}