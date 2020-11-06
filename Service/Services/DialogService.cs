using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Service.Interfaces;
using System.Threading.Tasks;

namespace Service.Services
{
    public class DialogService : IDialogService
    {
        private readonly MetroWindow _dialog;

        public DialogService(IWindowService windowService)
        {
            _dialog = windowService.Window;
        }

        public Task<MessageDialogResult> AskQuestionAsync(string message, string title)
        {
            return _dialog.ShowMessageAsync(title, message,
                MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Aceptar",
                    NegativeButtonText = "Cancelar"
                });
        }

        public Task<ProgressDialogController> ShowProgressAsync(string message)
        {
            return _dialog.ShowProgressAsync("Un momento por favor...", message);
        }

        public Task ShowMessageAsync(string message, string title)
        {
            return _dialog.ShowMessageAsync(title, message);
        }

        public async Task HideDialogAsync()
        {
            var currentDialog = await _dialog.GetCurrentDialogAsync<BaseMetroDialog>();
            if (currentDialog == null) return;
            if (currentDialog.IsVisible) await currentDialog.RequestCloseAsync();
        }
    }
}
