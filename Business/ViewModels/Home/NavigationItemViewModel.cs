using Business.Events.Home;
using Model.Models;
using Model.Utils;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Windows.Input;

namespace Business.ViewModels.Home
{
    public class NavigationItemViewModel : BindableBase
    {
        #region Properties

        public string Name { get; set; }
        public string Image { get; set; }
        public MenuAction Action { get; set; }
        public string Color { get; set; }

        #endregion

        #region Commands

        public ICommand NavCommand { get; set; }

        #endregion

        public NavigationItemViewModel(IEventAggregator eventAggregator, LookupItem lookupItem, string color)
        {

            Color = color;
            Name = lookupItem.Name;
            Image = lookupItem.Image;
            Action = lookupItem.Action;

            NavCommand = new DelegateCommand(() => eventAggregator.GetEvent<BeforeNavigationEvent>().Publish(Action));
        }

    }
}


