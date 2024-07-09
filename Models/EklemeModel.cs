using System.Diagnostics.Eventing.Reader;

namespace UrunlerApp.Models
{
    public class EklemeModel
    {
        public  int Id { get; set; }    
        public string? Name { get; set; }
        public int? Price { get; set; }
        public int? Stock { get; set; }
    }
}
