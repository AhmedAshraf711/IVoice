using Microsoft.AspNetCore.Mvc;

namespace IVoice.Controllers
{
    public class AdminController : Controller
    {
        IAdminRepository adminRepository;
        public AdminController(IAdminRepository adminRepository)
        {
            this.adminRepository = adminRepository;
        }

 
        public IActionResult OrdersDisplay()
        {
            var orders = adminRepository.OrdersDisplay();
            return View(orders);
        }

        [HttpGet]
        public IActionResult OutOfStock()
        {
            var outOfStockProducts = adminRepository.GetProductsRunningOutOfStock();
            return Json(outOfStockProducts);
        }
    }
}
