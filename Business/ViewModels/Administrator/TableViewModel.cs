using Business.Events.Administrator;
using Business.Interfaces.Administrator;
using Business.ViewModels.Main;
using Business.Wrappers;
using MahApps.Metro.Controls.Dialogs;
using Model.Models;
using Prism.Commands;
using Prism.Events;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Business.ViewModels.Administrator
{
    public class TableViewModel : ViewModelBase, ITableViewModel
    {

        #region Properties

        public ObservableCollection<TableWrapper> Tables { get; set; }

        #endregion

        #region Commands

        public ICommand NewTableCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        public ICommand UpdateTableCommand { get; set; }

        public ICommand RemoveTableCommand { get; set; }

        #endregion
        private ICollection<BranchOffice> _offices;
        private readonly ILogService _logService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly ITableRepository _tableRepository;
        private readonly ITableDetailViewMoldel _tableDetailViewModel;
        public TableViewModel(
            ILogService logService,
            IDataService dataService,
            IDialogService dialogService,
            ITableRepository tableRepository,
            IEventAggregator eventAggregator,
            ITableDetailViewMoldel tableDetailViewModel)
            : base(dialogService)
        {
            _logService = logService;
            _dataService = dataService;
            _dialogService = dialogService;
            _tableRepository = tableRepository;
            _tableDetailViewModel = tableDetailViewModel;

            _offices = new List<BranchOffice>();

            Tables = new ObservableCollection<TableWrapper>();

            RefreshCommand = new DelegateCommand(Load, () => !IsBusy);
            RemoveTableCommand = new DelegateCommand<int?>(OnRemoveTableExecute);
            NewTableCommand = new DelegateCommand(OnNewTableExecute, () => !IsBusy);
            UpdateTableCommand = new DelegateCommand<TableWrapper>(OnUpdateTableExecute);

            eventAggregator.GetEvent<AfterTableSalvedEvent>().Subscribe(OnAfterTableSalved);
        }

        public async void Load()
        {
            try
            {
                var restaurantId = _dataService.Restaurant.Id;
                var httpResponse = await ActionAsync(async () => await _tableRepository.GetTablesAsync(restaurantId));
                if (httpResponse == null) return;

                if (httpResponse.IsSuccess)
                {
                    Tables.Clear();
                    _offices = httpResponse.Value.Offices;
                    var tables = httpResponse.Value.Tables;
                    foreach (var table in tables)
                    {
                        Tables.Add(new TableWrapper(table));
                    }
                }
                else
                {
                    await _dialogService.ShowMessageAsync(httpResponse.Message, httpResponse.Title);
                }
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await _dialogService.ShowMessageAsync(ex.Message);
            }
            finally
            {
                ((DelegateCommand)RefreshCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)NewTableCommand).RaiseCanExecuteChanged();
            }
        }

        private void OnUpdateTableExecute(TableWrapper table)
        {
            if (table == null) return;
            _tableDetailViewModel.Open(table.Model, _offices);
        }

        private async void OnRemoveTableExecute(int? id)
        {
            try
            {
                if (!id.HasValue) return;
                var result = await _dialogService.AskQuestionAsync("¿Estas seguro de querer eliminar esta mesa?", "Elimiar Mesa");
                if (result == MessageDialogResult.Negative) return;

                var httpResponse = await ActionAsync(async () => await _tableRepository.DeleteTablesAsync(id.Value));
                if (httpResponse.IsSuccess)
                {
                    var table = Tables.Single(x => x.Id == httpResponse.Value);
                    Tables.Remove(table);
                }
                else
                {
                    await _dialogService.ShowMessageAsync(httpResponse.Message, httpResponse.Title);
                }
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await _dialogService.ShowMessageAsync(ex.Message);
            }
        }

        private void OnNewTableExecute()
        {
            _tableDetailViewModel.Open(new Table(), _offices);
        }

        private void OnAfterTableSalved(Table t)
        {
            var table = Tables.SingleOrDefault(x => x.Id == t.Id);
            if (table == null) Tables.Add(new TableWrapper(t));
            else table.Update(t);
        }
    }
}
