using Business.Interfaces.Register;
using Business.ViewModels.Main;
using Prism.Commands;
using Prism.Events;
using Repository.Interfaces;
using Service.Interfaces;
using System;

namespace Business.ViewModels.Register
{
    public class PrintTicketViewModel : ModalBase, IPrintTicketViewModel
    {

        #region Properties

        private int _orderNumber;

        public int OrderNumber
        {
            get => _orderNumber;
            set
            {
                SetProperty(ref _orderNumber, value);
                ((DelegateCommand)OkCommand).RaiseCanExecuteChanged();
            }
        }

        public IPrintingService PrintingService => _printingService;

        #endregion

        private readonly ILogService _logService;
        private readonly IDataService _dataService;
        private readonly IPrintingService _printingService;
        private readonly IOrderRepository _orderRepository;
        public PrintTicketViewModel(
            ILogService logService,
            IDataService dataService,
            IDialogService dialogService,
            IPrintingService printingService,
            IEventAggregator eventAggregator,
            IOrderRepository orderRepository)
            : base(dialogService, eventAggregator)
        {
            _logService = logService;
            _dataService = dataService;
            _printingService = printingService;
            _orderRepository = orderRepository;
        }

        public void Open()
        {
            IsOpen = true;
            OrderNumber = 0;
        }

        protected override async void OnOkExecute()
        {
            try
            {
                var restaurantId = _dataService.Restaurant.Id;
                var httpResponse = await _orderRepository.GetTicketAsync(restaurantId, OrderNumber);
                if (httpResponse.IsSuccess)
                {
                    PrintingService.Print(httpResponse.Value);
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
        }

        protected override bool OnOkCanExecute() => OrderNumber > 0;
    }
}
