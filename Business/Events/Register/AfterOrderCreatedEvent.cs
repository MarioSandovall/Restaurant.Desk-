using Model.Models;
using Prism.Events;

namespace Business.Events.Register
{
    public class AfterOrderCreatedEvent : PubSubEvent<Order>
    {

    }
}
