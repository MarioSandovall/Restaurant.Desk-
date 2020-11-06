using Business.Events.Home;
using Business.Interfaces.Login;
using Model.Utils;
using Prism.Events;
using Service.Interfaces;
using System;
using System.Windows;

namespace Business.ViewModels.Login
{
    public class StartUpViewModel : IStartUpViewModel
    {


        private readonly ILogService _logService;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private readonly ISoftwareUpdateViewModel _softwareUpdateViewModel;

        public StartUpViewModel(
            ILogService logService,
            IDialogService dialogService,
            IEventAggregator eventAggregator,
            ISoftwareUpdateViewModel softwareUpdateViewModel)
        {
            _logService = logService;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _softwareUpdateViewModel = softwareUpdateViewModel;
        }

        public async void Load()
        {
            try
            {
                var result = await _softwareUpdateViewModel.LoadAsync();
                if (result) return;

                _eventAggregator.GetEvent<BeforeNavigationEvent>().Publish(MenuAction.GoToEmailLogin);
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await _dialogService.ShowMessageAsync("Internal error", ex.Message);
                Application.Current.Shutdown();
            }
        }
    }
}