using Microsoft.AspNetCore.Connections.Features;
using Microsoft.Data.SqlClient;
using ECommerceBW.Helpers.Enums;
using ECommerceBW.Models;
using System.Data;

namespace ECommerceBW.Helpers
{
    public class DbHelper
    {
        private const string _masterConnectionString = "Server=DESKTOP-8TSL7P4\\SQLEXPRESS;User Id=sa;Password=WinterIs55;Database=master;TrustServerCertificate=True;Trusted_Connection=True";

        private const string _ecommerceConnectionString = "Server=DESKTOP-8TSL7P4\\SQLEXPRESS;User Id=sa;Password=WinterIs55;Database=ECommerceDb;TrustServerCertificate=True;Trusted_Connection=True";

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
                SELECT * FROM Products
                );
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
                    Id=id,
                    Name=name,
                    Description=description,
                    Cover=cover,
                    Image1=image1,
                    Image2=image2,
                    Price=price,
                }
                products.Add(product)
            }
            return products;
             
        }
    }
}
