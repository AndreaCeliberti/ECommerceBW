using Microsoft.AspNetCore.Mvc;
using ECommerceBW.Helpers;
using ECommerceBW.Models;
using ECommerceBW.ViewModels;


namespace ECommerceBW.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            List<Product> products = DbHelper.GetProducts();

            List<ProductViewModel> productViewModels = products.Select(p => new ProductViewModel()
            {
            Id = p.Id,
            Name=p.Name,
            Description=p.Description,
            Cover=p.Cover,
            Image1=p.Image1,
            Image2=p.Image2,
            Price=p.Price,
            }
            ).ToList();

            return View(productViewModels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateViewModel createViewModel)
        {
            Product newProduct = new Product()
            {
                Id = Guid.NewGuid(),
                Name=createViewModel.Name,
                Description=createViewModel.Description,
                Cover=createViewModel.Cover,
                Image1=createViewModel.Image1,
                Image2=createViewModel.Image2,
                Price=createViewModel.Price,
            };

            bool creationResult = DbHelper.AddProduct(newProduct);
            if (!creationResult)
            {
                TempData["CreationError"] = "Errore durante la creazione del prodotto";
                return RedirectToAction("Create","Product");
            }

            return RedirectToAction("Index", "Product");
        }




        //prendere la lista per la pagina edit
        
        public IActionResult EditGrid()
        {
            List<Product> products = DbHelper.GetProducts();

        List<ProductViewModel> productViewModels = products.Select(p => new ProductViewModel()
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Cover = p.Cover,
            Image1 = p.Image1,
            Image2 = p.Image2,
            Price = p.Price,
        }
        ).ToList();

            return View(productViewModels);
    }

        [HttpGet]
        public IActionResult EditProduct(Guid id)
        {
            
            Product? product = DbHelper.GetProductsById(id);

            if (product == null)
                return NotFound(); 

            return View(product);
        }

        [HttpPost]
        public IActionResult EditProduct(Product product)
        {
            Product editProduct = new Product()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Cover = product.Cover,
                Image1 = product.Image1,
                Image2 = product.Image2,
                Price = product.Price,
            };

            bool creationResult = DbHelper.UpdateProduct(editProduct);
            if (!creationResult)
            {
                TempData["CreationError"] = "Errore durante la creazione del prodotto";
                return RedirectToAction("Create", "Product");
            }

            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            Product? product = DbHelper.GetProductsById(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

    }
}
