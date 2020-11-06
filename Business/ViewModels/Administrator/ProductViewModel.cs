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
    public class ProductViewModel : ViewModelBase, IProductViewModel
    {
        #region Properties

        public ObservableCollection<ProductWrapper> Products { get; set; }

        #endregion

        #region Commands

        public ICommand NewProductCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        public ICommand UpdateProductCommand { get; set; }

        public ICommand DeleteProductCommand { get; set; }

        #endregion

        private IList<Category> _categories;
        private readonly ILogService _logService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IProductRepository _productRepository;
        private readonly IProductDetailViewModel _productDetailViewModel;
        public ProductViewModel(
            ILogService logService,
            IDataService dataService,
            IDialogService dialogService,
            IEventAggregator eventAggregator,
            IProductRepository productRepository,
            IProductDetailViewModel productDetailViewModel)
            : base(dialogService)
        {
            _logService = logService;
            _dataService = dataService;
            _dialogService = dialogService;
            _productRepository = productRepository;
            _productDetailViewModel = productDetailViewModel;
            Products = new ObservableCollection<ProductWrapper>();

            RefreshCommand = new DelegateCommand(Load, () => !IsBusy);
            NewProductCommand = new DelegateCommand(OnNewProductExecute, () => !IsBusy);
            DeleteProductCommand = new DelegateCommand<int?>(OnDeleteProductExecute);
            UpdateProductCommand = new DelegateCommand<ProductWrapper>(OnUpdateProductExecute);
            eventAggregator.GetEvent<AfterProductSavedEvent>().Subscribe(OnAfterProductSaved);
        }

        public async void Load()
        {
            try
            {
                var restaurantId = _dataService.Restaurant.Id;
                var httpResponse = await ShowProgressAsync(async () => await _productRepository.GetProductsWithCategoriesAsync(restaurantId));
                if (httpResponse == null) return;

                if (httpResponse.IsSuccess)
                {
                    var products = httpResponse.Value.Products;
                    _categories = httpResponse.Value.ProductCategories.ToList();

                    Products.Clear();
                    foreach (var product in products)
                    {
                        Products.Add(new ProductWrapper(product));
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
                ((DelegateCommand)NewProductCommand).RaiseCanExecuteChanged();
            }
        }

        private void OnNewProductExecute()
        {
            _productDetailViewModel.Open(new Product(), _categories);
        }

        private void OnUpdateProductExecute(ProductWrapper product)
        {
            if (product == null) return;
            _productDetailViewModel.Open(product.Model, _categories);
        }

        private async void OnDeleteProductExecute(int? id)
        {
            try
            {
                if (!id.HasValue) return;
                var result = await _dialogService.AskQuestionAsync("¿Estas seguro de querer eliminar este producto?", "Elimiar Producto");
                if (result == MessageDialogResult.Negative) return;

                var httpResponse = await ShowProgressAsync(async () => await _productRepository.DeleteProductAsync(id.Value));
                if (httpResponse.IsSuccess)
                {
                    var product = Products.Single(x => x.Id == httpResponse.Value);
                    Products.Remove(product);
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

        private void OnAfterProductSaved(Product p)
        {
            var product = Products.SingleOrDefault(x => x.Id == p.Id);
            if (product == null) Products.Add(new ProductWrapper(p));
            else product.Update(p);
        }
    }
}
