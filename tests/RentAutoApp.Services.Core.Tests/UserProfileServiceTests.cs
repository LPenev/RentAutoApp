using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RentAutoApp.Data.Models;
using RentAutoApp.Services.Core.Dtos;

namespace RentAutoApp.Services.Core.Tests.Generated.Extended
{
    [TestFixture]
    public class UserProfileService_ExtendedTests
    {
        private static (Mock<UserManager<ApplicationUser>> um, Mock<SignInManager<ApplicationUser>> sm) CreateIdentity()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(
                store.Object,
                Options.Create(new IdentityOptions()),
                new PasswordHasher<ApplicationUser>(),
                new IUserValidator<ApplicationUser>[0],
                new IPasswordValidator<ApplicationUser>[0],
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object
            );

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var options = Options.Create(new IdentityOptions());
            var logger = new Mock<ILogger<SignInManager<ApplicationUser>>>();
            var schemes = new Mock<Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider>();
            var confirmation = new Mock<IUserConfirmation<ApplicationUser>>();

            var signInManager = new Mock<SignInManager<ApplicationUser>>(
                userManager.Object,
                contextAccessor.Object,
                claimsFactory.Object,
                options,
                logger.Object,
                schemes.Object,
                confirmation.Object
            );

            return (userManager, signInManager);
        }

        [Test]
        public async Task GetAsync_WhenUserExists_ReturnsDtoWithFields()
        {
            var (um, sm) = CreateIdentity();
            var user = new ApplicationUser { Id = "u1", Email = "john@example.com", FirstName = "John", LastName = "Doe", PhoneNumber = "123" };
            um.Setup(x => x.FindByIdAsync("u1")).ReturnsAsync(user);

            var sut = new UserProfileService(um.Object, sm.Object);
            var res = await sut.GetAsync("u1");
            Assert.That(res.Succeeded, Is.True);
            Assert.That(res.Value, Is.Not.Null);
            Assert.That(res.Value.Email, Is.EqualTo("john@example.com"));
            Assert.That(res.Value.FirstName, Is.EqualTo("John"));
            Assert.That(res.Value.LastName, Is.EqualTo("Doe"));
            Assert.That(res.Value.PhoneNumber, Is.EqualTo("123"));
        }

        [Test]
        public async Task GetAsync_WhenUserMissing_ReturnsFail()
        {
            var (um, sm) = CreateIdentity();
            um.Setup(x => x.FindByIdAsync("ghost")).ReturnsAsync((ApplicationUser)null!);

            var sut = new UserProfileService(um.Object, sm.Object);
            var res = await sut.GetAsync("ghost");

            Assert.That(res.Succeeded, Is.False);
        }

        [Test]
        public async Task UpdateProfileAsync_WhenValid_Should_SetFields_And_UpdateAsyncCalled()
        {
            var (um, sm) = CreateIdentity();
            var user = new ApplicationUser { Id = "u1", Email = "john@example.com", FirstName = "", LastName = "", PhoneNumber = "" };
            um.Setup(x => x.FindByIdAsync("u1")).ReturnsAsync(user);
            um.Setup(x => x.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            var sut = new UserProfileService(um.Object, sm.Object);
            var cmd = new UpdateProfileCommand(FirstName: "John", LastName: "Doe", PhoneNumber: "123");
            var res = await sut.UpdateProfileAsync("u1", cmd);

            Assert.That(res.Succeeded, Is.True);
            Assert.That(user.FirstName, Is.EqualTo("John"));
            Assert.That(user.LastName, Is.EqualTo("Doe"));
            Assert.That(user.PhoneNumber, Is.EqualTo("123"));
            um.Verify(x => x.UpdateAsync(It.Is<ApplicationUser>(u => u.Id == "u1")), Times.Once);
            sm.Verify(x => x.RefreshSignInAsync(user), Times.Once);
        }

        [Test]
        public async Task ChangePasswordAsync_WhenCurrentWrong_Should_Fail()
        {
            var (um, sm) = CreateIdentity();
            var user = new ApplicationUser { Id = "u1" };
            um.Setup(x => x.FindByIdAsync("u1")).ReturnsAsync(user);
            um.Setup(x => x.ChangePasswordAsync(user, "wrong", "NewP@ssw0rd")).ReturnsAsync(IdentityResult.Failed(new IdentityError{ Description = "Bad password" }));

            var sut = new UserProfileService(um.Object, sm.Object);
            var res = await sut.ChangePasswordAsync("u1", "wrong", "NewP@ssw0rd");

            Assert.That(res.Succeeded, Is.False);
            sm.Verify(x => x.RefreshSignInAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [Test]
        public async Task ChangePasswordAsync_WhenValid_Should_Succeed_And_RefreshSignIn()
        {
            var (um, sm) = CreateIdentity();
            var user = new ApplicationUser { Id = "u1" };
            um.Setup(x => x.FindByIdAsync("u1")).ReturnsAsync(user);
            um.Setup(x => x.ChangePasswordAsync(user, "old", "NewP@ssw0rd")).ReturnsAsync(IdentityResult.Success);

            var sut = new UserProfileService(um.Object, sm.Object);
            var res = await sut.ChangePasswordAsync("u1", "old", "NewP@ssw0rd");

            Assert.That(res.Succeeded, Is.True);
            sm.Verify(x => x.RefreshSignInAsync(user), Times.Once);
        }

        [Test]
        public async Task ChangeEmailAsync_WhenInvalidEmail_Should_Fail()
        {
            var (um, sm) = CreateIdentity();
            var user = new ApplicationUser { Id = "u1", Email = "old@example.com", UserName = "old@example.com" };
            um.Setup(x => x.FindByIdAsync("u1")).ReturnsAsync(user);
            um.Setup(x => x.SetEmailAsync(user, "bad")).ReturnsAsync(IdentityResult.Failed(new IdentityError{ Description = "Invalid email" }));

            var sut = new UserProfileService(um.Object, sm.Object);
            var res = await sut.ChangeEmailAsync("u1", "bad", "pwd");

            Assert.That(res.Succeeded, Is.False);
            um.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }

        //[Test]
        //public async Task ChangeEmailAsync_WhenValid_And_UserNameEqualsEmail_Should_UpdateUserName_And_Refresh()
        //{
        //    // Arrange
        //    var (um, sm) = CreateIdentity();
        //    var user = new ApplicationUser
        //    {
        //        Id = "u1",
        //        Email = "old@example.com",
        //        UserName = "old@example.com"
        //    };

        //    um.Setup(x => x.FindByIdAsync("u1")).ReturnsAsync(user);
        //    um.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), "pwd")).ReturnsAsync(true);
        //    um.Setup(x => x.FindByEmailAsync("new@example.com")).ReturnsAsync((ApplicationUser?)null);
        //    um.Setup(x => x.GenerateChangeEmailTokenAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
        //        .ReturnsAsync("token123");
        //    // ToDo
            
        //}
    }
}