using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using TiendaAlvaro.Models;

namespace TiendaAlvaro.Controllers
{
    [Route("Books")]
    public class BookController : Controller
    {

        private static SqlConnection _conn;
        public BookController(IConfiguration? _config)
        {
            string connectionString = _config.GetConnectionString("SqlConnection");
            _conn = new SqlConnection(connectionString);
        }

        [Route("All")]
        public IActionResult Index()
        {
            List<Book> books = new();
            string q = "usp_ListBooks";
            SqlCommand com = new(q, _conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            DataTable dt = new();
            try
            {
                new SqlDataAdapter(com).Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    books.Add(
                        new Book(
                             Convert.ToInt32(dr["Id"]),
                             dr["Name"].ToString() ?? "unknown",
                             Convert.ToDouble(dr["Price"]),
                             Convert.ToInt32(dr["Stock"])
                        ));
                }
                return Ok(books);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Route("Details/{id}")]
        public IActionResult Details([FromRoute] int id)
        {
            Book book = new();
            string q = "usp_GetBook";
            SqlCommand com = new(q, _conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            com.Parameters.AddWithValue("@Id", id);
            DataTable dt = new();
            try
            {
                new SqlDataAdapter(com).Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    book = new Book(Convert.ToInt32(dr["Id"]), dr["Name"].ToString() ?? "unknown", Convert.ToDouble(dr["Price"]), Convert.ToInt32(dr["Stock"]));
                }
                return Ok(book);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create([FromBody] Book book)
        {
            string q = "usp_AddBook";
            SqlCommand com = new(q, _conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            com.Parameters.AddWithValue("@name", book.Name);
            com.Parameters.AddWithValue("@price", book.Price);
            com.Parameters.AddWithValue("@stock", book.Stock);
            try
            {
                _conn.Open();
                com.ExecuteNonQuery();
                return Ok("Book created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                _conn.Close();
            }

        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update([FromBody] Book book)
        {
            string q = "usp_UpdateBook";
            SqlCommand com = new(q, _conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            com.Parameters.AddWithValue("@id", book.Id);
            com.Parameters.AddWithValue("@name", book.Name);
            com.Parameters.AddWithValue("@price", book.Price);
            com.Parameters.AddWithValue("@stock", book.Stock);
            try
            {
                _conn.Open();
                com.ExecuteNonQuery();
                return Ok("Book updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                _conn.Close();
            }

        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            string q = "usp_DeleteBook";
            SqlCommand com = new(q, _conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            com.Parameters.AddWithValue("@id", id);
            try
            {
                _conn.Open();
                com.ExecuteNonQuery();
                return Ok("Book deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                _conn.Close();
            }
        }
    }

}

