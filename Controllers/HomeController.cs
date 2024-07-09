using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using UrunlerApp.Models;

namespace UrunlerApp.Controllers
{
    public class HomeController : Controller
    {
        string connectionString = "TrustServerCertificate=True";
        public IActionResult Index()
        {
            using var connection = new SqlConnection(connectionString);
            var products = connection.Query<Urun>("SELECT * FROM products ORDER BY Name ASC").ToList();

            return View(products);
        }

        public IActionResult SatinAl(int id)
        {
            using var connection = new SqlConnection(connectionString);
            var products = connection.QuerySingleOrDefault<Urun>("SELECT * FROM products WHERE Id = @Id", new { Id = id });

            if (products == null || products.Stock <= 0)
            {
                ViewBag.MessageCssClass = "alert-danger";
                ViewBag.Message = "Ürün bulunamadý veya stokta yok.";
                return View("Message");
            }

            var sqlInsertSales = "INSERT INTO sales (Name, TotalSale, Quantity) VALUES (@Name, @Price, @Quantity)";
            var data = new
            {
                products.Name,
                products.Price,
                Quantity = 1,

            };

            var rowsAffected = connection.Execute(sqlInsertSales, data);

            if (rowsAffected > 0)
            {
                var sqlUpdateProduct = "UPDATE products SET Stock = Stock - 1 WHERE Id = @Id";
                connection.Execute(sqlUpdateProduct, new { Id = id });

                ViewBag.MessageCssClass = "alert-success";
                ViewBag.Message = "Ürün baþarýyla satýn alýndý.";
            }
            else
            {
                ViewBag.MessageCssClass = "alert-danger";
                ViewBag.Message = "Satýn alma iþlemi baþarýsýz.";
            }

            return View("Message");
        }

        public IActionResult Rapor()
        {
            using var connection = new SqlConnection(connectionString);
            var sales = connection.Query<Satis>("SELECT Name, Sum(TotalSale) AS TotalSale, COUNT(*) AS Quantity FROM sales GROUP BY Name").ToList();

            var totalSales = sales.Sum(s => s.TotalSale);
            ViewBag.Sum = totalSales;

            return View(sales);
        }
    }
}
