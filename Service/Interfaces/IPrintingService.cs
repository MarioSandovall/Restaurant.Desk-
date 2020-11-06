using Model.Dtos;
using System.Collections.Generic;

namespace Service.Interfaces
{
    public interface IPrintingService
    {
        string DefaultPrinter { get; }
        bool IsActive { get; }
        void Start();
        void Print(ICollection<TicketDetailDto> details);
        void SetPrinter(string printer);
        void SetActive(bool isActive);
        IEnumerable<string> GetPrinters();
    }
}
