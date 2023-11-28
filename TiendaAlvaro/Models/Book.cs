namespace TiendaAlvaro.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
        public int? Stock { get; set; }

        public Book(int id, string name, double price, int stock)
        {
            Id = id;
            Name = name;
            Price = price;
            Stock = stock;
        }
        public Book()
        {

        }
    }
}
