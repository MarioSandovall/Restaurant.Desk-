using Business.Events.Register;
using Business.Interfaces.Register;
using Business.ViewModels.Main;
using Model.Models;
using Prism.Commands;
using Prism.Events;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Business.ViewModels.Register
{
    public class ChargeViewModel : ModalBase, IChargeViewModel
    {
        #region MyRegion

        private decimal _total;
        public decimal Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }

        private decimal _money;
        public decimal Money
        {
            get => _money;
            set
            {
                SetProperty(ref _money, value);
                AvailableMoney = Money - Total < 0 ? 0 : Money - Total;
                ((DelegateCommand)OkCommand).RaiseCanExecuteChanged();
            }
        }

        private decimal _availableMoney;
        public decimal AvailableMoney
        {
            get => _availableMoney;
            set => SetProperty(ref _availableMoney, value);
        }

        public ObservableCollection<PaymentType> PaymentTypes { get; set; }

        private PaymentType _selectedPaymentType;

        public PaymentType SelectedPaymentType
        {
            get => _selectedPaymentType;
            set
            {
                SetProperty(ref _selectedPaymentType, value);
                ((DelegateCommand)OkCommand).RaiseCanExecuteChanged();
            }
        }


        #endregion

        private Order _order;
        private readonly ILogService _logService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IPrintingService _printingService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IOrderRepository _orderRepository;
        public ChargeViewModel(
            ILogService logService,
            IDataService dataService,
            IDialogService dialogService,
            IEventAggregator eventAggregator,
            IPrintingService printingService,
            IOrderRepository orderRepository)
            : base(dialogService, eventAggregator)
        {
            _logService = logService;
            _dataService = dataService;
            _dialogService = dialogService;
            _printingService = printingService;
            _eventAggregator = eventAggregator;
            _orderRepository = orderRepository;

            PaymentTypes = new ObservableCollection<PaymentType>();
        }

        public void Open(Order order, decimal total)
        {
            _order = order;

            IsOpen = true;
            Total = total;
            Money = 0;
            AvailableMoney = 0;

            PaymentTypes.Clear();
            foreach (var type in _dataService.PaymentTypes) PaymentTypes.Add(type);
            if (PaymentTypes.Any()) SelectedPaymentType = PaymentTypes.First();

            ((DelegateCommand)OkCommand).RaiseCanExecuteChanged();
        }

        protected override async void OnOkExecute()
        {
            try
            {
                var cashierId = _dataService.User.Id;
                var paymentType = _selectedPaymentType.Id;
                var restaurantId = _dataService.Restaurant.Id;
                var httpResponse = await ActionAsync(async () => await _orderRepository.ChargeAsync(_order.Id, _order.OrderNumber, restaurantId, cashierId, paymentType));

                if (httpResponse == null) return;
                if (httpResponse.IsSuccess)
                {
                    OnCloseExecute();
                    _eventAggregator.GetEvent<AfterOrderChargedEvent>().Publish(_order.Id);
                    _printingService.Print(httpResponse.Value);
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

        protected override bool OnOkCanExecute() => Total <= Money && SelectedPaymentType != null;
    }
}
