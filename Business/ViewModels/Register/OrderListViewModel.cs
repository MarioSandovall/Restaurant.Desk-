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
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Business.ViewModels.Register
{

    public class OrderListViewModel : ViewModelBase, IOrderListViewModel
    {

        #region Properties

        public ObservableCollection<OrderWrapper> Orders { get; set; }

        #endregion


        #region Commands

        public ICommand RefreshCommand { get; set; }

        #endregion

        private readonly ILogService _logService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IOrderRepository _orderRepository;
        private readonly IEventAggregator _eventAggregator;
        public OrderListViewModel(
            ILogService logService,
            IDataService dataService,
            IDialogService dialogService,
            IOrderRepository orderRepository,
            IEventAggregator eventAggregator)
            : base(dialogService)
        {
            _logService = logService;
            _dataService = dataService;
            _dialogService = dialogService;
            _orderRepository = orderRepository;
            _eventAggregator = eventAggregator;

            Orders = new ObservableCollection<OrderWrapper>();

            RefreshCommand = new DelegateCommand(OnRefreshExecute);

            eventAggregator.GetEvent<RemoveOrderEvent>().Subscribe(OnRemoveOrder);
            eventAggregator.GetEvent<AfterOrderCreatedEvent>().Subscribe(OnOrderCreated);

        }



        public async void Load(ICollection<Order> orders)
        {
            try
            {
                Orders.Clear();
                foreach (var order in orders)
                {
                    Orders.Add(new OrderWrapper(order, _eventAggregator));
                }
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await _dialogService.ShowMessageAsync(ex.Message);
            }
        }

        private async void OnRefreshExecute()
        {
            try
            {
                IsBusy = true;
                var cashRegisterId = _dataService.CashRegister.Id;
                var httpResponse = await _orderRepository.GetOrdersByCashRegisterId(cashRegisterId);
                if (httpResponse.IsSuccess)
                {
                    Orders.Clear();
                    foreach (var order in httpResponse.Value)
                    {
                        Orders.Add(new OrderWrapper(order, _eventAggregator));
                    }
                }
                else
                {
                    await _dialogService.ShowMessageAsync(httpResponse.Message);
                }
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await _dialogService.ShowMessageAsync(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnOrderCreated(Order order)
        {
            Orders.Add(new OrderWrapper(order, _eventAggregator));
            _eventAggregator.GetEvent<SelectOrderEvent>().Publish(order);
        }

        private void OnRemoveOrder(int id)
        {
            var order = Orders.SingleOrDefault(x => x.Id == id);
            if (order == null) return;
            Orders.Remove(order);
        }

    }
}
