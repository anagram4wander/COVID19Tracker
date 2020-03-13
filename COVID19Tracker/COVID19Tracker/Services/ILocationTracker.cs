using System;
using System.Collections.Generic;
using System.Text;

namespace COVID19Tracker.Services
{
    public interface ILocationTracker
    {
        void StartLocationUpdates();
        event EventHandler<LocationUpdatedEventArgs> LocationUpdated;
        bool Track { get; set; }
    }
}
