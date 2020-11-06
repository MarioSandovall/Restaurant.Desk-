using Business.Events;
using Model.Models;
using Prism.Events;

namespace Business.Wrappers
{
    public class OrderDetailWrapper : ModelWrapper<OrderDetail>
    {

        public int Id => Model.Id;

        public int Quantity
        {
            get => GetValue<int>();
            set
            {
                SetValue(value);
                _eventAggregator.GetEvent<UpdateProductTotalEvent>().Publish();
            }
        }

        public string ProductName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public decimal Price
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public int ProductId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int OrderId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        private decimal _total;
        public decimal Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }



        private readonly IEventAggregator _eventAggregator;
        public OrderDetailWrapper(OrderDetail model, IEventAggregator eventAggregator)
            : base(model)
        {
            _eventAggregator = eventAggregator;
            Total = Quantity * Price;
        }
    }
}
