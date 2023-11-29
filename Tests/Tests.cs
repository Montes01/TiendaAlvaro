using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Timers;
using TiendaAlvaro.Controllers;
using TiendaAlvaro.Models;

namespace Tests
{
    public class Tests
    {
        private static readonly IConfiguration _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

        private readonly BookController _bookController;
        private readonly UserController _userController;
        private readonly CartController _cartController;

        public Tests()
        {
            _config.GetSection("ConnectionStrings")["SqlConnection"] = "server=DESKTOP-JIJLT7T;initial catalog=TIENDA1;integrated security=true";
            _bookController = new BookController(_config);
            _userController = new UserController(_config);
            _cartController = new CartController(_config);
        }

        [Fact]
        public void TestingBooks()
        {
            //libro a usar para las pruebas
            Book book = new(0, "test", 1, 1);
            //creamos el libro
            var createResult = _bookController.Create(book);
            var assert0 = Assert.IsType<OkObjectResult>(createResult);
            Assert.Equal("Book created", assert0.Value);

            //obtenemos la lista de libros
            var listResult = _bookController.Index();
            var assert1 = Assert.IsType<OkObjectResult>(listResult);
            var assert2 = Assert.IsType<List<Book>>(assert1.Value);
            
            //validamos que el ultimo libro en la base de datos sea el que recien se creo
            var Detailsresult = _bookController.Details(assert2[^1].Id);
            var assert3 = Assert.IsType<OkObjectResult>(Detailsresult);
            var assert4 = Assert.IsType<Book>(assert3.Value);
            Assert.Equal(book.Name, assert4.Name);

            //cambiamos el nombre del libro
            book.Name = "changedTest";
            book.Id = assert4.Id;
            var updateResult = _bookController.Update(book);
            var assert5 = Assert.IsType<OkObjectResult>(updateResult);
            Assert.Equal("Book updated", assert5.Value);

            //validamos que el libro se haya actualizado
            var Details2Result = _bookController.Details(assert4.Id);
            var assert6 = Assert.IsType<OkObjectResult>(Details2Result);
            var assert7 = Assert.IsType<Book>(assert6.Value);
            Assert.Equal(book.Name, assert7.Name);

            // eliminamos el libro
            var DeleteResult = _bookController.Delete(assert7.Id);
            var assert8 = Assert.IsType<OkObjectResult>(DeleteResult);
            var assert9 = Assert.IsType<string>(assert8.Value);
            Assert.Equal("Book deleted", assert9);

            //validamos que el libro ya no exista
            var list2Result = _bookController.Index();
            var assert10 = Assert.IsType<OkObjectResult>(list2Result);
            var assert11 = Assert.IsType<List<Book>>(assert10.Value);
            Assert.DoesNotContain(book, assert11);

        }   

        [Fact]
        public void TestingUsers()
        {
            //usuario con id random a crear
            User user = new(new Random().Next(), "test");
            //creamos usuario
            var createResult = _userController.Create(user);
            var assert = Assert.IsType<OkObjectResult>(createResult);
            Assert.Equal("User created", assert.Value);

            //validamos que el usuario se haya creado
            var assert1 = _userController.Get(user.Id);
            var assert2 = Assert.IsType<OkObjectResult>(assert1);
            var assert3 = Assert.IsType<User>(assert2.Value);
            Assert.Equal(user.Name, assert3.Name);

            //eliminamos usuario
            var deleteResult = _userController.Delete(user.Id);
            var assert4 = Assert.IsType<OkObjectResult>(deleteResult);
            var assert5 = Assert.IsType<string>(assert4.Value);
            Assert.Equal("User deleted", assert5);

            //validamos que el usuario se haya eliminado
            var listResult = _userController.Index();
            var assert6 = Assert.IsType<OkObjectResult>(listResult);
            var assert7 = Assert.IsType<List<User>>(assert6.Value);
            Assert.DoesNotContain(user, assert7);

        }

        [Fact]
        public void TestingItems()
        {
            //usuario con id random a crear
            User user = new (new Random().Next(), "test");
            //3 libros a usar
            Book book1 = new (0, "test1", 1, 1);
            Book book2 = new (0, "test2", 1, 1);
            Book book3 = new (0, "test3", 1, 1);

            //limpiar carrito por si acaso
            var clearResult = _cartController.ClearCart(user.Id);
            Assert.IsType<OkObjectResult>(clearResult);

            //crear usuario
            var createResult = _userController.Create(user);
            Assert.IsType<OkObjectResult>(createResult);
            
            //crear 2 libros
            var createResult1 = _bookController.Create(book1);
            Assert.IsType<OkObjectResult>(createResult1);

            var createResult2 = _bookController.Create(book2);
            Assert.IsType<OkObjectResult>(createResult2);

            //obtener id de los libros creados (debido a que es autoincremental)
            var listResult = _bookController.Index();
            var IsListBookOk = Assert.IsType<OkObjectResult>(listResult);
            var list = Assert.IsType<List<Book>>(IsListBookOk.Value);
            Assert.Equal(book1.Name, list[^2].Name);
            Assert.Equal(book2.Name, list[^1].Name);
            var id1 = list[^2].Id;
            var id2 = list[^1].Id;

            //añadir libro 1 al carrito
            Item item = new (user.Id, id1, 1);
            var addResult = _cartController.AddItem(item);
            Assert.IsType<OkObjectResult>(addResult);

            //validar longitud del carrito
            var carListResult = _cartController.SeeCart(user.Id);
            var IsCartOk = Assert.IsType<OkObjectResult>(carListResult);
            var carList = Assert.IsType<List<CartResponse>>(IsCartOk.Value);
            Assert.Single(carList);

            //añadir libro 2 al carrito
            Item item2 = new (user.Id, id2, 1);
            var addResult2 = _cartController.AddItem(item2);
            Assert.IsType<OkObjectResult>(addResult2);

            //validar longitud del carrito
            var carListResult2 = _cartController.SeeCart(user.Id);
            var IsCartOk2 = Assert.IsType<OkObjectResult>(carListResult2);
            var carList2 = Assert.IsType<List<CartResponse>>(IsCartOk2.Value);
            Assert.Equal(2, carList2.Count);

            //actualizar cantidad del libro 1
            item.Quantity = 50;
            var updateResult = _cartController.UpdateItem(item);
            Assert.IsType<OkObjectResult>(updateResult);

            //validar cantidad del libro 1
            var carListResult3 = _cartController.SeeCart(user.Id);
            var IsCartOk3 = Assert.IsType<OkObjectResult>(carListResult3);
            var carList3 = Assert.IsType<List<CartResponse>>(IsCartOk3.Value);
            Assert.Equal(50, carList3[0].Quantity);

            //crear tercer libro
            var createResult3 = _bookController.Create(book3);
            Assert.IsType<OkObjectResult>(createResult3);

            //obtener id del ultimo libro y validar si su nombre es el mismo
            var listResult2 = _bookController.Index();
            var IsListBookOk2 = Assert.IsType<OkObjectResult>(listResult2);
            var list2 = Assert.IsType<List<Book>>(IsListBookOk2.Value);
            Assert.Equal(book3.Name, list2[^1].Name);
            var id3 = list2[^1].Id;

            //añadir libro 3 al carrito
            Item item3 = new (user.Id, id3, 1);
            var addResult3 = _cartController.AddItem(item3);    
            Assert.IsType<OkObjectResult>(addResult3);

            //eliminar libro 1
            var removeResult = _cartController.RemoveItem(item);
            Assert.IsType<OkObjectResult>(removeResult);
            //validar longitud del carrito
            var carListResult4 = _cartController.SeeCart(user.Id);
            var IsCartOk4 = Assert.IsType<OkObjectResult>(carListResult4);
            var carList4 = Assert.IsType<List<CartResponse>>(IsCartOk4.Value);
            Assert.Equal(2, carList4.Count);
            //limpiar carrito
            var clearResult2 = _cartController.ClearCart(user.Id);
            Assert.IsType<OkObjectResult>(clearResult2);

            //validar longitud del carrito
            var carListResult5 = _cartController.SeeCart(user.Id);
            var IsCartOk5 = Assert.IsType<OkObjectResult>(carListResult5);
            var carList5 = Assert.IsType<List<CartResponse>>(IsCartOk5.Value);
            Assert.Empty(carList5);

            //eliminar usuario creado
            var deleteResult = _userController.Delete(user.Id);
            Assert.IsType<OkObjectResult>(deleteResult);

            //eliminar los 3 libros creados
            var deleteResult1 = _bookController.Delete(id1);
            Assert.IsType<OkObjectResult>(deleteResult1);
            var deleteResult2 = _bookController.Delete(id2);
            Assert.IsType<OkObjectResult>(deleteResult2);
            var deleteResult3 = _bookController.Delete(id3);
            Assert.IsType<OkObjectResult>(deleteResult3);
        }
    }
}