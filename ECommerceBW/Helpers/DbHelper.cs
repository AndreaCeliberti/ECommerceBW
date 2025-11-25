using Microsoft.AspNetCore.Connections.Features;
using Microsoft.Data.SqlClient;
using ECommerceBW.Helpers.Enums;
using ECommerceBW.Models;
using System.Data;
using System.Runtime.InteropServices.Marshalling;

namespace ECommerceBW.Helpers
{
    public class DbHelper
    {

        //Alessio
        //private const string _masterConnectionString = "Server=DESKTOP-8TSL7P4\\SQLEXPRESS;User Id=sa;Password=WinterIs55;Database=master;TrustServerCertificate=True;Trusted_Connection=True";

        //private const string _ecommerceConnectionString = "Server=DESKTOP-8TSL7P4\\SQLEXPRESS;User Id=sa;Password=WinterIs55;Database=ECommerceDb;TrustServerCertificate=True;Trusted_Connection=True";

        //Claudio
        //private const string _masterConnectionString = "Server=DESKTOP-LGN2PEU\\SQLEXPRESS;User Id=sa;Password=SA;Database=master;TrustServerCertificate=True;Trusted_Connection=True";

        //private const string _ecommerceConnectionString = "Server=DESKTOP-LGN2PEU\\SQLEXPRESS;User Id=sa;Password=SA;Database=ECommerceDb;TrustServerCertificate=True;Trusted_Connection=True";
        
        //Andrea
        private const string _masterConnectionString = "Server=WIN-39AQM68JP3H\\SQLEXPRESS;User Id=sa;Password=sa;Database=master;TrustServerCertificate=True;Trusted_Connection=True";

        private const string _ecommerceConnectionString = "Server=WIN-39AQM68JP3H\\SQLEXPRESS;User Id=sa;Password=sa;Database=ECommerceDb;TrustServerCertificate=True;Trusted_Connection=True";

        public static void InitializeDb()
        {
            CreateDb();
            CreateProductsTable();
        }
        public static void CreateDb()
        {
            using var connection = new SqlConnection(_masterConnectionString);
            connection.Open();
            var commandText = """
                CREATE DATABASE ECommerceDb;
                """;
            var command = connection.CreateCommand();
            command.CommandText = commandText;
            try
            {
                command.ExecuteNonQuery();
            }
            catch(SqlException ex)
            {
                if (ex.Number == (int)ECodiciDb.DatabaseEsistente)
                {
                    Console.WriteLine("Il database è già stato creato.");
                }

            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(1);
            }

        }

        public static void CreateProductsTable()
        {
            using var connection = new SqlConnection(_ecommerceConnectionString);

            connection.Open();

            var commandText = """
                CREATE TABLE Products (
                    Id UNIQUEIDENTIFIER PRIMARY KEY,
                    Name NVARCHAR(25) NOT NULL,
                    Description NVARCHAR(2000) NOT NULL,
                    Cover NVARCHAR(2000) NOT NULL,
                    Image1 NVARCHAR(2000) NOT NULL,
                    Image2 NVARCHAR(2000) NOT NULL,
                    Price DECIMAL(6,2)
                );
                """;

            var command = connection.CreateCommand();

            command.CommandText = commandText;

            try
            {
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                if (ex.Number == (int)ECodiciDb.TabellaEsistente)
                {
                    Console.WriteLine("La tabella è già stata creata.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(1);
            }
        }
        
        public static List<Product> GetProducts()
        {
            using var connection = new SqlConnection(_ecommerceConnectionString);

            connection.Open();

            var commandText = """
                SELECT * FROM Products                ;
                """;

            var command = connection.CreateCommand();

            command.CommandText = commandText;
            using var reader = command.ExecuteReader();
            var products = new List<Product>();
            while (reader.Read())
            {
                var id = reader.GetGuid(0);
                var name = reader.GetString(1);
                var description = reader.GetString(2);
                var cover = reader.GetString(3);
                var image1 = reader.GetString(4);
                var image2 = reader.GetString(5);
                var price = reader.GetDecimal(6);


                var product = new Product()
                {
                    Id = id,
                    Name = name,
                    Description = description,
                    Cover = cover,
                    Image1 = image1,
                    Image2 = image2,
                    Price = price,
                };
                products.Add(product);
            }
            return products;
             
        }

        public static bool AddProduct(Product product)
        {
            bool result = false;

            using var connection = new SqlConnection(_ecommerceConnectionString);

            connection.Open();

            var commandText = """
                INSERT INTO Products VALUES (
                    @Id,
                    @Name,
                    @Description,
                    @Cover,
                    @Image1,
                    @Image2,
                    @Price
                );
                """;

            var command = connection.CreateCommand();
            command.CommandText = commandText;

            command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier);
            command.Parameters.Add("@Name", SqlDbType.NVarChar,25);
            command.Parameters.Add("@Description", SqlDbType.NVarChar,2000);
            command.Parameters.Add("@Cover", SqlDbType.NVarChar, 2000);
            command.Parameters.Add("@Image1", SqlDbType.NVarChar, 2000);
            command.Parameters.Add("@Image2", SqlDbType.NVarChar, 2000);
            command.Parameters.Add("@Price", SqlDbType.Decimal).Precision=6;
            command.Parameters["@Price"].Scale = 2;

            command.Prepare();
            command.Parameters["@Id"].Value = product.Id;
            command.Parameters["@Name"].Value = product.Name;
            command.Parameters["@Description"].Value = product.Description;
            command.Parameters["@Cover"].Value = product.Cover;
            command.Parameters["@Image1"].Value = product.Image1;
            command.Parameters["@Image2"].Value = product.Image2;
            command.Parameters["@Price"].Value = product.Price;

            try
            {
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }


        public static Product? GetProductsById(Guid Id)
        {
            using var connection = new SqlConnection(_ecommerceConnectionString);
            connection.Open();

            var commandText = "SELECT * FROM Products WHERE Id=@Id;";
            var command = connection.CreateCommand();
            command.CommandText = commandText;

            command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Id;

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Product
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    Cover = reader.GetString(3),
                    Image1 = reader.GetString(4),
                    Image2 = reader.GetString(5),
                    Price = reader.GetDecimal(6)
                };
            }

            return null; 
        }


        public static bool UpdateProduct(Product product)
        {
 

            using var connection = new SqlConnection(_ecommerceConnectionString);
            connection.Open();

        var commandText = """
            UPDATE Products
                    SET Name = @Name,
                    Description = @Description,
                    Cover = @Cover,
                    Image1 = @Image1,
                    Image2 = @Image2,
                    Price = @Price
                    WHERE Id = @Id;
            """;


            using var command = connection.CreateCommand();
            command.CommandText = commandText;

            command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = product.Id;
            command.Parameters.Add("@Name", SqlDbType.NVarChar, 25).Value = product.Name;
            command.Parameters.Add("@Description", SqlDbType.NVarChar, 2000).Value = product.Description;
            command.Parameters.Add("@Cover", SqlDbType.NVarChar, 2000).Value = product.Cover;
            command.Parameters.Add("@Image1", SqlDbType.NVarChar, 2000).Value = product.Image1;
            command.Parameters.Add("@Image2", SqlDbType.NVarChar, 2000).Value = product.Image2;
            command.Parameters.Add("@Price", SqlDbType.Decimal).Value = product.Price;

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;


        }

    }
}
