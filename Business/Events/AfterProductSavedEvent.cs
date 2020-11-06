﻿using Model.Models;
using Prism.Events;

namespace Business.Events
{
    public class AfterProductSavedEvent : PubSubEvent<Product>
    {

    }
}
