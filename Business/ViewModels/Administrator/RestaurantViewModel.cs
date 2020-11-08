using Business.Events.Administrator;
using Business.Interfaces.Administrator;
using Business.ViewModels.Main;
using Business.Wrappers;
using Microsoft.Win32;
using Model.Models;
using Prism.Commands;
using Prism.Events;
using Repository.Interfaces;
using Service.Extensions;
using Service.Interfaces;
using Service.Utils;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Business.ViewModels.Administrator
{
    public class RestaurantViewModel : ViewModelBase, IRestaurantViewModel
    {
        #region Properties

        private RestaurantWrapper _restaurant;
        public RestaurantWrapper Restaurant
        {
            get => _restaurant;
            set => SetProperty(ref _restaurant, value);
        }


        #endregion

        #region Commands

        public ICommand SaveCommand { get; set; }

        public ICommand LoadImageCommand { get; set; }

        public ICommand DeleteImageCommand { get; set; }

        #endregion

        private readonly ILogService _logService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IRestaurantRepository _restaurantRepository;
        public RestaurantViewModel(
            ILogService logService,
            IDataService dataService,
            IDialogService dialogService,
            IEventAggregator eventAggregator,
            IRestaurantRepository restaurantRepository)
            : base(dialogService)
        {
            _logService = logService;
            _dataService = dataService;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _restaurantRepository = restaurantRepository;

            LoadImageCommand = new DelegateCommand(OnLoadImageExecute);
            DeleteImageCommand = new DelegateCommand(OnDeleteImageExecute);
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        public void Load() => InitializeRestaurant();

        private void InitializeRestaurant()
        {
            var model = _dataService.Restaurant;
            Restaurant = new RestaurantWrapper(new Restaurant(model));

            Restaurant.PropertyChanged -= RestaurantOnPropertyChanged;
            Restaurant.PropertyChanged += RestaurantOnPropertyChanged;

            Restaurant.Name = model.Name;
            Restaurant.Image = model.Image;
        }

        private void RestaurantOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Restaurant.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private async void OnSaveExecute()
        {
            try
            {
                var httpResponse = await ShowProgressAsync(async () => await _restaurantRepository.UpdateRestaurantAsync(Restaurant.Model));
                if (httpResponse.IsSuccess)
                {
                    _dataService.SetRestaurant(Restaurant.Model);
                    _eventAggregator.GetEvent<AfterRestaurantSavedEvent>().Publish();
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

        private bool OnSaveCanExecute() => Restaurant != null && !Restaurant.HasErrors;

        private void OnLoadImageExecute()
        {
            var openFile = new OpenFileDialog
            {
                Title = "Select an image",
                Filter = "All | *.jpg; *.jpeg; *.png"
            };

            if (openFile.ShowDialog() == true)
            {
                Restaurant.Image = openFile.FileName.ImgUrlToByteArray();
            }
        }

        private void OnDeleteImageExecute() => Restaurant.Image = Images.Restaurant.ImgUrlToByteArray();


    }
}
