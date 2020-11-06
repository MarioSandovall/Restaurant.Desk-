using Business.Interfaces.Register;
using Business.Wrappers;
using Model.Models;
using Prism.Events;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Business.ViewModels.Register
{
    public class SearchProductViewModel : ViewModelBase, ISearchProductViewModel
    {
        #region Properties

        public ObservableCollection<ProductWrapper> Products { get; set; }

        public ObservableCollection<Category> Categories { get; set; }

        private Category _selectedCategory;

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                SetProperty(ref _selectedCategory, value);
                if (SelectedCategory == null) return;
                OnSelectedCategoryChanged();
            }
        }

        private string _filterProducts;
        public string FilterProducts
        {
            get => _filterProducts;
            set
            {
                SetProperty(ref _filterProducts, value);
                OnFilterProductsChanged();
            }
        }
        #endregion

        private IList<Product> _allProducts;
        private readonly ILogService _logService;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        public SearchProductViewModel(
            ILogService logService,
            IDialogService dialogService,
            IEventAggregator eventAggregator) : base(dialogService)
        {
            _logService = logService;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;

            Products = new ObservableCollection<ProductWrapper>();
            Categories = new ObservableCollection<Category>();
            _allProducts = new List<Product>();

        }

        public async void Load(ICollection<Product> products, ICollection<Category> categories)
        {
            try
            {
                FilterProducts = string.Empty;
                _allProducts = products.ToList();

                AddProducts(_allProducts);
                AddCategories(categories);
            }
            catch (Exception ex)
            {
                _logService.Write(ex);
                await _dialogService.ShowMessageAsync(ex.Message);
            }
        }

        private void OnFilterProductsChanged()
        {
            if (!_allProducts.Any()) return;
            Products.Clear();
            var products = string.IsNullOrEmpty(FilterProducts)
                ? _allProducts : _allProducts.Where(x => x.Name.ToUpper().Contains(FilterProducts.ToUpper()));
            AddProducts(products);
        }

        private void OnSelectedCategoryChanged()
        {
            if (!_allProducts.Any()) return;
            Products.Clear();
            var products = SelectedCategory.Id == 0
                ? _allProducts : _allProducts.Where(x => x.ProductCategoryId == SelectedCategory.Id);
            AddProducts(products);
        }

        private void AddProducts(IEnumerable<Product> products)
        {
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(new ProductWrapper(product, _eventAggregator));
            }
        }

        private void AddCategories(IEnumerable<Category> categories)
        {
            Categories.Clear();
            Categories.Add(new Category() { Id = 0, Name = "Todos" });

            foreach (var category in categories)
            {
                Categories.Add(category);
            }
        }

    }
}
