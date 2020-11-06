using Model.Models;
using System.Collections.Generic;

namespace Business.Interfaces.Operations
{
    public interface ICashRegisterOperation
    {
        OrderDetail AddProduct(Product product);

        bool RemoveProduct(int id);

        void AddDetail(OrderDetail detail);

        IEnumerable<OrderDetail> GetDetails();

        void Clear();

        decimal Total { get; }
    }
}
