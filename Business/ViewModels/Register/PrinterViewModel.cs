using Business.Interfaces.Register;
using Prism.Commands;
using Prism.Events;
using Service.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Business.ViewModels.Register
{
    public class PrinterViewModel : ModalBase, IPrinterViewModel
    {


        #region Properties

        private string _selectedPrinter;
        public string SelectedPrinter
        {
            get => _selectedPrinter;
            set
            {
                SetProperty(ref _selectedPrinter, value);
                if (SelectedPrinter != null) SelectedPrinterChanged();
            }
        }

        private bool _isPrinterActivated;
        public bool IsPrinterActivated
        {
            get => _isPrinterActivated;
            set
            {
                SetProperty(ref _isPrinterActivated, value);
                OnIsPrinterActivatedChanged();
            }
        }

        public ObservableCollection<string> Printers { get; set; }

        #endregion

        #region Commands

        public ICommand ReloadPrintersCommand { get; set; }

        #endregion

        private readonly ILogService _logService;
        private readonly IConfigService _configService;
        private readonly IPrintingService _printingService;

        public PrinterViewModel(
            ILogService logService,
            IConfigService configService,
            IDialogService dialogService,
            IPrintingService printingService,
            IEventAggregator eventAggregator)
            : base(dialogService, eventAggregator)
        {
            _logService = logService;
            _configService = configService;
            _printingService = printingService;

            Printers = new ObservableCollection<string>();

            ReloadPrintersCommand = new DelegateCommand(OnReloadPrinters);
        }

        public async void Open()
        {
            try
            {
                IsOpen = true;
                var config = _configService.GetConfiguration();
                InitializePrinters();
                IsPrinterActivated = config.IsPrinterActivated;
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await DialogService.ShowMessageAsync(ex.Message);
            }
        }

        private void InitializePrinters()
        {
            Printers.Clear();
            foreach (var printer in _printingService.GetPrinters())
            {
                Printers.Add(printer);
            }
            SelectedPrinter = _printingService.DefaultPrinter;
        }

        private async void OnReloadPrinters()
        {
            try
            {

            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await DialogService.ShowMessageAsync(ex.Message);
            }
        }

        private async void OnIsPrinterActivatedChanged()
        {
            try
            {
                _printingService.SetActive(IsPrinterActivated);
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await DialogService.ShowMessageAsync(ex.Message);
            }
        }

        private async void SelectedPrinterChanged()
        {
            try
            {
                _printingService.SetPrinter(SelectedPrinter);

            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await DialogService.ShowMessageAsync(ex.Message);
            }
        }


    }
}
