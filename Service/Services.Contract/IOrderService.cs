
namespace Service.Services.Contract
{
    public interface IOrderService
    {
        IQueryable<Order> FilterOrderPaginatedQueryable(OrderSortingEnum sortBy, string search);
        IQueryable<Order> FilterOrderPaginatedByCustomerIdQueryable(OrderSortingEnum sortBy, string search, Guid customerId);
        Task<Order?> GetOrderByIdAsync(Guid id);
        Task<string> AddOrderAsync(Order order);
        Task<string> PlaceOrderAsync(Order order);
        Task<string> EditOrderAsync(Order order);
        Task<string> DeleteOrderAsync(Order order);
    }
}
