using System;
using System.Collections.Generic;

namespace PrettyHair
{
    public delegate void OnOrderReadyToPackHandler(object sender, OnOrderReadyToPackEventArgs e);
    public delegate void OnAmountNotEnoughHandler(object sender, OnAmountNotEnoughEventArgs e);
    public delegate void OnOrderIsPackedHandler(object sender, OnOrderIsPackedEventArgs e);

    public class OnOrderReadyToPackEventArgs : EventArgs
    {
        public string EmailMessage { get; set; }
    }

    public class OnAmountNotEnoughEventArgs : EventArgs
    {
        public string EmailMessage { get; set; }
    }

    public class OnOrderIsPackedEventArgs : EventArgs
    {
        public List<OrderLine> ListOfOrderLines { get; set; }
    }
}
