using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TiendaAlvaro.Controllers;
using TiendaAlvaro.Models;

namespace Tests
{
    public class Tests
    {
        private static readonly IConfiguration _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

        public Tests()
        {
            _config.GetSection("ConnectionStrings")["SqlConnection"] = "server=Fabrica1\\SQLEXPRESS;initial catalog=TIENDA1;integrated security=true";
        }

        [Fact]
        public void TestingBooks()
        {
            var controller = new BookController(_config);
            var result = controller.Index();
            var assert0 = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<List<Book>>(assert0.Value);
        }

        [Fact]
        public void TestingUsers()
        {
            var controller = new UserController(_config);
            var createResult = controller.Create(new User(1, "test"));

            var assert = Assert.IsType<OkObjectResult>(createResult);
            Assert.Equal("User created", assert.Value);
            var assert1 = controller.Get(1);
            var assert2 = Assert.IsType<OkObjectResult>(assert1);
            var assert3 = Assert.IsType<User>(assert2.Value);
            Assert.Equal("test", assert3.Name);

            var deleteResult = controller.Delete(1);
            var assert4 = Assert.IsType<OkObjectResult>(deleteResult);
            var assert5 = Assert.IsType<string>(assert4.Value);
            Assert.Equal("User deleted", assert5);

            var listResult = controller.Index();
            var assert6 = Assert.IsType<OkObjectResult>(listResult);
            var assert7 = Assert.IsType<List<User>>(assert6.Value);
            Assert.Empty(assert7);

        }
    }
}