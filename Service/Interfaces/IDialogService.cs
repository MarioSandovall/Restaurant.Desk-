using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IDialogService
    {
        Task<MessageDialogResult> AskQuestionAsync(string message, string title = "Restaurant System");
        
        Task<ProgressDialogController> ShowProgressAsync(string message);
        
        Task ShowMessageAsync(string message, string title = "Restaurant System");
        
        Task HideDialogAsync();
    }
}
