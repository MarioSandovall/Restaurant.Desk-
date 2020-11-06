using Business.Events.Home;
using Prism.Commands;
using Prism.Events;
using Service.Interfaces;
using System.Windows.Input;

namespace Business.ViewModels
{
    public abstract class ModalBase : ViewModelBase
    {
        #region Properties

        private bool _isOpen;
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                SetProperty(ref _isOpen, value);
                EventAggregator.GetEvent<VisibleMenuEvent>().Publish(!IsOpen);
            }
        }

        #endregion

        #region Commands

        public ICommand OkCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        #endregion

        protected readonly IDialogService DialogService;
        protected readonly IEventAggregator EventAggregator;
        protected ModalBase(
            IDialogService dialogService,
            IEventAggregator eventAggregator)
            : base(dialogService)
        {
            DialogService = dialogService;
            EventAggregator = eventAggregator;
            OkCommand = new DelegateCommand(OnOkExecute, OnOkCanExecute);
            CloseCommand = new DelegateCommand(OnCloseExecute, OnCloseCanExecute);
        }

        protected virtual void OnCloseExecute()
        {
            IsOpen = false;
            ClearValues();
        }

        protected virtual void ClearValues() { }

        protected virtual void OnOkExecute() { }

        protected virtual bool OnOkCanExecute() => true;

        protected virtual bool OnCloseCanExecute() => true;

    }
}
