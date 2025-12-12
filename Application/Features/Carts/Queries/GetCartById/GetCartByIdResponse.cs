using System.ComponentModel.DataAnnotations;

namespace Application.Features.Carts.Queries.GetCartById;

public class GetCartByIdResponse
{
    [Key]
    public Guid CustomerId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public List<CartItemResponse>? CartItems { get; set; }
}

public class CartItemResponse
{
    public Guid ProductId { get; set; }
    public string? ProductName { get; set; }
    public int? Quantity { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

