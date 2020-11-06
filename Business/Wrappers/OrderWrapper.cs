using Business.Events.Register;
using Model.Models;
using Model.Utils;
using Prism.Commands;
using Prism.Events;
using System;
using System.Windows.Input;

namespace Business.Wrappers
{
    public class OrderWrapper : ModelWrapper<Order>
    {


        public int Id => Model.Id;
        public DateTime? EndDate => Model.EndDate;
        public int OrderNumber => Model.OrderNumber;
        public DateTime StartDate => Model.StartDate;
        public OrderStatusEnum Status => Model.Status;
        public int CashRegisterId => Model.CashRegisterId;
        public string ShortName => Name.Length >= 10 ? $"{Name.Substring(0, 7)}..." : Name;

        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }


  
        public ICommand SelectCommand { get; set; }

       

        public OrderWrapper(Order model) : base(model)
        {

        }

        public OrderWrapper(Order model, IEventAggregator eventAggregator) : base(model)
        {
            SelectCommand = new DelegateCommand(() =>
            {
                eventAggregator.GetEvent<SelectOrderEvent>().Publish(Model);
            });
        }
    }
}
