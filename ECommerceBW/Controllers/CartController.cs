using ECommerceBW.Helpers;
using ECommerceBW.Models;
using ECommerceBW.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBW.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Cart()
        {
            List<CartViewModel> cart = DbHelper.GetCart();
           
            return View(cart);
        }
        //public IActionResult Index()
        //{
        //    List<Product> products = DbHelper.GetProducts();

        //    List<ProductViewModel> productViewModels = products.Select(p => new ProductViewModel()
        //    {
        //        Id = p.Id,
        //        Name = p.Name,
        //        Description = p.Description,
        //        Cover = p.Cover,
        //        Image1 = p.Image1,
        //        Image2 = p.Image2,
        //        Price = p.Price,
        //    }
        //    ).ToList();

        //    return View(productViewModels);
        //}

        [HttpPost]
        public IActionResult AddToCart(Guid id, int amount)
        {
            // qui chiami direttamente il DBHelper
            DbHelper.AddToCartByName(id, amount);

            TempData["SuccessMessage"] = "Prodotto aggiunto al carrello!";
            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            var deleteResult = DbHelper.RemoveProductByid(id);

            if (!deleteResult)
            {

                TempData["DeleteError"] = "Errone durante l'eliminazione del prodotto";
            }

            return RedirectToAction("Cart", "Cart");
        }


    }
}
