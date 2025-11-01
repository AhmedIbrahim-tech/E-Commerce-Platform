
namespace Service.Services.Contract
{
    public interface IOrderItemService
    {
        IQueryable<OrderItem> GetOrderItemsByOrderIdQueryable(Guid orderId);
    }
}
