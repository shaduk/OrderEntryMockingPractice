namespace OrderEntryMockingPractice.Models
{
    public class OrderItem
    {
        public Product Product { get; set; }
        public decimal Quantity { get; set; }

        public decimal GetNetPrice()
        {
            return Product.Price * Quantity;
        }
    }
}
