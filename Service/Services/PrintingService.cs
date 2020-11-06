using Model.Dtos;
using Model.Models;
using Service.Interfaces;
using Service.Utils;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;

namespace Service.Services
{
    public class PrintingService : IPrintingService
    {

        public bool IsActive { get; private set; }

        public ICollection<string> Printers { get; set; }

        public string DefaultPrinter { get; private set; }

        private readonly IDataService _dataService;
        private readonly IConfigService _configService;
        public PrintingService(
            IDataService dataService,
            IConfigService configService)
        {
            _dataService = dataService;
            _configService = configService;

            Printers = new List<string>();
        }

        public void Start()
        {
            var config = _configService.GetConfiguration();
            IsActive = config.IsPrinterActivated;

            Printers.Clear();
            foreach (var printer in PrinterSettings.InstalledPrinters)
            {
                Printers.Add(printer.ToString());
            }

            IsActive = Printers.Contains(config.Printer);
            if (IsActive) DefaultPrinter = config.Printer;
        }

        public void SetPrinter(string printer)
        {
            var propertyName = ObjectHelper.PropertyName<ConfigApp>(x => x.Printer);
            _configService.UpdateProperty(propertyName, printer);
            DefaultPrinter = printer;
        }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;
            var propertyName = ObjectHelper.PropertyName<ConfigApp>(x => x.IsPrinterActivated);
            _configService.UpdateProperty(propertyName, isActive);
        }

        public void Print(ICollection<TicketDetailDto> details)
        {
            if (details.Count == 0 || !IsActive) return;

            var office = _dataService.CurrentOffice;
            var restaurant = _dataService.Restaurant;
            using (var document = PurchaseDocument.Generate(details, office, restaurant))
            {
                document.PrinterSettings.PrinterName = DefaultPrinter;
                document.Print();
            }
        }

        public IEnumerable<string> GetPrinters() => Printers;

    }
}
