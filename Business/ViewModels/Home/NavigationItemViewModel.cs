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

        public NavigationItemViewModel(IEventAggregator ea, LookupItem model, string color)
        {

            Name = model.Name;
            Action = model.Action;
            Image = model.Image;
            Color = color;

            NavCommand = new DelegateCommand(() => ea.GetEvent<BeforeNavigationEvent>().Publish(Action));
        }

    }
}


