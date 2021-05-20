using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Text.Json;
using AlertDialog = Android.App.AlertDialog;

namespace App5DataBase
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        public RecyclerAdapter mAdapter;
        private List<Product> mProducts;
        private List<Product> mProductsCopy;
        private List<Magazin> mMagazin;
        public IMenu menu;
        DataBaseClass db;
        private int lastPosition;
        Spinner spinnerMagazine;
        public static DataBaseClass database;
        ArrayAdapter<Magazin> arrayAdapter;
        // public Button btnInsertData;

        // public CheckBox mDeleteProduct;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource


            var listPermissions = new System.Collections.Generic.List<string>();

            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.ReadExternalStorage) != Permission.Granted)
                listPermissions.Add(Android.Manifest.Permission.ReadExternalStorage);

            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.WriteExternalStorage) != Permission.Granted)
                listPermissions.Add(Android.Manifest.Permission.WriteExternalStorage);

            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.Internet) != Permission.Granted)
                listPermissions.Add(Android.Manifest.Permission.Internet);

            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.ReadPhoneState) != Permission.Granted)
                listPermissions.Add(Android.Manifest.Permission.ReadPhoneState);

            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.AccessFineLocation) != Permission.Granted)
                listPermissions.Add(Android.Manifest.Permission.AccessFineLocation);

            // Make the request with the permissions needed...and then check OnRequestPermissionsResult() for the results
            if (listPermissions.Count > 0)
                ActivityCompat.RequestPermissions(this, listPermissions.ToArray(), 123/*a code in OnRequestPermissionsResult*/);
            else
            {
                DoStartup();
            }



            //newProduct.Name = "Spirt";
            //newProduct.Cantity = "4";
            //db.addProduct(newProduct);


            //newProduct.Name = "Ulei";
            //newProduct.Cantity = "6";
            //db.addProduct(newProduct);

            //newProduct.Name = "Usturoi";
            //newProduct.Cantity = "10";
            //db.addProduct(newProduct);



            //newMagazin.Name = "Carrefour";
            //newMagazin.Locatie = "Cluj";
            //db.addMagazin(newMagazin);


            //newMagazin.Name = "Kaufland";
            //newMagazin.Locatie = "Zalau";
            //db.addMagazin(newMagazin);

            //newMagazin.Name = "Profi";
            //newMagazin.Locatie = "Oradea";
            //db.addMagazin(newMagazin);




        }


        // Alert Dialog box-if you want to delete something press Yes - then the product is deleted
        private void MAdapter_CellClick_ButtonDelete(object sender, Product e)
        {
            // throw new NotImplementedException();
            this.RunOnUiThread(() =>
            {
                AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);

                alertDialog.SetTitle("Are you sure?");
                alertDialog.SetMessage("Do you want to delete this item?");
                alertDialog.SetPositiveButton("yes", delegate
                {
                    alertDialog.Dispose();

                    //e.Position = mAdapter.mProducts.IndexOf(e);
                    db.deleteProduct(e);
                    mAdapter.mProducts.Remove(e);
                    mProductsCopy.Remove(e);
                    mAdapter.NotifyItemRemoved(e.Position);

                    Toast.MakeText(this, " The product " + e.ToString() + " was deleted! ", ToastLength.Long).Show();
                });
                alertDialog.SetNegativeButton("NO", (IDialogInterfaceOnClickListener)null);
                alertDialog.Create();
                alertDialog.Show();
            });
        }

        //public void mEditProduct_Click(int position)
        //{
        //    Intent nextActivity = new Intent(this, typeof(Activity2EditPage));
        //    lastPosition = position;
        //    StartActivityForResult(nextActivity, 222);
        //}



        // Insert and Edit product in DataBase
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            //Edit/Update
            if (requestCode == 222 && resultCode == Result.Ok)
            {
                Product product;
                string jsonProduct = data.GetStringExtra("product");
                product = JsonSerializer.Deserialize<Product>(jsonProduct);
                product.Id = mAdapter.mProducts[lastPosition].Id;
                mProductsCopy[mProductsCopy.IndexOf(mProducts[lastPosition])] = product;
                mAdapter.mProducts[lastPosition] = product;

                db.updateProduct(product);
                //       SpinnerMagazine_ItemSelected(null, new AdapterView.ItemSelectedEventArgs(null,null,lastPosition,lastPosition));
                Filter(); //functia de filtrare

            }

            //Insert
            else if (requestCode == 333 && resultCode == Result.Ok)
            {
                Product product;
                string jsonProduct = data.GetStringExtra("product");
                product = JsonSerializer.Deserialize<Product>(jsonProduct);
                //mAdapter.mProducts.Add(product);
               

              if (!spinnerMagazine.SelectedItem.ToString().Equals("Magazine")) //daca item-ul ales este diferit de cuvantul default Magazine -> produsul este pus in lista copie
                  mProductsCopy.Add(product);

               mAdapter.mProducts.Add(product); //produsul este adaugat in adapter, pe interfata
               db.addProduct(product); //adauga produsul in baza de date

              Filter(); //functia de filtrare

              

            }
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            if (requestCode == 123)
            {
                DoStartup();
            }

        }
        public void DoStartup()
        {


            SetContentView(Resource.Layout.activity_main);

            db = new DataBaseClass();

            Product newProduct = new Product();

            Magazin newMagazin = new Magazin();
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            mProducts = db.getAllProducts();
            mProductsCopy = new List<Product>();
            mProductsCopy.AddRange(mProducts);
            mMagazin = db.getAllMagazin();

            //create our layout manager
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mAdapter = new RecyclerAdapter(mProducts, mRecyclerView, db, this);
            mAdapter.CellClick_ButtonDelete += MAdapter_CellClick_ButtonDelete;
            mAdapter.CellClick_ButtonEdit += MAdapter_CellClick_ButtonEdit;
            //  mAdapter = new RecyclerAdapter(mMagazin);
            mRecyclerView.SetAdapter(mAdapter);

            // MenuInflater.Inflate(Resource.Menu.menu_pop, menu);
            // base.OnCreateOptionsMenu(menu);
            Button btnInsertData = FindViewById<Button>(Resource.Id.btnInsertData);
            btnInsertData.Click += BtnInsertData_Click;

            database = new DataBaseClass();

            spinnerMagazine = FindViewById<Spinner>(Resource.Id.spinnerMagazine);
            arrayAdapter = new ArrayAdapter<Magazin>(this, Resource.Layout.support_simple_spinner_dropdown_item);
            List<Magazin> magazine = MainActivity.database.GetMagazins();
            Magazin magazin = new Magazin();
            magazin.Name = "Magazine";
            arrayAdapter.Add(magazin);
            arrayAdapter.AddAll(magazine);
            arrayAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            spinnerMagazine.Adapter = arrayAdapter;

            spinnerMagazine.ItemSelected += SpinnerMagazine_ItemSelected;

        }

        private void SpinnerMagazine_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            //throw new NotImplementedException();
            // product.magazinId = arrayAdapter.GetItem(spinnerMagazine.SelectedItemPosition).Id;
            Filter();

        }

        private void Filter()
        {
            List<Product> listaNouaProduse = new List<Product>();
            if (spinnerMagazine.SelectedItem.ToString().Equals("Magazine"))
                mProducts = mProductsCopy;
            else
            {
                foreach (Product product in mProductsCopy)
                {
                    if (product.magazinId == arrayAdapter.GetItem(spinnerMagazine.SelectedItemPosition).Id)
                        listaNouaProduse.Add(product);
                }
                mProducts = listaNouaProduse;
            }
            mAdapter.mProducts = mProducts;
            mAdapter.NotifyDataSetChanged();
        }




        //When the EditButton is pressed -> will apear another page with a specific interface to edit a product
        private void MAdapter_CellClick_ButtonEdit(object sender, Product e)
        {
            //throw new NotImplementedException();
            Intent nextActivity = new Intent(this, typeof(Activity2EditPage));
            // lastPosition = position;
            e.Position = mAdapter.mProducts.IndexOf(e); //here is the position
            lastPosition = e.Position;
            StartActivityForResult(nextActivity, 222);

        }

        private void BtnInsertData_Click(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
            Intent nextActivity = new Intent(this, typeof(Activity3InsertData));
            StartActivityForResult(nextActivity, 333);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // set the menu layout on Main Activity  
            MenuInflater.Inflate(Resource.Menu.menu_pop, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menuItem1:
                    {
                        // add your code  
                        return true;
                    }
                case Resource.Id.menuItem2:
                    {

                        // add your code  
                        return true;
                    }
                case Resource.Id.menuItem3:
                    {

                        // add your code  
                        return true;
                    }
            }
            return base.OnOptionsItemSelected(item);
        }




    }

    public class RecyclerAdapter : RecyclerView.Adapter, View.IOnClickListener, IFilterable
    {
        public List<Product> mProducts;
        private IMenu menu;
        public int itemPosition;
        private RecyclerView recyclerView;
        private DataBaseClass db;
        MainActivity mainActivity;
        //Open Activity2EditPage
        public event EventHandler<int> ItemClick;
        public event EventHandler<Product> CellClick_ButtonDelete;
        public event EventHandler<Product> CellClick_ButtonEdit;

        //  private List<Magazin> mMagazin;

        public RecyclerAdapter(List<Product> mProducts, RecyclerView recyclerView, DataBaseClass db, MainActivity mainActivity)
        {
            this.mProducts = mProducts;
            this.recyclerView = recyclerView;
            this.db = db;
            this.mainActivity = mainActivity;


        }

        //  public RecyclerAdapter(List<Magazin> mMagazin)
        //  {
        //     this.mMagazin = mMagazin;
        //  }






        public override int ItemCount => mProducts.Count;

        public Filter Filter => throw new NotImplementedException();

        void OnClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // throw new System.NotImplementedException();

            //all the items which appear on the Itemi.axml
            View Itemi = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.Itemi, parent, false);
            TextView txtName = Itemi.FindViewById<TextView>(Resource.Id.txtName);
            TextView txtSubject = Itemi.FindViewById<TextView>(Resource.Id.txtSubject);
            TextView txtMessage = Itemi.FindViewById<TextView>(Resource.Id.txtMessage);
            CheckBox mDeleteProduct = Itemi.FindViewById<CheckBox>(Resource.Id.btn_delete);
            CheckBox mEditProduct = Itemi.FindViewById<CheckBox>(Resource.Id.btn_edit);

            //mEditProduct.Click += MEditProduct_Click;

            MyView view = new MyView(Itemi, OnCellClick_ButtonDelete, OnCellClick_ButtonEdit) { mName = txtName, mSubject = txtSubject, mMessage = txtMessage, mDeleteProduct = mDeleteProduct, mEditProduct = mEditProduct };
            view.Update(mDeleteProduct);
            view.Update2(mEditProduct);
            return view;

        }



        //private void MEditProduct_Click(object sender, EventArgs e)
        //{
        //    //throw new NotImplementedException();
        //    mainActivity.mEditProduct_Click(itemPosition);
        //}

        private void OnCellClick_ButtonEdit(Product obj)
        {
            if (CellClick_ButtonEdit != null)
                CellClick_ButtonEdit(this, obj);
        }

        private void OnCellClick_ButtonDelete(Product obj)
        {
            if (CellClick_ButtonDelete != null)
                CellClick_ButtonDelete(this, obj);
        }



        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;

            myHolder.mName.Text = mProducts[position].Name;
            myHolder.mSubject.Text = mProducts[position].Cantity;
            myHolder.mMessage.Text = (mProducts[position].Id).ToString();

            myHolder.product = mProducts[position];
        }




        public void OnClick(View v)
        {
            //throw new NotImplementedException();
            itemPosition = recyclerView.GetChildAdapterPosition(v);
        }

        public void setProduct(JavaList<Product> product)
        {
            // throw new NotImplementedException();
            // this.mProducts = product;
        }
    }

    public class MyView : RecyclerView.ViewHolder
    {
        public View mMainView { get; set; }
        public TextView mName { get; set; }
        public TextView mSubject { get; set; }
        public TextView mMessage { get; set; }
        public object menuPopUp { get; internal set; }

        public CheckBox mDeleteProduct { get; set; }

        private Action<Product> _cellClick_ButtonDelete;

        public CheckBox mEditProduct { get; set; }

        private Action<Product> _cellClick_ButtonEdit;

        public Product product { get; set; }

        public MyView(View view, Action<Product> buttonDelete, Action<Product> buttonEdit) : base(view)
        {
            mMainView = view;
            _cellClick_ButtonDelete = buttonDelete;
            _cellClick_ButtonEdit = buttonEdit;


        }

        public void Update(CheckBox delete)
        {
            mDeleteProduct = delete;
            mDeleteProduct.Click += MDeleteProduct_Click;
        }


        private void MDeleteProduct_Click(object sender, EventArgs e)
        {
            product.Position = this.AdapterPosition;
            _cellClick_ButtonDelete(product);
        }

        public void Update2(CheckBox edit)
        {
            mEditProduct = edit;
            mEditProduct.Click += MEditProduct_Click;
        }

        private void MEditProduct_Click(object sender, EventArgs e)
        {
            product.Position = this.AdapterPosition;
            _cellClick_ButtonEdit(product);

        }
    }
}