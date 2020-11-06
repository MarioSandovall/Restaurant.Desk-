using Business.Events.Home;
using Prism.Commands;
using Prism.Events;
using Service.Interfaces;
using System.Windows.Input;

namespace Business.ViewModels.Main
{
    public class FlyoutBase : ViewModelBase
    {
        #region Commands

        public ICommand SaveCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        #endregion

        private readonly string _viewModelName;
        private readonly IEventAggregator _eventAggregator;
        protected FlyoutBase(string viewModelName,
            IEventAggregator eventAggregator,
            IDialogService dialogService)
            : base(dialogService)
        {
            _viewModelName = viewModelName;
            _eventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnCanSaveExecute);
            CloseCommand = new DelegateCommand(OnCloseExecute);
        }

        protected virtual void OnSaveExecute() { }

        protected virtual bool OnCanSaveExecute() => true;

        protected virtual void OnCloseExecute() => _eventAggregator.GetEvent<CloseFlyoutEvent>().Publish(_viewModelName);

    }
}
