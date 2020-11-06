using Autofac;
using Business.Interfaces.Administrator;
using Business.Interfaces.Home;
using Business.Interfaces.Login;
using Business.Interfaces.Operations;
using Business.Interfaces.Register;
using Business.Utils.Operations;
using Business.ViewModels.Administrator;
using Business.ViewModels.Home;
using Business.ViewModels.Login;
using Business.ViewModels.Main;
using Business.ViewModels.Register;
using Prism.Events;
using Repository.Interfaces;
using Repository.Repositories;
using Service.Interfaces;
using Service.Services;

namespace Business.Startup
{
    public class Bootstrapper
    {
        public IContainer Container;
        private readonly ContainerBuilder _builder;
        public Bootstrapper()
        {
            if (_builder == null) _builder = new ContainerBuilder();
        }

        public static Bootstrapper Instance { get; } = new Bootstrapper();

        public IContainer BootStrap()
        {
            RegisterTypes();
            return Container = _builder.Build();
        }

        private void RegisterTypes()
        {

            //Events 
            _builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            //Services
            _builder.RegisterType<LogService>().As<ILogService>();
            _builder.RegisterType<ConfigService>().As<IConfigService>();
            _builder.RegisterType<WindowService>().As<IWindowService>();
            _builder.RegisterType<DialogService>().As<IDialogService>();
            _builder.RegisterType<LookupService>().As<ILookupServices>();
            _builder.RegisterType<WebService>().As<IWebService>().SingleInstance();
            _builder.RegisterType<DataService>().As<IDataService>().SingleInstance();
            _builder.RegisterType<PrintingService>().As<IPrintingService>().SingleInstance();

            //ViewModels
            _builder.RegisterType<MainViewModel>().AsSelf();

            _builder.RegisterType<TableViewModel>().As<ITableViewModel>();
            _builder.RegisterType<OfficeViewModel>().As<IOfficeViewModel>();
            _builder.RegisterType<HomeViewModel>().As<IHomeViewModel>().SingleInstance();
            _builder.RegisterType<AdminViewModel>().As<IAdminViewModel>().SingleInstance();
            _builder.RegisterType<ProductViewModel>().As<IProductViewModel>().SingleInstance();
            _builder.RegisterType<CashRegisterInfoViewModel>().As<ICashRegisterInfoViewModel>();
            _builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>().SingleInstance();
            _builder.RegisterType<CashRegisterViewModel>().As<ICashRegisterViewModel>().SingleInstance();

            _builder.RegisterType<UserViewModel>().As<IUserViewModel>();
            _builder.RegisterType<OrderViewModel>().As<IOrderViewModel>();
            _builder.RegisterType<LoginViewModel>().As<ILoginViewModel>();
            _builder.RegisterType<StartUpViewModel>().As<IStartUpViewModel>();
            _builder.RegisterType<SettingsViewModel>().As<ISettingsViewModel>();
            _builder.RegisterType<CategoryViewModel>().As<ICategoryViewModel>();
            _builder.RegisterType<OrderListViewModel>().As<IOrderListViewModel>();
            _builder.RegisterType<RestaurantViewModel>().As<IRestaurantViewModel>();
            _builder.RegisterType<EmailLoginViewModel>().As<IEmailLoginViewModel>();
            _builder.RegisterType<OrderDetailViewModel>().As<IOrderDetailViewModel>();
            _builder.RegisterType<ProductListViewModel>().As<IProductListViewModel>();
            _builder.RegisterType<SearchProductViewModel>().As<ISearchProductViewModel>();
            _builder.RegisterType<OfficeChooserViewModel>().As<IOfficeChooserViewModel>();
            _builder.RegisterType<UserInformationViewModel>().As<IUserInformationViewModel>();

            //Modals
            _builder.RegisterType<PrinterViewModel>().As<IPrinterViewModel>();
            _builder.RegisterType<PrintTicketViewModel>().As<IPrintTicketViewModel>();
            _builder.RegisterType<OrderViewModel>().As<IOrderViewModel>().SingleInstance();
            _builder.RegisterType<ChargeViewModel>().As<IChargeViewModel>().SingleInstance();
            _builder.RegisterType<UserDetailViewModel>().As<IUserDetailViewModel>().SingleInstance();
            _builder.RegisterType<InitialCashRegisterViewModel>().As<IInitialCashRegisterViewModel>();
            _builder.RegisterType<TableDetailViewModel>().As<ITableDetailViewMoldel>().SingleInstance();
            _builder.RegisterType<OfficeDetailViewModel>().As<IOfficeDetailViewModel>().SingleInstance();
            _builder.RegisterType<ProductDetailViewModel>().As<IProductDetailViewModel>().SingleInstance();
            _builder.RegisterType<CategoryDetailViewModel>().As<ICategoryDetailViewModel>().SingleInstance();
            _builder.RegisterType<SoftwareUpdateViewModel>().As<ISoftwareUpdateViewModel>().SingleInstance();


            ////Repositories
            _builder.RegisterType<UserRepository>().As<IUserRepository>();
            _builder.RegisterType<OrderRepository>().As<IOrderRepository>();
            _builder.RegisterType<TableRepository>().As<ITableRepository>();
            _builder.RegisterType<ProductRepository>().As<IProductRepository>();
            _builder.RegisterType<CategoryRepository>().As<ICategoryRepository>();
            _builder.RegisterType<RestaurantRepository>().As<IRestaurantRepository>();
            _builder.RegisterType<CashRegisterRepository>().As<ICashRegisterRepository>();

            //Operations
            _builder.RegisterType<CashRegisterOperation>().As<ICashRegisterOperation>();
        }
    }
}
