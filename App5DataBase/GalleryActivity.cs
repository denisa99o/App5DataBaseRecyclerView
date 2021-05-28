using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using Android;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Plugin.Media;

namespace App5DataBase
{
    [Activity(Label = "GalleryActivity")]
    public class GalleryActivity : Activity
    {
       public RecyclerView mRecyclerView;
       private RecyclerView.LayoutManager mLayoutManager;
       public RecyclerAdapterPhoto mAdapter;
        public static List<Bitmap> mPhotoAlbum = new List<Bitmap>(); //daca e statica se creeaza o singura data-> se poate apela
        Button btnCapture;
        ImageView thisImageViewCapture;

        readonly string[] permissionGroup =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera
        };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

         
           

            if(mPhotoAlbum.Count<1)
            foreach(string photoPath in GetAllImagePath())
            {
                Bitmap Source = BitmapFactory.DecodeFile(photoPath); //TRANSFORMA IN IMAGINE-mod de stocare a imaginii
                mPhotoAlbum.Add(Source);
            }
            
          
            //mPhotoAlbum=new PhotoAlbum();
            SetContentView(Resource.Layout.Gallery);
            // Create your application here

            //Get our RecyclerView layout
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            //create our layout manager

            mLayoutManager = new GridLayoutManager(this, 3);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            //Plug in my adapter
             mAdapter = new RecyclerAdapterPhoto(mPhotoAlbum);
            mRecyclerView.SetAdapter(mAdapter);
            mAdapter.CellClick_Image += MAdapter_CellClick_Image;

            btnCapture = FindViewById<Button>(Resource.Id.btnCapture);
            thisImageViewCapture = FindViewById<ImageView>(Resource.Id.thisImageViewCapture);

            btnCapture.Click += BtnCapture_Click;
            RequestPermissions(permissionGroup, 123);


        }

        private void BtnCapture_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            TakePhoto();

        }

       // we create a new method to capture the image-an asynchronous method
        async void TakePhoto()
        {
            await CrossMedia.Current.Initialize(); //initialize the package
            //capture image
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                //set size
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                CompressionQuality = 40,
                Name = "myimage.jpg",
                Directory = "sample"

            });

            if (file == null)
            {
                return;
            }

            //convert file to byte array and set the resulting bitmap to imageView
            byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);
            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
            thisImageViewCapture.SetImageBitmap(bitmap);

        }


        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        // {
       // Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
       // }

        private void MAdapter_CellClick_Image(object sender, Bitmap e)
        {
            // throw new NotImplementedException();
            Intent nextActivity = new Intent(this, typeof(SinglePhotoActivity));
            //nu merge daca serializam
            // string jsonProduct = JsonSerializer.Serialize<Bitmap>(e); //Seriallizez produsul selecat-ii iau toate datele
            //trimit eticheta produsului "Key" si data care vreau sa fie transferata, adica toate datele despre produs

            //luam pozitia imaginii din album
            int lastposition = mPhotoAlbum.IndexOf(e);
            
            //byte[] bytes;
            //using(MemoryStream ms=new MemoryStream())
            //{
            //    BinaryFormatter formatter = new BinaryFormatter();
            //    formatter.Serialize(ms, e);
            //    bytes = ms.ToArray();
            //}
            nextActivity.PutExtra("photo", lastposition); 
            StartActivity(nextActivity); 
        }



        public List<string> GetAllImagePath()
    {

        string[] imagePaths = Directory.GetFiles(Globals.RootDirectory, "*.jpg", SearchOption.AllDirectories);

        return imagePaths.ToList();
    }
    }



    

    public class RecyclerAdapterPhoto : RecyclerView.Adapter
    {
        public List<Bitmap> mPhotoAlbum;
        public event EventHandler<Bitmap> CellClick_Image;
        public RecyclerAdapterPhoto(List< Bitmap> photoAlbum)
        {
             mPhotoAlbum = photoAlbum;
         }

        public override int ItemCount => mPhotoAlbum.Count;
      

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            //throw new NotImplementedException();
            PhotoViewHolder myHolder = holder as PhotoViewHolder;
            //    myHolder.mImage.SetImageResource(mPhotoAlbum[position].PhotoID);
            //myHolder.mImage.SetImageLocation.Resource(ic_launcher); ca sa iau o imagine existenta-ceva de genul era
            myHolder.mImage.SetImageBitmap(mPhotoAlbum[position]);
           myHolder.image = mPhotoAlbum[position];
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // throw new NotImplementedException();
            View ItemiPhoto = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ItemiPhoto, parent, false);
            ImageView imageview = ItemiPhoto.FindViewById<ImageView>(Resource.Id.imageView);

          //  imageview.Click += Imageview_Click;

            PhotoViewHolder view = new PhotoViewHolder(ItemiPhoto, OnCellClick_Image) { mImage = imageview};
            view.Update(imageview);
            return view;
        }

        private void OnCellClick_Image(Bitmap obj)
        {
            
            //throw new NotImplementedException();
            if (CellClick_Image != null)
                CellClick_Image(this, obj);
        }

        public void OnClick(View v)
        {
            throw new NotImplementedException();
        }

        //private void Imageview_Click(object sender, EventArgs e)
        //{
        //   // throw new NotImplementedException();
        // //  Intent intent=new Intent(this,GetObject)
        //         Intent nextActivity = new Intent(this,typeof(SinglePhotoActivity));
        //         StartActivity(nextActivity);

        //}
    }
    public class PhotoViewHolder : RecyclerView.ViewHolder
    {
        public View mMainView { get; set; }
        public ImageView mImage { get;  set; }

        public Bitmap image { get; set; }
        private Action<Bitmap> _cellClick_Image;
        public PhotoViewHolder(View view, Action<Bitmap> buttonImage) : base(view)
        {
            mMainView = view;
            _cellClick_Image = buttonImage;
        }

        public void Update(ImageView splash)
        {
            mImage = splash;
            mImage.Click += MImage_Click;
        }

        private void MImage_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            
            _cellClick_Image(image);
        }

      

    }


 
}