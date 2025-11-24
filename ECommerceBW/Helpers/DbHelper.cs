using Microsoft.AspNetCore.Connections.Features;
using Microsoft.Data
using ECommerceBW.Helper.Enums;
using ECommerceBW.Models;
using System.Data;

namespace ECommerceBW.Helper
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
            using var connection = new SqlConnetion(_masterConnectionString);
            connection.Open();

        }
    }
}
