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
    public class CategoryViewModel : ViewModelBase, ICategoryViewModel
    {
        #region Propesties

        public ObservableCollection<CategoryWrapper> ProductCategories { get; set; }

        #endregion

        #region Commands

        public ICommand NewCategoryCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        public ICommand UpdateCategoryCommand { get; set; }

        public ICommand RemoveCategoryCommand { get; set; }

        #endregion

        private readonly ILogService _logService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryDetailViewModel _categoryDetailViewModel;

        public CategoryViewModel(
            ILogService logService,
            IDataService dataService,
            IDialogService dialogService,
            IEventAggregator eventAggregator,
            ICategoryDetailViewModel categoryDetailViewModel,
            ICategoryRepository categoryRepository)
            : base(dialogService)
        {
            _logService = logService;
            _dataService = dataService;
            _dialogService = dialogService;
            _categoryDetailViewModel = categoryDetailViewModel;
            _categoryRepository = categoryRepository;

            ProductCategories = new ObservableCollection<CategoryWrapper>();

            RefreshCommand = new DelegateCommand(Load, () => !IsBusy);
            NewCategoryCommand = new DelegateCommand(OnNewCategoryExecute, () => !IsBusy);
            UpdateCategoryCommand = new DelegateCommand<CategoryWrapper>(OnEditCategoryCommand);
            RemoveCategoryCommand = new DelegateCommand<int?>(OnRemoveCategoryExecute);

            eventAggregator.GetEvent<AfterCategorySavedEvent>().Subscribe(OnAfterCategorySaved);
        }


        public async void Load()
        {
            try
            {
                var restaurantId = _dataService.Restaurant.Id;
                var httpResponse = await ActionAsync(async () => await _categoryRepository.GetCategoriesAsync(restaurantId));
                if (httpResponse == null) return;

                if (httpResponse.IsSuccess)
                {
                    ProductCategories.Clear();
                    foreach (var category in httpResponse.Value)
                    {
                        ProductCategories.Add(new CategoryWrapper(category));
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
                ((DelegateCommand)NewCategoryCommand).RaiseCanExecuteChanged();
            }
        }

        private async void OnRemoveCategoryExecute(int? id)
        {
            try
            {
                if (!id.HasValue) return;
                var result = await _dialogService.AskQuestionAsync("¿Estas seguro de querer eliminar esta categoria?", "Elimiar Categoria");
                if (result == MessageDialogResult.Negative) return;

                var httpResponse = await ActionAsync(async () => await _categoryRepository.DeleteCategoryAsync(id.Value));
                if (httpResponse.IsSuccess)
                {
                    var category = ProductCategories.Single(x => x.Id == httpResponse.Value);
                    ProductCategories.Remove(category);
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

        private void OnNewCategoryExecute()
        {
            _categoryDetailViewModel.Open(new Category());
        }

        private void OnEditCategoryCommand(CategoryWrapper category)
        {
            if (category == null) return;
            _categoryDetailViewModel.Open(category.Model);
        }

        private void OnAfterCategorySaved(Category model)
        {
            var category = ProductCategories.SingleOrDefault(x => x.Id == model.Id);
            if (category == null) ProductCategories.Add(new CategoryWrapper(model));
            else category.Update(model);
        }
    }
}
