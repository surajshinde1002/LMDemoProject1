using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using TeacherWebApplication.Data;
using TeacherWebApplication.Services;
using TeacherWebApplication.Models.RequestModels;
using TeacherWebApplication.Models.ResponseModels;
using Microsoft.Extensions.Configuration;
using TeacherWebApplication.Controllers;
using TeacherWebApplication.Models.EntityModels;

namespace TeacherTest
{
    public class TeacherControllerTest
    {
        private DbContextOptions<TeacherDbContext> _options;

        public TeacherControllerTest()
        {
            _options = new DbContextOptionsBuilder<TeacherDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task Register_ValidModel_ReturnsOkResult()
        {
            // Arrange
            using (var context = new TeacherDbContext(_options))
            {
                var mockUserServices = new Mock<ITeacherService>();
                mockUserServices.Setup(s => s.CreateTeacher(It.IsAny<TeacherRegisterModel>())).ReturnsAsync(new TeacherResponseModel());

                var mockConfiguration = new Mock<IConfiguration>();



                var controller = new HomeController(mockUserServices.Object, mockConfiguration.Object, context);

                var objectValidator = new Mock<IObjectModelValidator>();
                objectValidator.Setup(o => o.Validate(
                    It.IsAny<ActionContext>(),
                    It.IsAny<ValidationStateDictionary>(),
                    It.IsAny<string>(),
                    It.IsAny<object>()));

                controller.ObjectValidator = objectValidator.Object;

                var model = new TeacherRegisterModel
                {
                    Name = "test",
                    Email = "test12example.com",
                    Password = "password",
                    Standard = 7,
                    PhoneNumber = "1234567890",
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
            using (var context = new TeacherDbContext(_options))
            {
                var mockUserServices = new Mock<ITeacherService>();
                var mockConfiguration = new Mock<IConfiguration>();

                var controller = new HomeController(mockUserServices.Object, mockConfiguration.Object, context);

                var objectValidator = new Mock<IObjectModelValidator>();
                objectValidator.Setup(o => o.Validate(
                    It.IsAny<ActionContext>(),
                    It.IsAny<ValidationStateDictionary>(),
                    It.IsAny<string>(),
                    It.IsAny<object>()));

                controller.ObjectValidator = objectValidator.Object;

                controller.ModelState.AddModelError("key", "error message");
                var model = new TeacherRegisterModel();

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
            using (var context = new TeacherDbContext(_options))
            {
                var existingUser = new Teacher
                {
                    Name = "existingUser",
                    Email = "existing@example.com",
                    Password = "password",
                    Role = "User",
                    Standard = 7,
                   PhoneNumber = "1234567891",
                };
                context.TeacherTable.Add(existingUser);
                context.SaveChanges();

                var mockUserServices = new Mock<ITeacherService>();
                var mockConfiguration = new Mock<IConfiguration>();

                var controller = new HomeController(mockUserServices.Object, mockConfiguration.Object, context);

                var objectValidator = new Mock<IObjectModelValidator>();
                objectValidator.Setup(o => o.Validate(
                    It.IsAny<ActionContext>(),
                    It.IsAny<ValidationStateDictionary>(),
                    It.IsAny<string>(),
                    It.IsAny<object>()));

                controller.ObjectValidator = objectValidator.Object;

                var model = new TeacherRegisterModel
                {
                    Name = "existingUser", // Existing username
                    Email = "new@example.com",
                    Password = "password",
                    Standard = 7,
                    PhoneNumber = "3892847892",
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
            using (var context = new TeacherDbContext(_options))
            {
                var existingUser = new Teacher
                {
                    Name = "existingUser",
                    Email = "existing@example.com",
                    Password = "password",
                    Role = "User",
                    Standard = 7,
                  PhoneNumber = "2387548943"
                };
                context.TeacherTable.Add(existingUser);
                context.SaveChanges();

                var mockUserServices = new Mock<ITeacherService>();
                var mockConfiguration = new Mock<IConfiguration>();

                var controller = new HomeController(mockUserServices.Object, mockConfiguration.Object, context);

                var objectValidator = new Mock<IObjectModelValidator>();
                objectValidator.Setup(o => o.Validate(
                    It.IsAny<ActionContext>(),
                    It.IsAny<ValidationStateDictionary>(),
                    It.IsAny<string>(),
                    It.IsAny<object>()));

                controller.ObjectValidator = objectValidator.Object;

                var model = new TeacherRegisterModel
                {
                    Name = "newUser",
                    Email = "existing@example.com", // Existing email
                    Password = "password",
                    Standard = 7,
                   PhoneNumber = "2378346523"
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