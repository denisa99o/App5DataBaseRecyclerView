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
using Android.Gms.Maps;
using Android.Gms.Maps.Model;


namespace App5DataBase
{
    [Activity(Label = "MapActivity")]
    public class MapActivity : Activity, IOnMapReadyCallback
    {
        private DataBaseClass database;
        JavaList<Magazin> mMagazine;

        public void OnMapReady(GoogleMap googleMap)
        {
            // throw new NotImplementedException();
            MarkerOptions markerOptions = new MarkerOptions();
            googleMap.MyLocationEnabled = true;
            markerOptions.SetPosition(new LatLng(16.03, 108));
            markerOptions.SetTitle("My Position");
            //  How to design the marker - asa obtin un marcaj albastru
            //  var bmDescriptor = BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan);
            //  markerOptions.InvokeIcon(bmDescriptor);
            googleMap.AddMarker(markerOptions); //To add a marker
            //foreach (Magazin m in mMagazine)
            //{
            //    MarkerOptions markerOptions = new MarkerOptions();
            //    var random1 = new Random();
            //    var random2 = new Random();
            //    double randomnumber1 = random1.Next();
            //    double randomnumber2 = random2.Next();

            //    markerOptions.SetPosition(new LatLng(randomnumber1, randomnumber2));
            //    markerOptions.SetTitle(m.Name);
            //    googleMap.AddMarker(markerOptions); //To add a marker
            //}

           

            ////Optional
            googleMap.UiSettings.ZoomControlsEnabled = true; // +/- zoom in, zoom out
            googleMap.UiSettings.CompassEnabled = true; //display compass
           // googleMap.MoveCamera(CameraUpdateFactory.ZoomIn());  // afiseaza harta de mai aproape

            googleMap.MapType = GoogleMap.MapTypeHybrid; //satellite map




            //Optional2

            //LatLng location = new LatLng(50.897778, 3.013333);

            //unghiul de la care putem vedea harta

            //CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            //builder.Target(new LatLng(16.03, 108));
            //builder.Zoom(18); //zoom level
            //builder.Bearing(155); // The bearing is the compass measurement clockwise from North
            //builder.Tilt(65); //The Tilt property controls the viewing angle and specifies an angle of 25 degrees from the vertical

            //CameraPosition cameraPosition = builder.Build();

            //CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            //googleMap.MoveCamera(cameraUpdate);

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Map);
            var mapFrag = MapFragment.NewInstance();
            FragmentManager.BeginTransaction()
                                    .Add(Resource.Id.map, mapFrag, "map_fragment")
                                    .Commit();

            // Create your application here
            // MapFragment mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mapFrag.GetMapAsync(this);

            database = new DataBaseClass();
            mMagazine = new JavaList<Magazin>();

            foreach (Magazin m in database.getAllMagazin())
               mMagazine.Add(m);
        }
    }
}