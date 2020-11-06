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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Business.ViewModels.Administrator
{
    public class ProductDetailViewModel : ModalBase, IProductDetailViewModel
    {
        #region Properties

        private ProductWrapper _product;

        public ProductWrapper Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }

        public ObservableCollection<Category> Categories { get; set; }

        #endregion

        #region Commands     

        public ICommand LoadImageCommand { get; set; }

        public ICommand DeleteImageCommand { get; set; }

        #endregion

        private readonly ILogService _logService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IProductRepository _productRepository;

        public ProductDetailViewModel(
            ILogService logService,
            IDataService dataService,
            IDialogService dialogService,
            IEventAggregator eventAggregator,
            IProductRepository productRepository)
            : base(dialogService, eventAggregator)
        {
            _logService = logService;
            _dataService = dataService;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _productRepository = productRepository;

            Categories = new ObservableCollection<Category>();

            LoadImageCommand = new DelegateCommand(OnLoadImageExecute);
            DeleteImageCommand = new DelegateCommand(OnDeleteImageExecute);
        }

        public void Open(Product product, ICollection<Category> categories)
        {
            InitializeCategories(categories);
            InitializeProduct(product);
            IsOpen = true;
        }

        private void InitializeCategories(IEnumerable<Category> categories)
        {
            Categories.Clear();
            foreach (var category in categories)
            {
                Categories.Add(category);
            }
        }

        private void OnLoadImageExecute()
        {
            var openFile = new OpenFileDialog
            {
                Title = "Select an image",
                Filter = "All | *.jpg; *.jpeg; *.png"
            };

            if (openFile.ShowDialog() == true)
            {
                Product.Image = openFile.FileName.ImgUrlToByteArray();
            }
        }

        private void InitializeProduct(Product model)
        {
            Product = new ProductWrapper(model);

            Product.PropertyChanged -= WrapperOnPropertyChanged;
            Product.PropertyChanged += WrapperOnPropertyChanged;

            //Trigger to validate properties
            Product.Name = model.Name;
            Product.Price = model.Price;
            Product.Description = model.Description;
            Product.RestaurantId = _dataService.Restaurant.Id;
            Product.ProductCategoryId = model.ProductCategoryId;
            Product.Image = model.Image ?? RestaurantImages.Product.ImgUrlToByteArray();

            ((DelegateCommand)OkCommand).RaiseCanExecuteChanged();
        }

        private void OnDeleteImageExecute()
        {
            Product.Image = RestaurantImages.Product.ImgUrlToByteArray();
        }

        private void WrapperOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Product.HasErrors))
            {
                ((DelegateCommand)OkCommand).RaiseCanExecuteChanged();
            }
        }

        protected override void ClearValues() => Product = null;

        protected override async void OnOkExecute()
        {
            try
            {
                var httpResponse = await ActionAsync(async () =>
                    await _productRepository.SaveProductAsync(Product.Model));

                if (httpResponse.IsSuccess)
                {
                    _eventAggregator.GetEvent<AfterProductSavedEvent>().Publish(httpResponse.Value);
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
            return Product != null && !Product.HasErrors;
        }
    }
}