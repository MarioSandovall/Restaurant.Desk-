namespace Model.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public byte[] ProductImage { get; set; }
        public string ProductName { get; set; }
        public decimal Total => Quantity * Price;

        public OrderDetail(OrderDetail detail)
        {
            Id = detail.Id;
            Price = detail.Price;
            OrderId = detail.OrderId;
            Quantity = detail.Quantity;
            ProductId = detail.ProductId;
            ProductName = detail.ProductName;
            ProductImage = detail.ProductImage;
        }

        public OrderDetail(Product product)
        {
            Quantity = 1;
            Price = product.Price;
            ProductId = product.Id;
            ProductName = product.Name;
        }

        public OrderDetail()
        {

        }

    }
}
