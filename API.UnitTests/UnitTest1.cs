using API.Controllers;
using API.DTOs;
using API.Interfaces;
using Entities;
using Moq;
using NUnit.Framework;

namespace API.UnitTests
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void AccountController_ValidUser_ReturnValidDto()
        {

          int expectedID = 1010;
          string expectedUserName = "Expected Username";
          string expectedToken = "euasnetuhsaontheusnaothuaoeuaoeuaoeuaoeuaoeu";

          AppUser user = new AppUser
          {
            Id = expectedID,
            UserName = expectedUserName
          };

          var fakeTokenService = new Mock<ITokenService>();
          fakeTokenService.Setup(x => x.CreateToken(It.IsAny<AppUser>())).Returns(expectedToken);

          AccountController ac = new AccountController(null, fakeTokenService.Object);

          UserDto actualUserDto = ac.createUserDto(user);          


          Assert.AreEqual(expectedUserName, actualUserDto.Username, "ID's should be equal");
          Assert.AreEqual(expectedToken, actualUserDto.Token);
          Assert.IsTrue(false);
            
        }
    }
}