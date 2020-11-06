using Business.Events.Administrator;
using Business.Events.Register;
using Business.Interfaces.Register;
using Business.ViewModels.Main;
using MahApps.Metro.Controls.Dialogs;
using Model.Models;
using Prism.Commands;
using Prism.Events;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Business.ViewModels.Register
{
    public class OrderDetailViewModel : ViewModelBase, IOrderDetailViewModel
    {
        #region Properties

        private Order _order;
        public Order Order
        {
            get => _order;
            set => SetProperty(ref _order, value);
        }

        private decimal _total;
        public decimal Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }

        public ObservableCollection<OrderDetail> Details { get; set; }

        #endregion


        #region Commands

        public ICommand CloseCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public ICommand ChargeCommand { get; set; }

        public ICommand EditCommand { get; set; }

        #endregion

        private readonly ILogService _logService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IOrderViewModel _orderViewModel;
        private readonly IChargeViewModel _chargeViewModel;
        private readonly IEventAggregator _eventAggregator;
        private readonly IOrderRepository _orderRepository;

        public OrderDetailViewModel(
            ILogService logService,
            IDataService dataService,
            IDialogService dialogService,
            IOrderViewModel orderViewModel,
            IChargeViewModel chargeViewModel,
            IEventAggregator eventAggregator,
            IOrderRepository orderRepository)
            : base(dialogService)
        {
            _logService = logService;
            _dataService = dataService;
            _dialogService = dialogService;
            _orderViewModel = orderViewModel;
            _chargeViewModel = chargeViewModel;
            _eventAggregator = eventAggregator;
            _orderRepository = orderRepository;

            Details = new ObservableCollection<OrderDetail>();

            EditCommand = new DelegateCommand(OnEditExecute, () => !IsBusy);
            CloseCommand = new DelegateCommand(OnCloseExecute, () => !IsBusy);
            ChargeCommand = new DelegateCommand(OnChargeExecute, () => !IsBusy);
            CancelCommand = new DelegateCommand(OnCancelExecute, () => !IsBusy);
        }

        public async Task LoadAsync(Order order)
        {
            try
            {
                Total = 0;
                Order = order;
                IsBusy = true;

                var httpResponse = await _orderRepository.GetDetailsByIdAsync(Order.Id);
                if (httpResponse == null) return;
                if (httpResponse.IsSuccess)
                {
                    Details.Clear();
                    foreach (var detail in httpResponse.Value) Details.Add(detail);
                    Total = Details.Sum(x => x.Total);
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
            finally
            {
                IsBusy = false;
                ((DelegateCommand)EditCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)CloseCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)ChargeCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)CancelCommand).RaiseCanExecuteChanged();
            }
        }

        private async void OnCancelExecute()
        {
            var result = await _dialogService.AskQuestionAsync("¿Estás seguro de querer cancelar el pedido?");
            if (result == MessageDialogResult.Negative) return;

            var cashierId = _dataService.User.Id;
            var httpResponse = await ShowProgressAsync(async () => await _orderRepository.CancelAsync(Order.Id, cashierId));
            if (httpResponse == null) return;

            if (httpResponse.IsSuccess)
            {
                _eventAggregator.GetEvent<RemoveOrderEvent>().Publish(Order.Id);
                _eventAggregator.GetEvent<CloseOrderDetailEvent>().Publish(Order.Id);
            }
            else
            {
                await _dialogService.ShowMessageAsync(httpResponse.Message);
            }
        }

        private void OnEditExecute() => _orderViewModel.Open(Order, Details);

        private void OnChargeExecute() => _chargeViewModel.Open(Order, Total);

        private void OnCloseExecute() => _eventAggregator.GetEvent<CloseOrderDetailEvent>().Publish(Order.Id);

    }
}
