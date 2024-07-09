using System.ComponentModel.DataAnnotations;

namespace UrunlerApp.Models
{
    public class SatisModel
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }   
    }
}
