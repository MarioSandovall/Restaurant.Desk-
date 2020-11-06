using Business.Interfaces.Register;
using Model.Models;
using Prism.Events;
using Repository.Interfaces;
using Service.Interfaces;
using System;

namespace Business.ViewModels.Register
{
    public class InitialCashRegisterViewModel : ModalBase, IInitialCashRegisterViewModel
    {

        #region Properties

        private decimal _quantity;
        public decimal Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }

        #endregion

        private readonly ILogService _logService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private readonly ICashRegisterRepository _cashRegisterRepository;
        public InitialCashRegisterViewModel(
            ILogService logService,
            IDataService dataService,
            IDialogService dialogService,
            IEventAggregator eventAggregator,
            ICashRegisterRepository cashRegisterRepository)
            : base(dialogService, eventAggregator)
        {
            _logService = logService;
            _dataService = dataService;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _cashRegisterRepository = cashRegisterRepository;
        }

        public void Open() => IsOpen = true;

        protected override async void OnOkExecute()
        {
            try
            {
                var model = new CashRegister()
                {
                    UserId = _dataService.User.Id,
                    BranchOfficeId = _dataService.CurrentOffice.Id,
                    Quantity = Quantity
                };

                var httpResponse = await ActionAsync(async () => await _cashRegisterRepository.SetCashRegisterAsync(model));
                if (httpResponse == null) return;

                if (httpResponse.IsSuccess)
                {
                    IsOpen = false;
                    Quantity = 0;
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

    }
}
