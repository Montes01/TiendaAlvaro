using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using TiendaAlvaro.Models;

namespace TiendaAlvaro.Controllers
{
    [Route("Users")]
    [ApiController]
    public class UserController : ControllerBase
    {


        private static SqlConnection _conn;
        public UserController(IConfiguration? _config)
        {
            string connectionString = _config.GetConnectionString("SqlConnection");
            _conn = new SqlConnection(connectionString);
        }
        [HttpPost]
        [Route("Create")]
        public IActionResult Create([FromBody] User user)
        {
            string q = "usp_CreateUser";
            SqlCommand com = new(q, _conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            com.Parameters.AddWithValue("@id", user.Id);
            com.Parameters.AddWithValue("@name", user.Name);
            try
            {
                _conn.Open();
                com.ExecuteNonQuery();
                return Ok("User created");
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
        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            List<User> users = new();
            string q = "usp_ListUsers";

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
                    users.Add(
                                               new User(
                                                                               Convert.ToInt32(dr["Id"]),
                                                                                                           dr["Name"].ToString() ?? "unknown"
                                                                                                                                  ));
                }
                return Ok(users);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("Get/{id}")]
        public IActionResult Get(int id)
        {
            User user = new();
            string q = "usp_GetUser";
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
                    user = new User(Convert.ToInt32(dr["Id"]), dr["Name"].ToString() ?? "unknown");
                }
                return Ok(user);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            string q = "usp_DeleteUser";

            SqlCommand com = new(q, _conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            com.Parameters.AddWithValue("@Id", id);
            try
            {
                _conn.Open();
                com.ExecuteNonQuery();
                return Ok("User deleted");
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
