using Business.Events.Administrator;
using Business.Interfaces.Administrator;
using Business.ViewModels.Main;
using Business.Wrappers;
using Model.Models;
using Prism.Commands;
using Prism.Events;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.ComponentModel;

namespace Business.ViewModels.Administrator
{
    public class OfficeDetailViewModel : ModalBase, IOfficeDetailViewModel
    {

        #region Properties

        private BranchOfficeWrapper _office;
        public BranchOfficeWrapper Office
        {
            get => _office;
            set => SetProperty(ref _office, value);
        }

        #endregion

        private readonly ILogService _logService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IRestaurantRepository _restaurantRepository;

        public OfficeDetailViewModel(
            ILogService logService,
            IDataService dataService,
            IDialogService dialogService,
            IEventAggregator eventAggregator,
            IRestaurantRepository restaurantRepository)
            : base(dialogService, eventAggregator)
        {
            _logService = logService;
            _dataService = dataService;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _restaurantRepository = restaurantRepository;
        }

        public void Open(BranchOffice model)
        {
            Office = new BranchOfficeWrapper(model);
            Office.PropertyChanged -= OfficeOnPropertyChanged;
            Office.PropertyChanged += OfficeOnPropertyChanged;

            Office.Name = model.Name;
            Office.Town = model.Town;
            Office.Suburb = model.Suburb;
            Office.Street = model.Street;
            Office.StateProvince = model.StateProvince;
            Office.OutdoorNumber = model.OutdoorNumber;
            Office.RestaurantId = _dataService.Restaurant.Id;

            IsOpen = true;

            ((DelegateCommand)OkCommand).RaiseCanExecuteChanged();
        }

        private void OfficeOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Office.HasErrors))
            {
                ((DelegateCommand)OkCommand).RaiseCanExecuteChanged();
            }
        }

        protected override async void OnOkExecute()
        {
            try
            {
                var httpResponse = await ActionAsync(async () => await _restaurantRepository.SaveBranchOfficeAsync(Office.Model));
                if (httpResponse.IsSuccess)
                {
                    OnCloseExecute();
                    _eventAggregator.GetEvent<AfterBranchOfficeSalvedEvent>().Publish(httpResponse.Value);
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

        protected override bool OnOkCanExecute() => Office != null && !Office.HasErrors;

    }
}
