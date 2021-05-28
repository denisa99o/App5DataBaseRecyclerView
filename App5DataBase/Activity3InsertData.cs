using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace App5DataBase
{
    [Activity(Label = "Activity3InsertData")]
    public class Activity3InsertData : Activity
    {

        EditText txtNume;
        EditText txtCantitate;
        EditText txtId;
        Spinner spinnerMagazine;
        List<Magazin> magazine;
        public static DataBaseClass database;
        ArrayAdapter<Magazin> arrayAdapter;

        //private static readonly int NOTIFICATION_ID = 1000;
        //private static readonly string CHANNEL_ID ="location_notification";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InsertData);

            txtNume = FindViewById<EditText>(Resource.Id.txtNume);
            txtCantitate = FindViewById<EditText>(Resource.Id.txtCantitate);

           // txtId = FindViewById<EditText>(Resource.Id.txtId);
            Button btnInsert = FindViewById<Button>(Resource.Id.btnInsert);

            btnInsert.Click += BtnInsert_Click;
            database = new DataBaseClass();

            spinnerMagazine = FindViewById<Spinner>(Resource.Id.spinnerMagazine);
            arrayAdapter = new ArrayAdapter<Magazin>(this, Resource.Layout.support_simple_spinner_dropdown_item);
            List<Magazin> magazine = Activity3InsertData.database.GetMagazins();
            arrayAdapter.AddAll(magazine); //adaug magazine in spinner
            arrayAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            
           spinnerMagazine.Adapter = arrayAdapter; 
            // Create your application here


        }
    
        private void BtnInsert_Click(object sender, EventArgs e)
        {
           

            // throw new NotImplementedException();
            Product product = new Product();
            product.Name = txtNume.Text;
            product.Cantity = txtCantitate.Text;
            product.magazinId = arrayAdapter.GetItem(spinnerMagazine.SelectedItemPosition).Id; //asa am luat id-ul magazinelor

          //  product.Id = int.Parse(txtId.Text);
            string jsonString = JsonSerializer.Serialize(product);
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra("product", jsonString);
            SetResult(Result.Ok, intent);
            Finish();

            //var1-NOTIFICARI--nu poate gasi canalul
            // var builder = new NotificationCompat.Builder(this, CHANNEL_ID).SetAutoCancel(true)
            //.SetContentTitle("Button Clicked")
            //.SetSmallIcon(Resource.Drawable.abc_ic_star_black_36dp)
            //.SetContentText("A new element was inserted in the list");

            // var nmc = NotificationManagerCompat.From(this);
            // nmc.Notify(NOTIFICATION_ID, builder.Build());


            //var2-NOTIFICARI-nu poate gasi canalul
          //  NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
          //.SetAutoCancel(true)
          //.SetContentTitle("Button Clicked")
          //.SetSmallIcon(Resource.Drawable.navigation_empty_icon)
          //.SetContentText("A new element was inserted in the list");

          //  NotificationManager manager = (NotificationManager)GetSystemService(Context.NotificationService);
          // manager.Notify(NOTIFICATION_ID, builder.Build());

        }
    }
}