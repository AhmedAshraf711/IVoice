using IVoice.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IVoice.Reopsitories
{
    public class AdminRepository:IAdminRepository
    {
        ApplicationDbContext dbContext { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminRepository(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            _userManager = userManager;
        }

        public List<OrdersDisplayModel> OrdersDisplay()
        {
            var ordersWithDetails =  dbContext.Orders
                .Select(order => new OrdersDisplayModel
                {
                    OrderId = order.Id,
                    UserName = dbContext.Users.FirstOrDefault(u => u.Id == order.UserId).UserName,
                    CreatedAt = order.CreateDate,
                    //Status = order.OrderStatus.Name // Assuming you have an OrderStatus entity associated with each order
                    // You can add more properties as needed
                }).ToList();

            return ordersWithDetails;
        }
        public List<UsersDisplayModel> UsersDisplay()
        {
            var adminUsers = _userManager.GetUsersInRoleAsync("admin").GetAwaiter().GetResult();

            // Retrieve admin user IDs
            var adminUserIds = adminUsers.Select(u => u.Id);

            // Perform LINQ query using adminUserIds
            var usersWithOrderCount = from user in dbContext.Users
                                      where !adminUserIds.Contains(user.Id)
                                      join order in dbContext.Orders on user.Id equals order.UserId into userOrders
                                      select new UsersDisplayModel
                                      {
                                          Name = user.UserName,
                                          Email = user.Email,
                                          Address = user.Address,
                                          OrdersCount = userOrders.Count() // Count the orders for each user
                                      };

            return usersWithOrderCount.ToList();
        }
        public List<Product> GetProductsRunningOutOfStock()
        {
            return dbContext.products.Where(p => p.Quantity < 10).ToList();
        }


    }
}
