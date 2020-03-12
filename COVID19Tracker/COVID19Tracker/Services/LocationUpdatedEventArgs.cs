using System;
using System.Collections.Generic;
using System.Text;

namespace COVID19Tracker.Services
{
    public class Location
    {
        public double Latitude;
        public double Longitude;
        public double Altitude;
        public long Timestamp;
    }

    public class LocationUpdatedEventArgs : EventArgs
    {
        Location location;

        public LocationUpdatedEventArgs(Location location)
        {
            this.location = location;
        }

        public Location Location
        {
            get { return location; }
        }
    }
}
