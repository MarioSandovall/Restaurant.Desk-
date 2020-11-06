using Model.Models;
using Prism.Events;

namespace Business.Events.Login
{
    public class EmailLoginValidEvent : PubSubEvent<User>
    {

    }
}
