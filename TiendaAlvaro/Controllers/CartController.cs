using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using TiendaAlvaro.Models;

namespace TiendaAlvaro.Controllers
{
    [Route("Cart")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private static SqlConnection _conn;
        public CartController(IConfiguration? _config)
        {
            string connectionString = _config.GetConnectionString("SqlConnection");
            _conn = new SqlConnection(connectionString);
        }

        [HttpPost]
		[Route("AddItem")]
		public IActionResult AddItem([FromBody] Item item)
		{
			string q = "usp_AddItemToCart";
            SqlCommand com = new(q, _conn)
			{
                CommandType = CommandType.StoredProcedure
            };
            com.Parameters.AddWithValue("@userId", item.UserId);
            com.Parameters.AddWithValue("@bookId", item.BookId);
            com.Parameters.AddWithValue("@quantity", item.Quantity);
            try
			{
                com.Connection.Open();
                com.ExecuteNonQuery();
                com.Connection.Close();
                return Ok("Item added to cart");
            }
            catch (Exception ex)
			{
                return BadRequest(ex.Message);
            }
		}

        [HttpPut]
        [Route("UpdateItem")]
        public IActionResult UpdateItem([FromBody] Item item)
        {
            string q = "usp_UpdateItemQuantity";
            SqlCommand com = new(q, _conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            com.Parameters.AddWithValue("@userId", item.UserId);
            com.Parameters.AddWithValue("@bookId", item.BookId);
            com.Parameters.AddWithValue("@quantity", item.Quantity);
            try
            {
                com.Connection.Open();
                com.ExecuteNonQuery();
                com.Connection.Close();
                return Ok("Item quantity updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("RemoveItem")]
        public IActionResult RemoveItem([FromBody] Item item)
        {
            string q = "usp_RemoveItem";
            SqlCommand com = new(q, _conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            com.Parameters.AddWithValue("@userId", item.UserId);
            com.Parameters.AddWithValue("@bookId", item.BookId);
            try
            {
                com.Connection.Open();
                com.ExecuteNonQuery();
                com.Connection.Close();
                return Ok("Item removed from cart");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("ClearCart")]
        public IActionResult ClearCart([FromQuery] int id)
        {
            string q = "usp_ClearCart";
            SqlCommand com = new(q, _conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            com.Parameters.AddWithValue("@userId", id);
            try
            {
                com.Connection.Open();
                com.ExecuteNonQuery();
                com.Connection.Close();
                return Ok("Cart cleared");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*CREATE PROC usp_SeeCart
		@userId INT
AS
BEGIN
	SELECT I.quantity, B.name FROM ITEM I INNER JOIN USERS U ON U.id = I.fkIdUser INNER JOIN BOOKS B ON B.id = I.fkIdBooks WHERE I.fkIdUser = @userId
END;*/
        [HttpGet]
        [Route("SeeCart")]
        public IActionResult SeeCart([FromQuery] int id)
        {
            string q = "usp_SeeCart";
            
            SqlCommand com = new(q, _conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            com.Parameters.AddWithValue("@userId", id);
            DataTable dt = new();
            SqlDataAdapter da = new(com);
            try
            {
                da.Fill(dt);
                List<CartResponse> items = new();
                foreach (DataRow dr in dt.Rows)
                {
                    items.Add(new CartResponse(dr["name"].ToString() ?? "unknown", Convert.ToInt32(dr["quantity"])));
                }
                return Ok(items);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*CREATE PROC usp_SeeTotal*/
    }
}
