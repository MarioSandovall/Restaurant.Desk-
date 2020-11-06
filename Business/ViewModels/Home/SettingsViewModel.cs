using Business.Events;
using Business.Interfaces.Home;
using MahApps.Metro;
using Model.Models;
using Prism.Commands;
using Prism.Events;
using Service.Interfaces;
using Service.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Business.ViewModels.Home
{
    public class SettingsViewModel : FlyoutBase, ISettingsViewModel
    {

        #region Properties

        public ObservableCollection<AccentColor> AccentColors { get; set; }

        //public ObservableCollection<AccentColor> Themes { get; set; }

        public ObservableCollection<string> Printers { get; set; }

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

        private AccentColor _selectedAccentColor;
        public AccentColor SelectedAccentColor
        {
            get => _selectedAccentColor;
            set
            {
                if (SelectedAccentColor == value) return;
                SetProperty(ref _selectedAccentColor, value);
                OnAccentColorChanged();
            }
        }

        private AccentColor _selectedTheme;
        public AccentColor SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (SelectedTheme == value) return;
                SetProperty(ref _selectedTheme, value);
                OnThemeChanged();
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

        #endregion

        #region Commands

        public ICommand ReloadPrintersCommand { get; set; }

        #endregion

        private readonly ILogService _logService;
        private readonly IConfigService _configService;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IPrintingService _printingService;

        public SettingsViewModel(
            ILogService logService,
            IDialogService dialogService,
            IConfigService configService,
            IEventAggregator eventAggregator,
            IPrintingService printingService)
            : base(nameof(SettingsViewModel), eventAggregator, dialogService)
        {
            _logService = logService;
            _dialogService = dialogService;
            _configService = configService;
            _eventAggregator = eventAggregator;
            _printingService = printingService;

            ReloadPrintersCommand = new DelegateCommand(OnReloadPrintersExecute);

            Printers = new ObservableCollection<string>();

            AccentColors = new ObservableCollection<AccentColor>();
        }

        public async void Load()
        {
            try
            {
                var config = _configService.GetConfiguration();

                if (!AccentColors.Any()) InitializeAccentColors();

                SelectedAccentColor = AccentColors.Single(x => x.Name == config.AccentColor);

                InitializePrinters();

                IsPrinterActivated = config.IsPrinterActivated;
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await _dialogService.ShowMessageAsync(ex.Message);
            }
        }

        private async void OnReloadPrintersExecute()
        {
            try
            {
                InitializePrinters();
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await _dialogService.ShowMessageAsync(ex.Message);
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

        private void InitializeAccentColors()
        {
            AccentColors.Clear();
            foreach (var accent in ThemeManager.Accents)
            {
                var color = new AccentColor()
                {
                    Name = accent.Name,
                    Color = accent.Resources["AccentColor"].ToString()
                };

                AccentColors.Add(color);
            }
        }

        private async void OnAccentColorChanged()
        {
            try
            {
                var accentProperty = ObjectHelper.PropertyName<ConfigApp>(x => x.AccentColor);
                _configService.UpdateProperty(accentProperty, SelectedAccentColor.Name);
                _configService.LoadTheme();
                _eventAggregator.GetEvent<UpdateAccentColorEvent>().Publish();
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await _dialogService.ShowMessageAsync(ex.Message);
            }
        }

        private async void OnThemeChanged()
        {
            try
            {
                var themeProperty = ObjectHelper.PropertyName<ConfigApp>(x => x.Theme);
                _configService.UpdateProperty(themeProperty, SelectedTheme.Name);
                _configService.LoadTheme();
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await _dialogService.ShowMessageAsync(ex.Message);
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
                await _dialogService.ShowMessageAsync(ex.Message);
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
                await _dialogService.ShowMessageAsync(ex.Message);
            }
        }
    }
}
