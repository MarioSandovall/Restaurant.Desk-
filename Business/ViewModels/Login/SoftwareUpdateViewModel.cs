using Business.Interfaces.Login;
using Prism.Events;
using Service.Interfaces;
using System;
using System.Deployment.Application;
using System.Threading.Tasks;
using System.Windows;

namespace Business.ViewModels.Login
{
    public class SoftwareUpdateViewModel : ModalBase, ISoftwareUpdateViewModel
    {

        #region Properties

        private bool _isNotUpdateRequired;

        public bool IsNotUpdateRequired
        {
            get => _isNotUpdateRequired;
            set => SetProperty(ref _isNotUpdateRequired, value);
        }

        #endregion

        private readonly ILogService _logService;
        private readonly IDialogService _dialogService;
        public SoftwareUpdateViewModel(
            ILogService logService,
            IDialogService dialogService,
            IEventAggregator eventAggregator)
            : base(dialogService, eventAggregator)
        {
            _logService = logService;
            _dialogService = dialogService;
            IsOpen = false;
        }

        public async Task<bool> LoadAsync()
        {
            try
            {
                await Task.Delay(2000);
                IsNotUpdateRequired = false;
                return await IsThereNewAppVersionAsync();
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                return false;
            }
        }

        public async void CheckForUpdates()
        {
            try
            {
                IsNotUpdateRequired = true;
                var result = await ActionAsync(async () => await IsThereNewAppVersionAsync());
                if (!result)
                {
                    await _dialogService.ShowMessageAsync
                        ("No hay nuevas actualizaciones disponibles por el momento", "Actualización de software");
                }
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                while (ex.InnerException != null) ex = ex.InnerException;
                await _dialogService.ShowMessageAsync(ex.Message, "Update Error");
            }
        }

        private async Task<bool> IsThereNewAppVersionAsync()
        {
            return IsOpen = await Task.Run(() =>
            {
                if (!ApplicationDeployment.IsNetworkDeployed) return false;
                return ApplicationDeployment.CurrentDeployment.CheckForUpdate();
            });
        }

        protected override async void OnOkExecute()
        {
            try
            {
                await ActionAsync(async () =>
                await Task.Run(() => ApplicationDeployment.CurrentDeployment.Update()),
                "Actualizando software");
                Application.Current.Shutdown();
                System.Windows.Forms.Application.Restart();
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await _dialogService.ShowMessageAsync(ex.Message, "Update Error");
            }
        }

        protected override void OnCloseExecute() => IsOpen = false;

    }
}
