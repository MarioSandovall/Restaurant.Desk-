using Business.Events;
using Business.Extensions;
using Business.Interfaces.Administrator;
using Business.Wrappers;
using Microsoft.Win32;
using Model.Models;
using Prism.Commands;
using Prism.Events;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Business.ViewModels.Administrator
{
    public class CategoryDetailViewModel : ModalBase, ICategoryDetailViewModel
    {

        #region Properties

        public int CategoryId { get; set; }

        public bool IsNewCatetory { get; set; }

        private CategoryWrapper _category;
        public CategoryWrapper Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        #endregion

        #region Commands     

        public ICommand LoadImageCommand { get; set; }

        public ICommand DeleteImageCommand { get; set; }

        #endregion

        private readonly ILogService _logService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private readonly ICategoryRepository _categoryRepository;
        public CategoryDetailViewModel(
            ILogService logService,
            IDataService dataService,
            IDialogService dialogService,
            IEventAggregator eventAggregator,
            ICategoryRepository categoryRepository)
            : base(dialogService,eventAggregator)
        {
            _logService = logService;
            _dataService = dataService;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _categoryRepository = categoryRepository;

            LoadImageCommand = new DelegateCommand(OnLoadImageExecute);
            DeleteImageCommand = new DelegateCommand(OnDeleteImageExecute);
        }

        public void Open(Category product)
        {
            var model = new Category();
            model.Update(product);

            InitializeCategory(model);
            IsOpen = true;
        }

        private void InitializeCategory(Category model)
        {
            Category = new CategoryWrapper(model);

            Category.PropertyChanged -= CategoryWrapperOnPropertyChanged;
            Category.PropertyChanged += CategoryWrapperOnPropertyChanged;

            Category.Name = model.Name;
            Category.Description = model.Description;
            Category.RestaurantId = _dataService.Restaurant.Id;
            Category.Image = model.Image ?? ImageExtension.CategoryImg.ImgUrlToByteArray();

            ((DelegateCommand)OkCommand).RaiseCanExecuteChanged();
        }

        private void CategoryWrapperOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Category.HasErrors))
            {
                ((DelegateCommand)OkCommand).RaiseCanExecuteChanged();
            }
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
                Category.Image = openFile.FileName.ImgUrlToByteArray();
            }
        }

        private void OnDeleteImageExecute()
        {
            Category.Image = ImageExtension.CategoryImg.ImgUrlToByteArray();
        }

        protected override async void OnOkExecute()
        {
            try
            {
                var httpResponse = await ActionAsync(async () => await _categoryRepository.SaveCategoryAsync(Category.Model));
                if (httpResponse.IsSuccess)
                {
                    _eventAggregator.GetEvent<AfterCategorySavedEvent>().Publish(httpResponse.Value);
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

        protected override bool OnOkCanExecute() => Category != null && !Category.HasErrors;

        protected override void ClearValues() => Category = null;


    }
}
