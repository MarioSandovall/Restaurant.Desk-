using Business.Events.Administrator;
using Business.Events.Register;
using Business.Interfaces.Register;
using Business.ViewModels.Main;
using Business.Wrappers;
using Model.Models;
using Prism.Commands;
using Prism.Events;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.Generic;

namespace Business.ViewModels.Register
{
    public class OrderViewModel : ModalBase, IOrderViewModel
    {
        #region Properties

        private OrderWrapper _order;
        public OrderWrapper Order
        {
            get => _order;
            set => SetProperty(ref _order, value);
        }

        public ISearchProductViewModel SearchProductViewModel { get; set; }

        public IProductListViewModel ProductListViewModel { get; set; }

        #endregion

        private readonly ILogService _logService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IOrderRepository _orderRepository;

        public OrderViewModel(
            ILogService logService,
            IDataService dataService,
            IDialogService dialogService,
            IEventAggregator eventAggregator,
            IProductListViewModel productListViewModel,
            ISearchProductViewModel searchProductViewModel,
            IOrderRepository orderRepository)
            : base(dialogService,eventAggregator)
        {
            _logService = logService;
            _dataService = dataService;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _orderRepository = orderRepository;

            SearchProductViewModel = searchProductViewModel;
            ProductListViewModel = productListViewModel;
        }

        public void Load(ICollection<Product> products, ICollection<Category> categories)
        {
            SearchProductViewModel.Load(products, categories);
        }

        public void Open(Order order, ICollection<OrderDetail> details)
        {
            IsOpen = true;
            InitializeOrder(order);
            ProductListViewModel.Load(details);
            ((DelegateCommand)OkCommand).RaiseCanExecuteChanged();
        }

        private void InitializeOrder(Order order)
        {
            Order = new OrderWrapper(order);
            Order.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Order.HasErrors))
                {
                    ((DelegateCommand)OkCommand).RaiseCanExecuteChanged();
                }
            };

            Order.Name = order.Name;
        }

        protected override async void OnOkExecute()
        {
            try
            {
                if (ProductListViewModel.Deatils.Count == 0)
                {
                    await _dialogService.ShowMessageAsync("Debes agregar productos");
                    return;
                }

                var order = Order.Model;
                order.RestaurantId = _dataService.Restaurant.Id;
                order.CashRegisterId = _dataService.CashRegister.Id;
                var httpResponse = await ShowProgressAsync(async () => await _orderRepository.CreateAsync(order, ProductListViewModel.Deatils));
                if (httpResponse == null) return;

                if (httpResponse.IsSuccess)
                {
                    IsOpen = false;
                    _eventAggregator.GetEvent<CloseOrderDetailEvent>().Publish(httpResponse.Value.Id);
                    _eventAggregator.GetEvent<AfterOrderCreatedEvent>().Publish(httpResponse.Value);
                }
                else
                {
                    await _dialogService.ShowMessageAsync(httpResponse.Message, httpResponse.Title);
                }
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await _dialogService.ShowMessageAsync(ex.Message);
            }
        }

        protected override bool OnOkCanExecute() => Order != null && !Order.HasErrors;

    }

}
