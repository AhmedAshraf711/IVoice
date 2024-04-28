using IVoice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using Stripe;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using IVoice.ViewModel;
using IVoice.Data;
using Microsoft.AspNetCore.Identity;

namespace IVoice.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly StripeSettings _stripeSettings;
        private ApplicationDbContext context;
        IAdminRepository adminRepository;

        private readonly IHome home;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IOptions<StripeSettings> stripeSettings, ILogger<HomeController> logger,ApplicationDbContext context,IHome home,UserManager<ApplicationUser> userManager, IAdminRepository adminRepository)
        {
            _stripeSettings = stripeSettings.Value;
            _logger = logger;
            this.context = context;
            this.home = home;
            _userManager = userManager;
            this.adminRepository = adminRepository;
        }

        public IActionResult GetUserFeadback()
        {
            
            return View(context.contactus.ToList());
        }
        public IActionResult RedirectBasedOnRole()
        {

            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Dashboard", "Home");
            }
            else if (User.IsInRole("User"))
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login", "Account");
        }
        public IActionResult dashboard()
        {
            ViewBag.productcount=context.products.Count();
            ViewBag.ordercount=context.Orders.Count();
            ViewBag.usercount=context.Users.Count();

            //var currentUser = _userManager.GetUserAsync(User).Result; // Get the current user
            //var allUsersExceptCurrentUser = _userManager.Users.Where(u => u.Id != currentUser.Id).ToList();

            //ViewBag.Users = allUsersExceptCurrentUser;
            ViewBag.Users = adminRepository.UsersDisplay();

            var orders = adminRepository.OrdersDisplay();
            return View(orders);
            
        }
        [HttpPost]
        public IActionResult dashboard(Admindraft admindraft)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            context.admindrafts.Add(admindraft);
            context.SaveChanges();
            return View();
        }
 
        public IActionResult Index()
        {
            return View();
        }
       
        public IActionResult Contactus()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ContactusAsync(ContactUsViewModel contactusvm)
        {
           
            if (!ModelState.IsValid)
            {
                return View(contactusvm);
            }
            await home.newcontactus(contactusvm);
            return RedirectToAction(nameof(Index));
        }



        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult CreateCheckoutSession(string amount)
        {
            try
            {
                var currency = "usd"; // Currency code
                var successUrl = "http://localhost:25467/Cart/Checkout";
                var cancelUrl = "http://localhost:25467/Cart/GetUserCart";
                StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string>
            {
                "card"
            },
                    LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = currency,
                        UnitAmount = Convert.ToInt32(amount) * 100,  // Amount in smallest currency unit (e.g., cents)
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Total Price",

                        }
                    },
                    Quantity = 1
                }
            },
                    Mode = "payment",
                    SuccessUrl = successUrl,
                    CancelUrl = cancelUrl
                };

                var service = new SessionService();
                var session = service.Create(options);

                return Redirect(session.Url);
            }
            catch (StripeException ex)
            {
                // Log the error or handle it appropriately
                // For example, you can return a view with an error message
                return View("Error", ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other types of exceptions (e.g., network issues) here
                // For example, you can return a view with a generic error message
                return View("InternetErrorPage");
                     
            }
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
