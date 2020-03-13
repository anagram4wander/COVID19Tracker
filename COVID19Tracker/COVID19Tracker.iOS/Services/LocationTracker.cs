using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreLocation;
using COVID19Tracker.Services;
using Foundation;
using UIKit;

namespace COVID19Tracker.iOS.Services
{
    public class LocationTracker : ILocationTracker
    {
        protected CLLocationManager locMgr;
        // event for the location changing
        public event EventHandler<LocationUpdatedEventArgs> LocationUpdated = delegate { };
        private bool _tracking = false;

        public bool Track
        {
            get
            {
                return _tracking;
            }
            set
            {
                if (_tracking != value)
                {
                    _tracking = value;
                    if (_tracking)
                    {
                        StartLocationUpdates();
                    } else
                    {
                        StopLocationUpdates();
                    }
                }
            }
        }

        public LocationTracker()
        {
            this.locMgr = new CLLocationManager();
            this.locMgr.PausesLocationUpdatesAutomatically = false;

            // iOS 8 has additional permissions requirements
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                locMgr.RequestAlwaysAuthorization(); // works in background
                                                     //locMgr.RequestWhenInUseAuthorization (); // only in foreground
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                locMgr.AllowsBackgroundLocationUpdates = true;
            }
        }

        public CLLocationManager LocMgr
        {
            get { return this.locMgr; }
        }

        private EventHandler<CLLocationsUpdatedEventArgs> _Event;

        public void StartLocationUpdates()
        {
            if (CLLocationManager.LocationServicesEnabled)
            {
                //set the desired accuracy, in meters
                LocMgr.DesiredAccuracy = 1;
                _Event = new EventHandler<CLLocationsUpdatedEventArgs>(OnLocationChanged);
                LocMgr.LocationsUpdated += _Event;
                LocMgr.StartUpdatingLocation();
            }
        }

        private void OnLocationChanged(object sender, CLLocationsUpdatedEventArgs e)
        {
            // fire our custom Location Updated event
            var l = e.Locations[e.Locations.Length - 1];

            Location loc = new Location() { Latitude = l.Coordinate.Latitude, Longitude = l.Coordinate.Longitude, Altitude = l.Altitude, Timestamp = (long)l.Timestamp.SecondsSince1970 };

            LocationUpdated(this, new LocationUpdatedEventArgs(loc));
        }

        public void StopLocationUpdates()
        {
            if(CLLocationManager.LocationServicesEnabled)
            {
                LocMgr.StopUpdatingLocation();
                if(_Event != null)
                {
                    LocMgr.LocationsUpdated -= _Event;
                    _Event = null;
                }
            }
        }
    }
}