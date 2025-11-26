using ECommerceBW.Models;

namespace ECommerceBW.ViewModels
{
    public class DetailViewModel
    {
        public List<Product> ProductList { get; set; } = new List<Product>();

        public Product? Prodotto { get; set; }


        
    }
}
