using Business.Interfaces.Operations;
using Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace Business.Utils.Operations
{
    public class CashRegisterOperation : ICashRegisterOperation
    {
        public decimal Total => _orderDetails.Values.Sum(x => x.Price * x.Quantity);

        private readonly Dictionary<int, OrderDetail> _orderDetails;

        public CashRegisterOperation()
        {
            _orderDetails = new Dictionary<int, OrderDetail>();
        }


        public OrderDetail AddProduct(Product product)
        {
            OrderDetail detail;
            if (_orderDetails.ContainsKey(product.Id))
            {
                _orderDetails[product.Id].Quantity += 1;
                detail = _orderDetails[product.Id];
            }
            else
            {
                detail = new OrderDetail(product);
                _orderDetails.Add(detail.ProductId, detail);
            }

            return detail;
        }

        public bool RemoveProduct(int productId)
        {
            if (!_orderDetails.ContainsKey(productId)) return false;
            _orderDetails.Remove(productId);
            return true;
        }

        public void AddDetail(OrderDetail detail)
        {
            _orderDetails.Add(detail.ProductId, detail);
        }

        public IEnumerable<OrderDetail> GetDetails()
        {
            return _orderDetails.Values.ToList();
        }

        public void Clear()
        {
            _orderDetails.Clear();
        }

    }
}
