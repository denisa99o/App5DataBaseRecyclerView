using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Common;
using Android.Gms.Location;
using Android.Util;
using System.Threading.Tasks;
using System.Text.Json;

namespace App5DataBase
{
    [Activity(Label = "InsertDepozitActivity")]
    public class InsertDepozitActivity : Activity
    {
        EditText txtDenumire;
        EditText txtLongitudine;
        EditText txtLatitudine;
        Button btnInsertDepozit;
        List<Depozit> depozit;
      
        FusedLocationProviderClient fusedLocationProviderClient;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Depozit);


            // Create your application here

            txtDenumire = FindViewById<EditText>(Resource.Id.txtDenumire);
            txtLongitudine = FindViewById<EditText>(Resource.Id.txtLongitudine);
            txtLatitudine = FindViewById<EditText>(Resource.Id.txtLatitudine);

            btnInsertDepozit = FindViewById<Button>(Resource.Id.btnInsertDepozit);

            btnInsertDepozit.Click += BtnInsertDepozit_Click;
            fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(this);
            GetLastLocationFromDevice();
            Subscribe();
        }

        private void Subscribe()
        {
            LocationRequest locationRequest = new LocationRequest()
                                  .SetPriority(LocationRequest.PriorityHighAccuracy)
                                  .SetInterval(60 * 1000 * 5)
                                  .SetFastestInterval(60 * 1000 * 2);
          //  fusedLocationProviderClient.RequestLocationUpdates(locationRequest, locationCallback, null);
        }

        private void BtnInsertDepozit_Click(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
            Depozit depozit = new Depozit();
            DataBaseClass database;
            depozit.Name = txtDenumire.Text;

            depozit.Longitudine = double.Parse(txtLongitudine.Text);
            depozit.Latitudine = double.Parse(txtLatitudine.Text);
            string jsonString = JsonSerializer.Serialize(depozit);
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra("depozit", jsonString);
            SetResult(Result.Ok, intent);
            Finish();

        }

        async Task GetLastLocationFromDevice()
        {
            // This method assumes that the necessary run-time permission checks have succeeded.

            Android.Locations.Location location = await fusedLocationProviderClient.GetLastLocationAsync();

            if (location == null)
            {
                // Seldom happens, but should code that handles this scenario
            }
            else
            {
                txtLongitudine.Text = location.Longitude.ToString();
                txtLatitudine.Text = location.Latitude.ToString();
            }

            //private bool IsGooglePlayServicesInstalled()
            //{
            //    var queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            //    if (queryResult == ConnectionResult.Success)
            //    {
            //        Log.Info("MainActivity", "Google Play Services is installed on this device.");
            //        return true;
            //    }

            //    if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
            //    {
            //        // Check if there is a way the user can resolve the issue
            //        var errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
            //        Log.Error("MainActivity", "There is a problem with Google Play Services on this device: {0} - {1}",
            //                  queryResult, errorString);

            //        // Alternately, display the error to the user.
            //    }

            //    return false;
            //}
        }
    }

    public class FusedLocationProviderCallback : LocationCallback
    {
        readonly MainActivity activity;

        public FusedLocationProviderCallback(MainActivity activity)
        {
            this.activity = activity;
        }

        public override void OnLocationAvailability(LocationAvailability locationAvailability)
        {
            Log.Debug("FusedLocationProviderSample", "IsLocationAvailable: {0}", locationAvailability.IsLocationAvailable);
        }

        public override void OnLocationResult(LocationResult result)
        {
            if (result.Locations.Any())
            {
                var location = result.Locations.First();
                Log.Debug("Sample", "The latitude is :" + location.Latitude);
            }

        }
    }

}
