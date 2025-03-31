namespace ONLINEBOOKSTORE.Models
{
    public class CheckoutViewModel
    {
        public required List<CartItem> CartItems { get; set; }
        public decimal TotalPrice => CartItems.Sum(item=>item.Book.Price*item.Quantity);
    }
}
