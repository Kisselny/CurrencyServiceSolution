using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using UserService.Application.Contracts;
using UserService.Application.Interfaces;
using UserService.Application.UseCases;
using UserService.Domain.Models;

namespace UserServiceTests
{
    [TestFixture]
    public class UserLoginUseCaseTests
    {
        private IUserRepository _userRepo;
        private IJwtTokenGenerator _jwt;
        private ILogger<UserLoginUseCase> _logger;
        private UserLoginUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _userRepo = Substitute.For<IUserRepository>();
            _jwt = Substitute.For<IJwtTokenGenerator>();
            _logger = Substitute.For<ILogger<UserLoginUseCase>>();
            _useCase = new UserLoginUseCase(_userRepo, _jwt, _logger);
        }
        
        [TestCase("")]
        [TestCase("   ")]
        public void ExecuteAsync_EmptyName_ThrowsValidation(string name)
        {
            var cmd = new LoginUserCommand(name, "pass");
            Assert.ThrowsAsync<ValidationException>(() => _useCase.ExecuteAsync(cmd, CancellationToken.None));
        }
        
        [TestCase("")]
        [TestCase("   ")]
        public void ExecuteAsync_EmptyPassword_ThrowsValidation(string password)
        {
            var cmd = new LoginUserCommand("user", password);
            Assert.ThrowsAsync<ValidationException>(() => _useCase.ExecuteAsync(cmd, CancellationToken.None));
        }

        [Test]
        public void ExecuteAsync_UserNotFound_ThrowsException()
        {
            _userRepo.GetByNameAsync("nouser", Arg.Any<CancellationToken>()).Returns((User)null);
            var cmd = new LoginUserCommand("nouser", "pass");
            Assert.ThrowsAsync<System.Exception>(() => _useCase.ExecuteAsync(cmd, CancellationToken.None));
        }

        [Test]
        public void ExecuteAsync_WrongPassword_ThrowsException()
        {
            var user = new User(name: "petya", password: "passwordpassword");
            // user.IsPasswordCorrect("badpass") == false;

            _userRepo.GetByNameAsync("petya", Arg.Any<CancellationToken>()).Returns(user);

            var cmd = new LoginUserCommand("petya", "badpass");

            Assert.ThrowsAsync<System.Exception>(() => _useCase.ExecuteAsync(cmd, CancellationToken.None));
        }

        [Test]
        public async Task ExecuteAsync_CorrectCredentials_ReturnsToken()
        {
            var user = new User(name: "vasya", password: "passwordpassword");
            // user.IsPasswordCorrect("passwordpassword") == true;

            _userRepo.GetByNameAsync("vasya", Arg.Any<CancellationToken>()).Returns(user);
            _jwt.GenerateToken(user.Id, "vasya").Returns("jwt_abc");

            var cmd = new LoginUserCommand("vasya", "passwordpassword");

            var result = await _useCase.ExecuteAsync(cmd, CancellationToken.None);

            Assert.That(result.Token, Is.EqualTo("jwt_abc"));
        }
    }
}