using Business.Interfaces.Login;
using Business.ViewModels.Main;
using Model.Models;
using Prism.Commands;
using Prism.Events;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Business.ViewModels.Login
{
    public class OfficeChooserViewModel : ModalBase, IOfficeChooserViewModel
    {
        #region Properties

        public ObservableCollection<BranchOffice> BranchOffices { get; set; }

        private BranchOffice _selectedBranchOffice;
        public BranchOffice SelectedBranchOffice
        {
            get => _selectedBranchOffice;
            set
            {
                SetProperty(ref _selectedBranchOffice, value);
                ((DelegateCommand)OkCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)OkCommand).RaiseCanExecuteChanged();
            }
        }

        #endregion

        private readonly ILogService _logService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        public OfficeChooserViewModel(
            ILogService logService,
            IDataService dataService,
            IDialogService dialogService,
            IEventAggregator eventAggregator)
            : base(dialogService, eventAggregator)
        {
            _logService = logService;
            _dataService = dataService;
            _dialogService = dialogService;
        }

        public void Open(IEnumerable<BranchOffice> branchOffices)
        {
            BranchOffices = new ObservableCollection<BranchOffice>(branchOffices);
            IsOpen = true;
        }

        protected override async void OnOkExecute()
        {
            try
            {
                _dataService.SetBranchOffice(SelectedBranchOffice);
                OnCloseExecute();
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await _dialogService.ShowMessageAsync(ex.Message);
            }
        }

        protected override bool OnOkCanExecute() => SelectedBranchOffice != null;

    }
}
