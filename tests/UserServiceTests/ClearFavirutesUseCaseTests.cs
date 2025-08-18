using Microsoft.Extensions.Logging;
using NSubstitute;
using UserService.Application.Contracts;
using UserService.Application.Interfaces;
using UserService.Application.UseCases;

namespace UserServiceTests
{
    [TestFixture]
    public class ClearFavoritesUseCaseTests
    {
        private IFavoritesRepository _repo;
        private ILogger<ClearFavoritesUseCase> _logger;
        private ClearFavoritesUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _repo = Substitute.For<IFavoritesRepository>();
            _logger = Substitute.For<ILogger<ClearFavoritesUseCase>>();
            _useCase = new ClearFavoritesUseCase(_repo, _logger);
        }

        [TestCase(0)]
        [TestCase(-3)]
        public void Handle_InvalidUserId_ThrowsArgumentException(int userId)
        {
            var command = new ClearFavoritesCommand(userId);
            Assert.ThrowsAsync<ArgumentException>(() => _useCase.Handle(command));
        }

        [Test]
        public async Task Handle_ValidUserId_CallsRepository()
        {
            _repo.ClearAsync(2, Arg.Any<CancellationToken>()).Returns(5);
            var command = new ClearFavoritesCommand(2);

            var result = await _useCase.Handle(command);

            Assert.That(result, Is.EqualTo(5));
            await _repo.Received(1).ClearAsync(2, Arg.Any<CancellationToken>());
        }
    }
}