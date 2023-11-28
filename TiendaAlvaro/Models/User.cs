namespace TiendaAlvaro.Models
{
    public class User
    {
        /*
         CREATE TABLE USERS
(
 id INT PRIMARY KEY,
 name VARCHAR(50)
);*/
        public int Id { get; set; }
        public string? Name { get; set; }

        public User(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public User()
        {

        }
    }
}
