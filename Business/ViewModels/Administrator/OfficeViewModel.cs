using Business.Events;
using Business.Interfaces.Administrator;
using Business.Wrappers;
using MahApps.Metro.Controls.Dialogs;
using Model.Models;
using Prism.Commands;
using Prism.Events;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Business.ViewModels.Administrator
{
    public class OfficeViewModel : ViewModelBase, IOfficeViewModel
    {
        #region Properties

        private byte[] _restaurantImage;
        public byte[] RestaurantImage
        {
            get => _restaurantImage;
            set => SetProperty(ref _restaurantImage, value);
        }

        public ObservableCollection<BranchOfficeWrapper> Offices { get; set; }

        #endregion

        #region Commands

        public ICommand NewOfficeCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        public ICommand UpdateOfficeCommand { get; set; }

        public ICommand RemoveOfficeCommand { get; set; }

        #endregion

        private readonly ILogService _logService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IOfficeDetailViewModel _officeDetailViewModel;

        public OfficeViewModel(
            ILogService logService,
            IDataService dataService,
            IDialogService dialogService,
            IEventAggregator eventAggregator,
            IRestaurantRepository restaurantRepository,
            IOfficeDetailViewModel officeDetailViewModel)
            : base(dialogService)
        {
            _logService = logService;
            _dataService = dataService;
            _dialogService = dialogService;
            _restaurantRepository = restaurantRepository;
            _officeDetailViewModel = officeDetailViewModel;

            Offices = new ObservableCollection<BranchOfficeWrapper>();

            NewOfficeCommand = new DelegateCommand(OnNewOfficeExecute, () => !IsBusy);
            RefreshCommand = new DelegateCommand(InitializeBranchOffices, () => !IsBusy);
            RemoveOfficeCommand = new DelegateCommand<int?>(OnRemoveOfficeExecute);
            UpdateOfficeCommand = new DelegateCommand<BranchOfficeWrapper>(OnUpdateOfficeExecute);

            eventAggregator.GetEvent<AfterBranchOfficeSalvedEvent>().Subscribe(OnAfterBranchOfficeSalved);
        }

        public void Load()
        {
            ClearValues();
            InitializeBranchOffices();
        }

        private void ClearValues()
        {
            Offices.Clear();
            RestaurantImage = null;
        }

        private async void InitializeBranchOffices()
        {
            try
            {
                var restaurantId = _dataService.Restaurant.Id;
                var httpResponse = await ActionAsync(async () => await _restaurantRepository.GetOfficesAsync(restaurantId));
                if (httpResponse == null) return;

                if (httpResponse.IsSuccess)
                {
                    RestaurantImage = _dataService.Restaurant.Image;

                    Offices.Clear();
                    foreach (var office in httpResponse.Value)
                    {
                        Offices.Add(new BranchOfficeWrapper(office));
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
                ((DelegateCommand)NewOfficeCommand).RaiseCanExecuteChanged();
            }
        }

        private async void OnRemoveOfficeExecute(int? id)
        {
            try
            {
                if (!id.HasValue) return;
                var result = await _dialogService.AskQuestionAsync("Elimiar Sucursal",
                    "¿Estas seguro de querer eliminar esta sucursal?");
                if (result == MessageDialogResult.Negative) return;

                var httpResponse = await ActionAsync(async () => await _restaurantRepository.DeleteBranchOfficeAsync(id.Value));
                if (httpResponse.IsSuccess)
                {
                    var office = Offices.Single(x => x.Id == httpResponse.Value);
                    Offices.Remove(office);
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


        private void OnUpdateOfficeExecute(BranchOfficeWrapper office)
        {
            if (office == null) return;
            _officeDetailViewModel.Open(office.Model);
        }

        private void OnNewOfficeExecute()
        {
            _officeDetailViewModel.Open(new BranchOffice());
        }

        private void OnAfterBranchOfficeSalved(BranchOffice o)
        {
            var office = Offices.SingleOrDefault(x => x.Id == o.Id);
            if (office == null) Offices.Add(new BranchOfficeWrapper(o));
            else office.Update(o);
        }



    }
}
