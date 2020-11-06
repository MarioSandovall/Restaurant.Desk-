using Business.Interfaces.Register;
using Model.Dtos;
using Model.Models;
using Model.Utils;
using Prism.Events;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.ObjectModel;

namespace Business.ViewModels.Register
{
    public class CashRegisterInfoViewModel : ModalBase, ICashRegisterInfoViewModel
    {
        #region Properites

        private CashRegister _cashRegister;
        public CashRegister CashRegister
        {
            get => _cashRegister;
            set => SetProperty(ref _cashRegister, value);
        }

        private decimal _total;
        public decimal Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }


        private decimal _totalSold;
        public decimal Totalsold
        {
            get => _totalSold;
            set => SetProperty(ref _totalSold, value);
        }

        private int _cancelOrders;
        public int CanceledOrders
        {
            get => _cancelOrders;
            set => SetProperty(ref _cancelOrders, value);
        }

        private int _pendingOrders;
        public int PendingOrders
        {
            get => _pendingOrders;
            set => SetProperty(ref _pendingOrders, value);
        }

        private int _deliveredOrders;
        public int DeliveredOrders
        {
            get => _deliveredOrders;
            set => SetProperty(ref _deliveredOrders, value);
        }



        public ObservableCollection<OrderInfoDto> Orders { get; set; }

        #endregion

        private readonly ILogService _logService;
        private readonly IDataService _dataService;
        private readonly ICashRegisterRepository _cashRegisterRepository;
        public CashRegisterInfoViewModel(
            ILogService logService,
            IDataService dataService,
            IDialogService dialogService,
            IEventAggregator eventAggregator,
            ICashRegisterRepository cashRegisterRepository)
            : base(dialogService, eventAggregator)
        {
            _logService = logService;
            _dataService = dataService;
            _cashRegisterRepository = cashRegisterRepository;

            Orders = new ObservableCollection<OrderInfoDto>();
        }

        public void Open()
        {
            IsOpen = true;
            ClearValues();
            InitializeInfo();
        }

        protected override void ClearValues()
        {
            Total = 0;
            Totalsold = 0;
            PendingOrders = 0;
            CanceledOrders = 0;
            DeliveredOrders = 0;
            Orders.Clear();
        }

        private async void InitializeInfo()
        {
            try
            {
                IsBusy = true;
                CashRegister = _dataService.CashRegister;
                Total += CashRegister.Quantity;
                var httpResponse = await _cashRegisterRepository.GetCashRegisterInfoAsync(CashRegister.Id);
                if (httpResponse.IsSuccess)
                {
                    foreach (var order in httpResponse.Value)
                    {
                        switch (order.Status)
                        {
                            case OrderStatusEnum.Cancel:
                                CanceledOrders++;
                                break;
                            case OrderStatusEnum.Pending:
                                PendingOrders++;
                                break;
                            case OrderStatusEnum.Delivered:
                                Totalsold += order.Total;
                                DeliveredOrders++;
                                break;
                        }
                        Orders.Add(order);
                    }

                    Total += Totalsold;
                }
                else
                {
                    await DialogService.ShowMessageAsync(httpResponse.Message);
                }
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await DialogService.ShowMessageAsync(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
