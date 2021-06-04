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
using static Android.Gms.Maps.GoogleMap;
using Android.Gms.Location;
using Android.Util;

namespace App5DataBase
{

   
    [Activity(Label = "MapActivity")]
    public class MapActivity : Activity, IOnMapReadyCallback, IInfoWindowAdapter
    {
        private Button btnNormal;
        private Button btnHybrid;
        private Button btnSatellite;
        private Button btnTerrain;
        private GoogleMap mMap;
        private DataBaseClass database;
        JavaList<Depozit> mDepozit;


        public void OnMapReady(GoogleMap googleMap)
        {
            // throw new NotImplementedException();
            mMap = googleMap;
           // googleMap.MyLocationEnabled = true;
            LatLng latlng = new LatLng(46.770439, 23.591423);
            LatLng latlng2 = new LatLng(47, 23.591423);

            CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(latlng,10); //pune camera pe marker
            googleMap.MoveCamera(cameraUpdate);
            //we create an instance of Marker Options
            //MarkerOptions markerOptions = new MarkerOptions();
            //markerOptions.SetPosition(latlng);
            //markerOptions.SetTitle("My location");
            //markerOptions.SetSnippet("Cluj-Napoca");
            //markerOptions.Draggable(true); //pot misca marker-ul pe harta

            //add another marker


            //  How to design the marker - asa obtin un marcaj albastru
            //  var bmDescriptor = BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan);
            //  markerOptions.InvokeIcon(bmDescriptor);
            //Marker1
           // googleMap.AddMarker(markerOptions); //To add a marker

            foreach (Depozit d in mDepozit)
           {
               MarkerOptions markerOptions = new MarkerOptions();
             //  var random1 = new Random();
            //    var random2 = new Random();
            //    double randomnumber1 = random1.Next();
            //    double randomnumber2 = random2.Next();

             markerOptions.SetPosition(new LatLng(d.Latitudine,d.Longitudine ));
                markerOptions.SetTitle(d.Name);
                googleMap.AddMarker(markerOptions); //To add a marker
            }

           

            ////Optional
            googleMap.UiSettings.ZoomControlsEnabled = true; // +/- zoom in, zoom out
            googleMap.UiSettings.CompassEnabled = true; //display compass
           // googleMap.MoveCamera(CameraUpdateFactory.ZoomIn());  // afiseaza harta de mai aproape

            googleMap.MapType = GoogleMap.MapTypeHybrid; //satellite map

            googleMap.MyLocationEnabled = true;


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

            // Marker 2
            //googleMap.AddMarker(new MarkerOptions()
            //    .SetPosition(latlng2)
            //    .SetTitle("Marker 2")
            //    .SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueBlue)));

            mMap.MarkerClick += MMap_MarkerClick;
                
            mMap.MarkerDragEnd += MMap_MarkerDragEnd;
            mMap.SetInfoWindowAdapter(this);

        }

        private void MMap_MarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            // throw new NotImplementedException();
            LatLng pos = e.Marker.Position;
            mMap.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(pos, 10));

        }

        private void MMap_MarkerDragEnd(object sender, GoogleMap.MarkerDragEndEventArgs e)
        {
            //throw new NotImplementedException();
     
                LatLng pos = e.Marker.Position;
                Console.WriteLine(pos.ToString());

            
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
            mDepozit = new JavaList<Depozit>();

            foreach (Depozit d in database.getAllDepozite())
               mDepozit.Add(d);

            btnNormal = FindViewById<Button>(Resource.Id.btnNormal);
            btnHybrid = FindViewById<Button>(Resource.Id.btnHybrid);
            btnSatellite = FindViewById<Button>(Resource.Id.btnSatellite);
            btnTerrain = FindViewById<Button>(Resource.Id.btnTerrain);

            btnNormal.Click += BtnNormal_Click;
            btnHybrid.Click += BtnHybrid_Click;
            btnSatellite.Click += BtnSatellite_Click;
            btnTerrain.Click += BtnTerrain_Click;

           

        }
      
        private void BtnTerrain_Click(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
            mMap.MapType = GoogleMap.MapTypeTerrain;
        }

        private void BtnSatellite_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            mMap.MapType = GoogleMap.MapTypeSatellite;
        }

        private void BtnHybrid_Click(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
            mMap.MapType = GoogleMap.MapTypeHybrid;
        }

        private void BtnNormal_Click(object sender, EventArgs e)
        {
            //  throw new NotImplementedException();
            mMap.MapType = GoogleMap.MapTypeNormal;
        }

        public View GetInfoContents(Marker marker)
        {
            //throw new NotImplementedException();
            return null;
        }

        public View GetInfoWindow(Marker marker)
        {
            // throw new NotImplementedException();
            View view = LayoutInflater.Inflate(Resource.Layout.info_window, null, false);
            view.FindViewById<TextView>(Resource.Id.txtName).Text = "Xamarin";

            return view;
        }
    }
}