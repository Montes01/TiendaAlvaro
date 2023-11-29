namespace TiendaAlvaro.Models
{
    public class Item
    {
        /*
        userId, bookId, quantity 
         */

        public int UserId { get; set; }
        public int? BookId { get; set; }
        public int? Quantity { get; set; }

        public Item (int userId, int? bookId, int? quantity)
        {
            UserId = userId;
            BookId = bookId;
            Quantity = quantity;
        }
    }   
}
