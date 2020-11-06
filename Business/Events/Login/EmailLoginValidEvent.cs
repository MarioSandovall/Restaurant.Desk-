using Model.Models.Login;
using Prism.Events;

namespace Business.Events.Login
{
    public class EmailLoginValidEvent : PubSubEvent<UserAccount>
    {

    }
}
