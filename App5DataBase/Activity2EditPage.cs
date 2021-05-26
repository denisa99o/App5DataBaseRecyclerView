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
        Spinner spinnerMagazine;
        List<Magazin> magazine;
       // EditText txtMagazin;
        public static DataBaseClass database;
        ArrayAdapter<Magazin> arrayAdapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditPage);

             txtNume = FindViewById<EditText>(Resource.Id.txtNume);
             txtCantitate = FindViewById<EditText>(Resource.Id.txtCantitate);
            //EditText txtId = FindViewById<EditText>(Resource.Id.txtId);
            Button btnEdit = FindViewById<Button>(Resource.Id.btnEdit);

            btnEdit.Click += BtnEdit_Click;
            database = new DataBaseClass();
            spinnerMagazine = FindViewById<Spinner>(Resource.Id.spinnerMagazine);
            arrayAdapter = new ArrayAdapter<Magazin>(this, Resource.Layout.support_simple_spinner_dropdown_item);
            List<Magazin> magazine = Activity2EditPage.database.GetMagazins();
            arrayAdapter.AddAll(magazine);
            arrayAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            spinnerMagazine.Adapter = arrayAdapter;

            // Create your application here

            //Ca sa imi apara in EditText datele produsului pe care vreau sa il editez
            Product product; //creez un nou produs
            string jsonProduct = Intent.Extras.GetString("product"); //ii iau datele
            product= JsonSerializer.Deserialize<Product>(jsonProduct); //il deserializez-din string face obiect
            txtNume.Text = product.Name; //pun in EditText-ul txtNume pun numele produsului 
            txtCantitate.Text = product.Cantity; 
            spinnerMagazine.SetSelection(magazine.IndexOf(magazine.Where(i => (i.Id == product.magazinId)).FirstOrDefault())); //verific Id-ul magazinului care e egal cu magazinId din tabela Produse al produsului respectiv
            //=> in spinner va aparea magazinul curent al produsului 

           // txtMagazin = FindViewById<EditText>(Resource.Id.txtMagazin);
            //txtMagazin.Text = product.magazinId.ToString();

        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {

            //throw new NotImplementedException();
            Product product = new Product();
            product.Name = txtNume.Text;
            product.Cantity = txtCantitate.Text;
            product.magazinId = arrayAdapter.GetItem(spinnerMagazine.SelectedItemPosition).Id; //asa am luat id-ul magazinelor
            string jsonString = JsonSerializer.Serialize(product);
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra("product", jsonString);
            SetResult(Result.Ok, intent);
            Finish();
        }

      
       
    }
}