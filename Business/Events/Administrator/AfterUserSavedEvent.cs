using Model.Models;
using Prism.Events;

namespace Business.Events.Administrator
{
    public class AfterUserSavedEvent : PubSubEvent<User>
    {

    }
}
