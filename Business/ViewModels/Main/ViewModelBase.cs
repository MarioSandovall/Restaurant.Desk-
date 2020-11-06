using Autofac;
using Business.Startup;
using MahApps.Metro.Controls.Dialogs;
using Prism.Mvvm;
using Service.Interfaces;
using System;
using System.Threading.Tasks;

namespace Business.ViewModels.Main
{
    public class ViewModelBase : BindableBase
    {

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private ProgressDialogController _progress;

        private readonly IDialogService _dialogService;
        protected ViewModelBase(IDialogService dialogService)
        {
            IsBusy = false;
            _dialogService = dialogService;
        }

        protected async Task<T> ActionAsync<T>(Func<Task<T>> action, string message = "")
        {
            try
            {
                if (IsBusy) return default(T);

                IsBusy = true;
                _progress = await _dialogService.ShowProgressAsync(message);
                _progress.SetCancelable(true);
                _progress.Canceled += ProgressOnCanceled;
                _progress.SetIndeterminate();

                var result = await Task.Run(async () => await action());

                await _progress.CloseAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                IsBusy = false;
                if (_progress != null && _progress.IsOpen) await _progress.CloseAsync();
            }
        }

        protected async Task ActionAsync(Func<Task> action, string message = "")
        {
            try
            {
                if (IsBusy) return;

                IsBusy = true;
                _progress = await _dialogService.ShowProgressAsync(message);
                _progress.SetIndeterminate();
                _progress.SetCancelable(true);
                _progress.Canceled += ProgressOnCanceled;

                await Task.Run(async () => await action());

                await _progress.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                IsBusy = false;
                if (_progress != null && _progress.IsOpen) await _progress.CloseAsync();
            }
        }

        private void ProgressOnCanceled(object sender, EventArgs e)
        {
            if (!_progress.IsCanceled) return;
            var webService = Bootstrapper.Instance.Container.Resolve<IWebService>();
            webService.Client.CancelPendingRequests();
        }

    }
}
