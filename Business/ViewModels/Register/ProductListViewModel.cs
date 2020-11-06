using Business.Events;
using Business.Interfaces.Operations;
using Business.Interfaces.Register;
using Business.Wrappers;
using Model.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Business.ViewModels.Register
{

    public class ProductListViewModel : BindableBase, IProductListViewModel
    {
        #region Properties

        private decimal _total;
        public decimal Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }

        public ObservableCollection<OrderDetailWrapper> OrderDetails { get; set; }

        public ICollection<OrderDetail> Deatils
        {
            get { return OrderDetails.Select(x => x.Model).ToList(); }
        }

        #endregion

        #region Commands

        public ICommand RemoveProductCommand { get; set; }

        #endregion

        private readonly IEventAggregator _eventAggregator;
        private readonly ICashRegisterOperation _cashRegisterOperation;
        public ProductListViewModel(
            ICashRegisterOperation cashRegisterOperation,
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _cashRegisterOperation = cashRegisterOperation;

            OrderDetails = new ObservableCollection<OrderDetailWrapper>();

            RemoveProductCommand = new DelegateCommand<int?>(OnRemoveProductExecute);

            _eventAggregator.GetEvent<AddProductToProductListEvent>()
                .Subscribe(OnAddProductToCashRegister);

            _eventAggregator.GetEvent<UpdateProductTotalEvent>()
                .Subscribe(() => Total = _cashRegisterOperation.Total);
        }

        private void OnRemoveProductExecute(int? productId)
        {
            if (!productId.HasValue) return;
            if (_cashRegisterOperation.RemoveProduct(productId.Value))
            {
                var product = OrderDetails.Single(x => x.ProductId == productId);
                OrderDetails.Remove(product);
                Total = _cashRegisterOperation.Total;
            }
        }

        private void OnAddProductToCashRegister(Product product)
        {
            var model = _cashRegisterOperation.AddProduct(product);
            var detail = OrderDetails.SingleOrDefault(x => x.ProductId == model.ProductId);
            if (detail == null) { OrderDetails.Add(new OrderDetailWrapper(model, _eventAggregator)); }
            else detail.Quantity = model.Quantity;
            Total = _cashRegisterOperation.Total;
        }

        public void Load(ICollection<OrderDetail> details)
        {
            ClearValues();
            if (details.Count == 0) return;
            foreach (var detail in details)
            {
                var newDetail = new OrderDetail(detail);
                _cashRegisterOperation.AddDetail(newDetail);
                OrderDetails.Add(new OrderDetailWrapper(newDetail, _eventAggregator));
            }

            Total = _cashRegisterOperation.Total;
        }
        private void ClearValues()
        {
            Total = 0M;
            OrderDetails.Clear();
            _cashRegisterOperation.Clear();
        }

    }
}
