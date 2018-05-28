using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MyService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Shop : IShop
    {
        private static List<int> ides = new List<int>();

        private static string connectionString =
             "Data Source=(local);Initial Catalog=MyShopDB;"
             + "Integrated Security=true";


        public Result Add(Book book)
        {
            if (ides.Count == 0)
                GetIdes();

            if (book.Title.Equals(""))
                return new Result("there aren't Title", "Failed");

            foreach (var item in ides)
            {
                if (item == book.ID)
                    return new Result("The Book is already exist", "Failed");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string queryString = $"INSERT INTO Bookes (Id,Author,Title,Price,Year)" +
                    $" VALUES('{book.ID}','{book.Author}','{book.Title}','{book.Price}','{book.Year}')";

                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
            ides.Add(book.ID);
            return new Result("The Book will be added", "Access");
        }

        public List<Book> SeeAll()
        {
            List<Book> result = new List<Book>();

            if (ides.Count == 0)
                GetIdes();

            if (ides.Count == 0)
                return null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string queryString = $"select * from Bookes";

                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    result.Add(new Book()
                    {
                        ID = dataReader.GetInt32(0),
                        Author = dataReader[1].ToString(),
                        Title = dataReader[2].ToString(),
                        Price = (double)dataReader[3],
                        Year = dataReader.GetInt32(4)
                    });
                }
                dataReader.Close();
            }
            return result;
        }

        public Result UpdatePrice(int id, int price)
        {
            if (ides.Count == 0)
                GetIdes();

            bool isExist = false;
            foreach (var item in ides)
            {
                if (item == id)
                {
                    isExist = true;
                    break;
                }
            }

            if (!isExist)
                return new Result("there aren't book with that ID", "Failed");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string queryString = $"UPDATE Bookes Set Price = '{price}' where Id = '{id}';";

                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
            return new Result("The Price was updated", "Access");
        }

        private void GetIdes()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string queryString = "select Id from Bookes";

                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    ides.Add(dataReader.GetInt32(0));
                }
                dataReader.Close();
            }
        }
    }
}
