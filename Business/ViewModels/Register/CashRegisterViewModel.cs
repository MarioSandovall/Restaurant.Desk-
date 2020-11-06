using Business.Events.Administrator;
using Business.Events.Register;
using Business.Interfaces.Register;
using Business.ViewModels.Main;
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
    public class CashRegisterViewModel : ViewModelBase, ICashRegisterViewModel
    {
        #region Properties

        private string _cashierName;
        public string CashierName
        {
            get => _cashierName;
            set => SetProperty(ref _cashierName, value);
        }

        private string _branchOfficeName;
        public string BranchOfficeName
        {
            get => _branchOfficeName;
            set => SetProperty(ref _branchOfficeName, value);
        }

        private IOrderDetailViewModel _selectedOrderDetailViewModel;
        public IOrderDetailViewModel SelectedOrderDetailViewModel
        {
            get => _selectedOrderDetailViewModel;
            set => SetProperty(ref _selectedOrderDetailViewModel, value);
        }

        public ObservableCollection<IOrderDetailViewModel> OrderDetailViewModels { get; set; }

        public IPrinterViewModel PrinterViewModel { get; set; }

        public IOrderViewModel OrderViewModel { get; set; }

        public IChargeViewModel ChargeViewModel { get; set; }


        public IOrderListViewModel OrderListViewModel { get; set; }

        public IPrintTicketViewModel PrintTicketViewModel { get; set; }

        public ICashRegisterInfoViewModel CashRegisterInfoViewModel { get; set; }

        public IInitialCashRegisterViewModel InitialCashRegisterViewModel { get; set; }


        #endregion

        #region Commands

        public ICommand PrinterCommand { get; set; }

        public ICommand NewOrderCommand { get; set; }

        public ICommand OpenCashRegisterCommand { get; set; }

        public ICommand ReprintTicketCommand { get; set; }

        public ICommand CashRegisterInfoCommand { get; set; }

        #endregion

        private CashRegister _cashRegister;
        private readonly ILogService _logService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly ICashRegisterRepository _cashRegisterRepository;
        private readonly Func<IOrderDetailViewModel> _orderDetailViewModelCreator;

        public CashRegisterViewModel(
            ILogService logService,
            IDataService dataService,
            IDialogService dialogService,
            IOrderViewModel orderViewModel,
            IChargeViewModel chargeViewModel,
            IEventAggregator eventAggregator,
            IPrinterViewModel printerViewModel,
            IOrderListViewModel orderListViewModel,
            IPrintTicketViewModel printTicketViewModel,
            ICashRegisterRepository cashRegisterRepository,
            ICashRegisterInfoViewModel cashRegisterInfoViewModel,
            Func<IOrderDetailViewModel> orderDetailViewModelCreator,
            IInitialCashRegisterViewModel initialCashRegisterViewModel)
            : base(dialogService)
        {
            _logService = logService;
            _dataService = dataService;
            _dialogService = dialogService;
            _cashRegisterRepository = cashRegisterRepository;
            _orderDetailViewModelCreator = orderDetailViewModelCreator;

            OrderViewModel = orderViewModel;
            ChargeViewModel = chargeViewModel;
            PrinterViewModel = printerViewModel;
            OrderListViewModel = orderListViewModel;
            PrintTicketViewModel = printTicketViewModel;
            CashRegisterInfoViewModel = cashRegisterInfoViewModel;
            InitialCashRegisterViewModel = initialCashRegisterViewModel;

            OrderDetailViewModels = new ObservableCollection<IOrderDetailViewModel>();

            PrinterCommand = new DelegateCommand(() => PrinterViewModel.Open());
            ReprintTicketCommand = new DelegateCommand(() => PrintTicketViewModel.Open());
            CashRegisterInfoCommand = new DelegateCommand(() => CashRegisterInfoViewModel.Open());
            NewOrderCommand = new DelegateCommand(() => OrderViewModel.Open(new Order(), new List<OrderDetail>()));
            OpenCashRegisterCommand = new DelegateCommand(() => InitialCashRegisterViewModel.Open(), () => _cashRegister == null);

            eventAggregator.GetEvent<SelectOrderEvent>().Subscribe(OnSelectOrder);
            eventAggregator.GetEvent<CloseOrderDetailEvent>().Subscribe(OnCloseOrderDetail);

        }

        public async void Load()
        {
            try
            {
                ClearValues();
                CashierName = _dataService.User.Name;
                BranchOfficeName = _dataService.CurrentOffice.Name;

                var officeId = _dataService.CurrentOffice.Id;
                var httpResponse = await ShowProgressAsync(async () => await _cashRegisterRepository.GetCashRegisterByOfficesAsync(officeId));
                if (httpResponse == null) return;

                if (httpResponse.IsSuccess)
                {
                    _cashRegister = httpResponse.Value.CashRegister;
                    ((DelegateCommand)OpenCashRegisterCommand).RaiseCanExecuteChanged();

                    if (_cashRegister != null)
                    {
                        _dataService.SetCashRegister(_cashRegister);
                        _dataService.SetPaymentTypes(httpResponse.Value.PaymentTypes);
                        OrderViewModel.Load(httpResponse.Value.Products, httpResponse.Value.Categories);
                        OrderListViewModel.Load(httpResponse.Value.Orders);
                    }
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

        private void ClearValues()
        {
            CashierName = string.Empty;
            BranchOfficeName = string.Empty;
            OrderDetailViewModels.Clear();
        }

        private async void OnSelectOrder(Order order)
        {
            var detail = OrderDetailViewModels.SingleOrDefault(x => x.Order.Id == order.Id);
            if (detail == null)
            {
                detail = _orderDetailViewModelCreator();
                await detail.LoadAsync(order);
                OrderDetailViewModels.Insert(0, detail);
            }

            SelectedOrderDetailViewModel = detail;
        }

        private void OnCloseOrderDetail(int id)
        {
            var detail = OrderDetailViewModels.SingleOrDefault(x => x.Order.Id == id);
            if (detail != null) OrderDetailViewModels.Remove(detail);
        }

    }
}
