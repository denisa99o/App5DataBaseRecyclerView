using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App5DataBase
{
    [Activity(Label = "Activity2EditPage")]
    public class Activity2EditPage : Activity
    {
        EditText txtNume;
        EditText txtCantitate;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditPage);

             txtNume = FindViewById<EditText>(Resource.Id.txtNume);
             txtCantitate = FindViewById<EditText>(Resource.Id.txtCantitate);
            //EditText txtId = FindViewById<EditText>(Resource.Id.txtId);
            Button btnEdit = FindViewById<Button>(Resource.Id.btnEdit);

            btnEdit.Click += BtnEdit_Click;

           

            // Create your application here
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {

            //throw new NotImplementedException();
            Product product = new Product();
            product.Name = txtNume.Text;
            product.Cantity = txtCantitate.Text;
            string jsonString = JsonSerializer.Serialize(product);
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra("product", jsonString);
            SetResult(Result.Ok, intent);
            Finish();
        }

      
       
    }
}