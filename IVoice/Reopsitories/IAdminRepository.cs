using IVoice.Models.DTO;

namespace IVoice.Reopsitories
{
    public interface IAdminRepository
    {
        List<UsersDisplayModel> UsersDisplay();

        public List<Product> GetProductsRunningOutOfStock();
        List<OrdersDisplayModel> OrdersDisplay();


    }
}
