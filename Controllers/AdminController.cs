using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;
using UrunlerApp.Models;

namespace UrunlerApp.Controllers
{
    public class AdminController : Controller
    {
        string connectionString = "TrustServerCertificate=True";
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(EklemeModel model)
        {
            using var connection = new SqlConnection(connectionString);
            var products = "INSERT INTO products (Name, Price, Stock) VALUES (@Name, @Price, @Stock)";

            if (!ModelState.IsValid)
            {
                ViewBag.MessageCssClass = "alert-danger";
                ViewBag.Message = "Eksik veya hatalı işlem yaptın.";
                return View("Message");
            }
            var data = new
            {
                model.Name,
                model.Price,
                model.Stock
            };

            var rowsAffected = connection.Execute(products, data);
            ViewBag.MessageCssClass = "alert-success";
            ViewBag.Message = "Eklendi.";
            return View("Message");
        }

        public IActionResult Guncelle()
        {
            using var connection = new SqlConnection(connectionString);
            var products = connection.Query<EklemeModel>("SELECT * FROM products ORDER BY Name ASC").ToList();
            return View(products);
        }

        [HttpPost]
        public IActionResult Guncelle(EklemeModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Geçersiz veri.";
                ViewBag.MessageCssClass = "alert-danger";
                return View(model);
            }
            using var connection = new SqlConnection(connectionString);
            var products = "UPDATE products SET Name = @Name, Price = @Price, Stock = @Stock WHERE Id = @Id";
            var data = new
            {
                model.Name,
                model.Price,
                model.Stock,
                model.Id
            };
            var affectedRows = connection.Execute(products, data);
            ViewBag.Message = "Güncellendi.";
            ViewBag.MessageCssClass = "alert-success";
            return View("Message");
        }

        public IActionResult Sil()
        {
            using var connection = new SqlConnection(connectionString);
            var products = connection.Query<Urun>("SELECT * FROM products ORDER BY Name ASC").ToList();

            return View(products);
        }

        [HttpPost]
        public IActionResult Sil(int id)
        {
            using var connection = new SqlConnection(connectionString);
            var products = "DELETE FROM products WHERE Id = @Id";
            var rowsAffected = connection.Execute(products, new { Id = id });
            return RedirectToAction("Sil");
        }
    }
}
