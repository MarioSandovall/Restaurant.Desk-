using Business.Interfaces;
using Prism.Events;

namespace Business.Events.Home
{
    public class AfterNavigationEvent : PubSubEvent<AfterNavigationEventArgs>
    {

    }

    public class AfterNavigationEventArgs
    {
        public bool IsMenuVisible { get; set; }
        public bool IsHamburgerMenuOpen { get; set; }
        public IViewModelBase ViewModel { get; set; }
    }
}
