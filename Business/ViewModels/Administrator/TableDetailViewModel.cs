using Business.Events.Administrator;
using Business.Interfaces.Administrator;
using Business.ViewModels.Main;
using Business.Wrappers;
using Microsoft.Win32;
using Model.Models;
using Prism.Commands;
using Prism.Events;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Service.Extensions;
using Service.Utils;

namespace Business.ViewModels.Administrator
{
    public class TableDetailViewModel : ModalBase, ITableDetailViewMoldel
    {
        #region Properties

        private TableWrapper _table;
        public TableWrapper Table
        {
            get => _table;
            set => SetProperty(ref _table, value);
        }

        public ObservableCollection<BranchOffice> Offices { get; set; }
        #endregion

        #region Commands     

        public ICommand LoadImageCommand { get; set; }

        public ICommand DeleteImageCommand { get; set; }

        #endregion

        private readonly ILogService _logService;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private readonly ITableRepository _tableRepository;

        public TableDetailViewModel(
            ILogService logService,
            IDialogService dialogService,
            IEventAggregator eventAggregator,
            ITableRepository tableRepository)
            : base(dialogService,eventAggregator)
        {
            _logService = logService;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _tableRepository = tableRepository;

            Offices = new ObservableCollection<BranchOffice>();

            LoadImageCommand = new DelegateCommand(OnLoadImageExecute);
            DeleteImageCommand = new DelegateCommand(OnDeleteImageExecute);
        }

        public void Open(Table table, ICollection<BranchOffice> offices)
        {
            InitializareOffice(offices);
            InitializareTable(table);
            IsOpen = true;
        }

        private void InitializareOffice(ICollection<BranchOffice> offices)
        {
            Offices.Clear();
            foreach (var office in offices)
            {
                Offices.Add(office);
            }
        }

        private void InitializareTable(Table model)
        {
            Table = new TableWrapper(model);
            Table.PropertyChanged -= TableOnPropertyChanged;
            Table.PropertyChanged += TableOnPropertyChanged;

            Table.Name = model.Name;
            Table.BranchOfficeId = model.BranchOfficeId;

            ((DelegateCommand)OkCommand).RaiseCanExecuteChanged();
        }

        private void TableOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Table.HasErrors))
            {
                ((DelegateCommand)OkCommand).RaiseCanExecuteChanged();
            }
        }

        private void OnDeleteImageExecute()
        {
            Table.Image = RestaurantImages.Table.ImgUrlToByteArray();
        }

        private void OnLoadImageExecute()
        {
            var openFile = new OpenFileDialog
            {
                Title = "Selecione una imagen",
                Filter = "All | *.jpg; *.jpeg; *.png"
            };

            if (openFile.ShowDialog() == true)
            {
                Table.Image = openFile.FileName.ImgUrlToByteArray();
            }
        }

        protected override async void OnOkExecute()
        {
            try
            {
                var httpResponse = await ActionAsync(async () => await _tableRepository.SaveTablesAsync(Table.Model));
                if (httpResponse.IsSuccess)
                {
                    _eventAggregator.GetEvent<AfterTableSalvedEvent>().Publish(httpResponse.Value);
                    OnCloseExecute();
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

        protected override bool OnOkCanExecute()
        {
            return Table != null && !Table.HasErrors;
        }

        protected override void ClearValues() => Table = null;

    }
}
