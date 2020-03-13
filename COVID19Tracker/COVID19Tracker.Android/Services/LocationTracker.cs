using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using COVID19Tracker.Services;
using Xamarin.Essentials;

namespace COVID19Tracker.Droid.Services
{

    [Service]
    public class LocationTracker : Service, ILocationTracker
    {
        private Intent BatteryOptimizationsIntent;
        public event EventHandler<LocationUpdatedEventArgs> LocationUpdated = delegate { };
        private bool _Tracking = false;

        public bool Track
        {
            get
            {
                return _Tracking;
            }
            set
            {
                if(_Tracking != value)
                {
                    _Tracking = value;
                    if(_Tracking)
                    {
                        // TODO: Start service
                    } else
                    {
                        // TODO: Stop service
                    }
                }
            }
        }

        CancellationTokenSource _cts;
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            var t = new Java.Lang.Thread(() =>
            {
                _cts = new CancellationTokenSource();

                Task.Run(() =>
                {
                    try
                    {
                        var counter = new BackgroundTask(this);
                        counter.ExecutePost(_cts.Token).Wait();
                    }
                    catch (Android.Accounts.OperationCanceledException)
                    {
                    }
                    finally
                    {
                        if (_cts.IsCancellationRequested)
                        {

                        }
                    }

                }, _cts.Token);
            }
            );
            t.Start();
            return StartCommandResult.Sticky;
        }



        public void StartLocationUpdates()
        {

        }

        public void DoLocationUpdated(Xamarin.Essentials.Location location)
        {
            LocationUpdated(this, new LocationUpdatedEventArgs(new COVID19Tracker.Services.Location() 
            { Latitude = location.Latitude, Longitude = location.Longitude, Altitude = location.Altitude??0, Timestamp = location.Timestamp.ToUnixTimeSeconds() } ));
        }

        public void SetDozeOptimization(Context _parentContext)
        {
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
            {
                var packageName = _parentContext.PackageName;
                var pm = (PowerManager)_parentContext.GetSystemService(Context.PowerService);
                if (!pm.IsIgnoringBatteryOptimizations(packageName))
                {
                    BatteryOptimizationsIntent = new Intent();
                    BatteryOptimizationsIntent.AddFlags(ActivityFlags.NewTask);
                    BatteryOptimizationsIntent.SetAction(Android.Provider.Settings.ActionRequestIgnoreBatteryOptimizations);
                    BatteryOptimizationsIntent.SetData(Android.Net.Uri.Parse("package:" + packageName));
                    _parentContext.StartActivity(BatteryOptimizationsIntent);
                }
            }
        }

        public void ClearDozeOptimization()
        {
            if (null != BatteryOptimizationsIntent && Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
            {
                BatteryOptimizationsIntent.ReplaceExtras(new Bundle());
                BatteryOptimizationsIntent.SetAction("");
                BatteryOptimizationsIntent.SetData(null);
                BatteryOptimizationsIntent.SetFlags(0);
            }
        }

    }

    public class BackgroundTask
    {
        private LocationTracker _Context;

        public BackgroundTask(LocationTracker context)
        {
            _Context = context;
        }

        public async Task ExecutePost(CancellationToken token)
        {
            await Task.Run(async () => {
                while (1 < 2)
                {
                    await Task.Delay(30000);
                    UpdateLocation();
                }

            }, token);
        }

        private async void UpdateLocation()
        {
            var l = await Geolocation.GetLastKnownLocationAsync();

            _Context.DoLocationUpdated(l);
        }
    }
}