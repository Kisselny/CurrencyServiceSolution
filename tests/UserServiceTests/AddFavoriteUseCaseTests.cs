using Microsoft.Extensions.Logging;
using NSubstitute;
using UserService.Application.Contracts;
using UserService.Application.Interfaces;
using UserService.Application.UseCases;

namespace UserServiceTests
{
    [TestFixture]
    public class AddFavoriteUseCaseTests
    {
        private IFavoritesRepository _repo;
        private ILogger<AddFavoriteUseCase> _logger;
        private AddFavoriteUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _repo = Substitute.For<IFavoritesRepository>();
            _logger = Substitute.For<ILogger<AddFavoriteUseCase>>();
            _useCase = new AddFavoriteUseCase(_repo, _logger);
        }

        [TestCase(0, "usd", TestName = "UserId_Zero_ShouldThrow")]
        [TestCase(-15, "eur", TestName = "UserId_Negative_ShouldThrow")]
        public void Handle_InvalidUserId_ThrowsArgumentException(int userId, string code)
        {
            var command = new AddFavoriteCommand(userId, code);
            Assert.ThrowsAsync<ArgumentException>(() => _useCase.Handle(command));
        }

        [TestCase(1, "   ", TestName = "CurrencyCode_Whitespaces_ShouldThrow")]
        [TestCase(1, "", TestName = "CurrencyCode_Empty_ShouldThrow")]
        public void Handle_EmptyCurrencyCode_ThrowsArgumentException(int userId, string code)
        {
            var command = new AddFavoriteCommand(userId, code);
            Assert.ThrowsAsync<ArgumentException>(() => _useCase.Handle(command));
        }

        [Test]
        public async Task Handle_ValidInput_AddsFavorite()
        {
            var command = new AddFavoriteCommand(1, "usd");
            _repo.ExistsAsync(1, "USD", Arg.Any<CancellationToken>()).Returns(false);

            await _useCase.Handle(command);

            await _repo.Received(1).AddAsync(1, "USD", Arg.Any<CancellationToken>());
        }
    }
}