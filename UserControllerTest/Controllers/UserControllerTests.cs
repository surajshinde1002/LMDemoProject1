using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserController.Controllers;
using UserController.data;
using UserController.Models.EntityModel;
using UserController.Models.RequestModel;
using UserController.Models.ResponseModel;
using UserController.Services;

namespace UserControllerTest.Controllers
{
 
    public class UserControllerTests
    {

        private DbContextOptions<UserDbContext> _options;

        public UserControllerTests()
        {
            _options = new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task Register_ValidModel_ReturnsOkResult()
        {
            // Arrange
            using (var context = new UserDbContext(_options))
            {
                var mockUserServices = new Mock<IUserServices>();
                mockUserServices.Setup(s => s.CreateUser(It.IsAny<RegisterModel>())).ReturnsAsync(new RegisterResponseModel());

                var mockConfiguration = new Mock<IConfiguration>();



                var controller = new UserControllers(mockUserServices.Object, mockConfiguration.Object, context);

                var objectValidator = new Mock<IObjectModelValidator>();
                objectValidator.Setup(o => o.Validate(
                    It.IsAny<ActionContext>(),
                    It.IsAny<ValidationStateDictionary>(),
                    It.IsAny<string>(),
                    It.IsAny<object>()));

                controller.ObjectValidator = objectValidator.Object;

                var model = new RegisterModel
                {
                    Username = "test",
                    Email = "test12example.com",
                    Password = "password",
                    Standard = 7,
                    Roll = 32,
                    DOB = DateOnly.FromDateTime(new DateTime())
                };

                // Act
                var result = await controller.Register(model);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.NotNull(okResult.Value);
            }
        }


        [Fact]
        public async Task Register_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            using (var context = new UserDbContext(_options))
            {
                var mockUserServices = new Mock<IUserServices>();
                var mockConfiguration = new Mock<IConfiguration>();

                var controller = new UserControllers(mockUserServices.Object, mockConfiguration.Object, context);

                var objectValidator = new Mock<IObjectModelValidator>();
                objectValidator.Setup(o => o.Validate(
                    It.IsAny<ActionContext>(),
                    It.IsAny<ValidationStateDictionary>(),
                    It.IsAny<string>(),
                    It.IsAny<object>()));

                controller.ObjectValidator = objectValidator.Object;

                controller.ModelState.AddModelError("key", "error message");
                var model = new RegisterModel();

                // Act
                var result = await controller.Register(model);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsAssignableFrom<SerializableError>(badRequestResult.Value);
            }
        }

        [Fact]
        public async Task Register_ExistingUsername_ReturnsBadRequest()
        {
            // Arrange
            using (var context = new UserDbContext(_options))
            {
                var existingUser = new User
                {
                    Username = "existingUser",
                    Email = "existing@example.com",
                    Password = "password",
                    Role = "User",
                    Standard = 7,
                    Roll = 32,
                    DOB = DateOnly.FromDateTime(new DateTime()),
                    Flag = true
                };
                context.Users.Add(existingUser);
                context.SaveChanges();

                var mockUserServices = new Mock<IUserServices>();
                var mockConfiguration = new Mock<IConfiguration>();

                var controller = new UserControllers(mockUserServices.Object, mockConfiguration.Object, context);

                var objectValidator = new Mock<IObjectModelValidator>();
                objectValidator.Setup(o => o.Validate(
                    It.IsAny<ActionContext>(),
                    It.IsAny<ValidationStateDictionary>(),
                    It.IsAny<string>(),
                    It.IsAny<object>()));

                controller.ObjectValidator = objectValidator.Object;

                var model = new RegisterModel
                {
                    Username = "existingUser", // Existing username
                    Email = "new@example.com",
                    Password = "password",
                    Standard = 7,
                    Roll = 32,
                };

                // Act
                var result = await controller.Register(model);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Username already exists, try giving a different username.", badRequestResult.Value);
            }
        }

        [Fact]
        public async Task Register_ExistingEmail_ReturnsBadRequest()
        {
            // Arrange
            using (var context = new UserDbContext(_options))
            {
                var existingUser = new User
                {
                    Username = "existingUser",
                    Email = "existing@example.com",
                    Password = "password",
                    Role = "User",
                    Standard = 7,
                    Roll = 32,
                    DOB = DateOnly.FromDateTime(new DateTime()),
                    Flag = true
                };
                context.Users.Add(existingUser);
                context.SaveChanges();

                var mockUserServices = new Mock<IUserServices>();
                var mockConfiguration = new Mock<IConfiguration>();

                var controller = new UserControllers(mockUserServices.Object, mockConfiguration.Object, context);

                var objectValidator = new Mock<IObjectModelValidator>();
                objectValidator.Setup(o => o.Validate(
                    It.IsAny<ActionContext>(),
                    It.IsAny<ValidationStateDictionary>(),
                    It.IsAny<string>(),
                    It.IsAny<object>()));

                controller.ObjectValidator = objectValidator.Object;

                var model = new RegisterModel
                {
                    Username = "newUser",
                    Email = "existing@example.com", // Existing email
                    Password = "password",
                    Standard = 7,
                    Roll = 32,
                };

                // Act
                var result = await controller.Register(model);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Email already exists, try giving another email.", badRequestResult.Value);
            }
        }



    }
}
