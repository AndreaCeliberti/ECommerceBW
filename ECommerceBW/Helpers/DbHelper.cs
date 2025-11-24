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

        }
        public static void CreateProductsTable() { }
    }
}
