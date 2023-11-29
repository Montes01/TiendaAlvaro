namespace TiendaAlvaro.Models
{
    public class CartResponse
    {
        public string BookName { get; set; }
        public int Quantity { get; set; }

        public CartResponse(string bookName, int quantity)
        {
            BookName = bookName;
            Quantity = quantity;
        }
    }
}
